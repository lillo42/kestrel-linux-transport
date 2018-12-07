using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace RedHat.AspNetCore.Server.Kestrel.Transport.Linux
{
    internal sealed class AcceptThread : ITransportActionHandler
    {
        private enum State
        {
            Initial,
            Started,
            Stopped
        }

        private readonly Socket _socket;
        private readonly List<int> _handlers;
        private readonly object _gate = new object();
        
        private State _state;
        private TaskCompletionSource<object> _stoppedTcs;
        private Thread _thread;
        private PipeEndPair _pipeEnds;
        

        public AcceptThread(Socket socket)
        {
            _socket = socket;
            _state = State.Initial;
            _handlers = new List<int>();
        }

        public int CreateReceiveSocket()
        {
            lock (_gate)
            {
                if (_state != State.Initial)
                {
                    throw new InvalidOperationException($"Invalid operation: {_state}");
                }

                var pair = Socket.CreatePair(AddressFamily.Unix, SocketType.Stream,
                    ProtocolType.Unspecified, false);

                _handlers.Add(pair.Socket1);
                
                return pair.Socket2;
            }
        }

        public Task BindAsync()
        {
            lock (_gate)
            {
                if (_state != State.Initial)
                {
                    throw new InvalidOperationException($"Invalid operation: {_state}");
                }

                _stoppedTcs = new TaskCompletionSource<object>();
                
                try
                {
                    _pipeEnds = PipeEnd.CreatePair(false);
                    _thread = new Thread(AcceptThreadStart);
                    _thread.Start();
                    _state = State.Started;
                }
                catch (Exception)
                {
                    _state = State.Stopped;
                    _stoppedTcs = null;
                    _socket.Dispose();
                    Cleanup();
                    throw;
                }
            }

            return Task.CompletedTask;
        }

        public Task UnbindAsync()
        {
            lock (_gate)
            {
                if (_state != State.Stopped)
                {
                    _state = State.Stopped;

                    try
                    {
                        _pipeEnds.WriteEnd?.WriteByte(0);
                    }
                    catch (IOException ex) when (ex.HResult == PosixResult.EPIPE)
                    {
                        
                    }
                    catch (ObjectDisposedException)
                    {
                        
                    }
                }

                return _stoppedTcs?.Task ?? Task.CompletedTask;
            }
        }

        public Task StopAsync()
            => UnbindAsync();

        private void Cleanup()
        {
            _pipeEnds.Dispose();
            
            foreach (var handler in _handlers)
            {
                IOInterop.Close(handler);
            }
        }

        private unsafe void AcceptThreadStart(object state)
        {
            try
            {
                var socket = _socket;
                using (socket)
                {
                    using (EPoll epoll = EPoll.Create())
                    {
                        int epollFd = epoll.DangerousGetHandle().ToInt32();
                        const int acceptKey = 0;
                        const int pipeKey = 1;
                        // accept socket
                        epoll.Control(EPollOperation.Add, _socket, EPollEvents.Readable,
                            new EPollData {Int1 = acceptKey, Int2 = acceptKey});
                        // add pipe
                        epoll.Control(EPollOperation.Add, _pipeEnds.ReadEnd, EPollEvents.Readable,
                            new EPollData {Int1 = pipeKey, Int2 = pipeKey});

                        const int EventBufferLength = 1;
                        int notPacked = !EPoll.PackedEvents ? 1 : 0;
                        var buffer = stackalloc int[EventBufferLength * (3 + notPacked)];
                        int* key = &buffer[2];

                        bool running = true;
                        int nextHandler = 0;
                        var handlers = _handlers;
                        
                        do
                        {
                            int numEvents = EPollInterop.EPollWait(epollFd, buffer, EventBufferLength,
                                EPoll.TimeoutInfinite).Value;


                            if (numEvents != 1)
                            {
                                continue;
                            }

                            if (*key == acceptKey)
                            {
                                var handler = handlers[nextHandler];
                                nextHandler = (nextHandler + 1) % handlers.Count;
                                socket.TryAcceptAndSendHandleTo(handler);
                            }
                            else
                            {
                                running = false;
                            }
                            
                        } while (running);
                    }
                }

                _stoppedTcs.TrySetResult(null);
            }
            catch (Exception e)
            {
                _stoppedTcs.SetException(e);
            }
            finally
            {
                Cleanup();
            }
        }
    }
}