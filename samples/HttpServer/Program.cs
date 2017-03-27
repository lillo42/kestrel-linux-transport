﻿using System;
using System.Net;
using System.Threading.Tasks;
using Tmds.Kestrel.Linux;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        public static async Task MainAsync(string[] args)
        {
            IPAddress address = IPAddress.Loopback;
            int port = 0;
            if (args.Length > 0)
            {
                var split = args[0].Split(new[] {':'});
                address = IPAddress.Parse(split[0]);
                port = int.Parse(split[1]);
            }

            var endpoint = new IPEndPoint(address, port);
            var transport = new Transport(new[] { endpoint }, new HttpServer(), new TransportOptions() {
                ThreadCount = Environment.ProcessorCount

            } );
            await transport.BindAsync();
            Console.WriteLine($"Listening on {endpoint}.");
            Console.WriteLine("Press any key to stop the server.");
            Console.Read();
            await transport.StopAsync();
        }
    }
}