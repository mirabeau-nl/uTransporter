using log4net;

namespace Mirabeau.uTransporter.Extensions
{
    public static class Logger
    {
        private static ILog _logger;

        public static void WriteDebugLine<T>(string message, params object[] args)
        {
            _logger = LogManager.GetLogger(typeof(T));

            _logger.Debug(string.Format(message, args));
        }

        public static void WriteInfoLine<T>(string message, params object[] args)
        {
            _logger = LogManager.GetLogger(typeof(T));
            _logger.Info(string.Format(message, args));
        }

        public static void WriteErrorLine<T>(string message, params object[] args)
        {
            _logger = LogManager.GetLogger(typeof(T));
            _logger.Error(string.Format(message, args));
        }
    }
}
