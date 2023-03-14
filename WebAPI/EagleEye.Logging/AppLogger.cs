using EagleEye.Application.Contracts.Logger;
using Microsoft.Extensions.Logging;

namespace EagleEye.Logging
{
    internal class AppLogger<T> : IAppLogger<T>
    {
        readonly ILogger<T> _logger;

        public AppLogger(ILoggerFactory loggerFactory)
            => _logger = loggerFactory.CreateLogger<T>();

        public void LogInformation(string message, params object[] args)
            => _logger.LogInformation(message, args);

        public void LogWarning(string message, params object[] args)
            => _logger.LogWarning(message, args);
    }
}
