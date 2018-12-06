using System;
using System.Runtime.InteropServices;

namespace RedHat.AspNetCore.Server.Kestrel.Transport.Linux
{
    internal struct PipeEndPair
    {
        public PipeEnd ReadEnd;
        public PipeEnd WriteEnd;

        public void Dispose()
        {
            ReadEnd?.Dispose();
            WriteEnd?.Dispose();
        }
    }

    internal static class PipeInterop
    {
        [DllImport(Interop.Library, EntryPoint = "RHXKL_Pipe")]
        public static extern PosixResult Pipe(out PipeEnd readEnd, out PipeEnd writeEnd, bool blocking);
    }

    internal class PipeEnd : CloseSafeHandle
    {
        private PipeEnd()
        {
        }

        public void WriteByte(byte b)
        {
            TryWriteByte(b)
                .ThrowOnError();
        }

        public unsafe PosixResult TryWriteByte(byte b)
        {
            return base.TryWrite(&b, 1);
        }

        public unsafe byte ReadByte()
        {
            byte b = 0;
            var result = base.TryRead(&b, 1);
            result.ThrowOnError();
            return b;
        }

        public unsafe PosixResult TryReadByte()
        {
            byte b;
            var result = base.TryRead(&b, 1);

            if (result.IsSuccess)
            {
                return new PosixResult(b);
            }

            return result;
        }

        public int Write(ArraySegment<byte> buffer)
        {
            var result = TryWrite(buffer);
            result.ThrowOnError();
            return result.Value;
        }

        public new PosixResult TryWrite(ArraySegment<byte> buffer)
        {
            return base.TryWrite(buffer);
        }

        public int Read(ArraySegment<byte> buffer)
        {
            var result = TryRead(buffer);
            result.ThrowOnError();
            return result.Value;
        }

        public new PosixResult TryRead(ArraySegment<byte> buffer)
        {
            return base.TryRead(buffer);
        }

        public static PipeEndPair CreatePair(bool blocking)
        {
            var result = PipeInterop.Pipe(out PipeEnd readEnd, out PipeEnd writeEnd, blocking);
            result.ThrowOnError();
            return new PipeEndPair {ReadEnd = readEnd, WriteEnd = writeEnd};
        }
    }
}