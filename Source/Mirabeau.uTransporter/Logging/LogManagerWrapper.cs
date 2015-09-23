using System;

using Mirabeau.uTransporter.Interfaces;

namespace Mirabeau.uTransporter.Logging 
{
    public static class LogManagerWrapper 
    {
        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="loggerName">Name of the logger.</param>
        /// <returns>Log4NetWrapper object</returns>
        public static ILog4NetWrapper GetLogger(Type loggerName) 
        {
            return new Log4NetWrapper(loggerName);
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <param name="loggerNameString">The logger name string.</param>
        /// <returns>Log4NetWrapper object</returns>
        public static ILog4NetWrapper GetLogger(string loggerNameString)
        {
            return new Log4NetWrapper(loggerNameString);
        }
    }
}
