using System;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Abstractions.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace RedHat.AspNetCore.Server.Kestrel.Transport.Linux
{
    public class LinuxTransportFactory : ITransportFactory
    {
        private readonly LinuxTransportOptions _options;
        private readonly ILoggerFactory _loggerFactory;

        public LinuxTransportFactory(IOptions<LinuxTransportOptions> options, ILoggerFactory loggerFactory)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options.Value;
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public ITransport Create(IEndPointInformation endPointInformation, IConnectionDispatcher handler)
        {
            return new Transport(endPointInformation, handler, _options, _loggerFactory);
        }
    }
}