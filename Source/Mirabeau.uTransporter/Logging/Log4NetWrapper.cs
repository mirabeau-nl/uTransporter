using System;
using System.Collections.Concurrent;
using System.Linq;

using log4net;

using Mirabeau.uTransporter.Interfaces;

namespace Mirabeau.uTransporter.Logging 
{
    public class Log4NetWrapper : ILog4NetWrapper
    {
        #region Private Fields

        private static string indentChar = "\t";

        private static bool indent = false;

        private static int indentLevel;

        private readonly ILog _logger;
        
        private bool _isDebugEnabled;
        
        private bool _isInfoEnabled;
        
        private bool _isWarnEnabled;
        
        private bool _isErrorEnabled;
        
        private bool _isFatalEnabled;
        
        #endregion

        public static ConcurrentQueue<string> LoggingQueue = new ConcurrentQueue<string>(); 

        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetWrapper"/> class.
        /// </summary>
        /// <param name="loggerName">Name of the logger.</param>
        public Log4NetWrapper(Type loggerName) 
        {
            _logger = LogManager.GetLogger(loggerName);
            SetLoggingLevelContants();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetWrapper"/> class.
        /// </summary>
        /// <param name="loggerName">Name of the logger.</param>
        public Log4NetWrapper(string loggerName)
        {
            _logger = LogManager.GetLogger(loggerName);
            SetLoggingLevelContants();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetWrapper"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public Log4NetWrapper(ILog logger) 
        {
            _logger = logger;
            SetLoggingLevelContants();
        }

        /// <summary>
        /// Gets a value indicating whether this instance is debug enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is debug enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsDebugEnabled
        {
            get { return _logger.IsDebugEnabled; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is information enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is information enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsInfoEnabled
        {
            get { return _logger.IsInfoEnabled; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is warn enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is warn enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsWarnEnabled
        {
            get { return _logger.IsWarnEnabled; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is error enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is error enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsErrorEnabled
        {
            get { return _logger.IsErrorEnabled; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is fatal enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is fatal enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsFatalEnabled
        {
            get { return _logger.IsFatalEnabled; }
        }

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public void Debug(string message, params object[] args) 
        {
            string formatedMessage = string.Format(message, args);
            
            Log(_isDebugEnabled, _logger.Debug, formatedMessage);
        }

        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public void Info(string message, params object[] args)
        {
            string formatedMessage = string.Format(message, args);
            Log(_isInfoEnabled, _logger.Info, formatedMessage);
        }

        /// <summary>
        /// Warns the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public void Warn(string message, params object[] args)
        {
            string formatedMessage = string.Format(message, args);
            Log(_isWarnEnabled, _logger.Warn, formatedMessage);
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public void Error(string message, params object[] args) 
        {
            string formatedMessage = string.Format(message, args);
            Log(_isErrorEnabled, _logger.Error, formatedMessage);
        }

        /// <summary>
        /// Fatals the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        public void Fatal(string message, params object[] args) 
        {
            string formatedMessage = string.Format(message, args);
            Log(_isFatalEnabled, _logger.Fatal, formatedMessage);
        }

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="e">The exception object.</param>
        public void Debug(string message, Exception e = null)
        {
            Log(_isDebugEnabled, _logger.Debug, message, e);
        }

        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="e">The e.</param>
        public void Info(string message,  Exception e = null)
        {
            Log(_isInfoEnabled, _logger.Info, message, e);
        }

        /// <summary>
        /// Warns the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="e">The e.</param>
        public void Warn(string message, Exception e = null)
        {
            Log(_isWarnEnabled, _logger.Warn, message, e);
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="e">The exception object.</param>
        public void Error(string message, Exception e = null) 
        {
            Log(_isErrorEnabled, _logger.Error, message, e);
        }

        /// <summary>
        /// Fatals the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="e">The exception object.</param>
        public void Fatal(string message, Exception e = null) 
        {
            Log(_isFatalEnabled, _logger.Fatal, message, e);
        }

        /// <summary>
        /// Add some indent to a log message.
        /// </summary>
        /// <param name="level">The level.</param>
        public void Indent(int level = 1)
        {
            indent = true;
            indentLevel = level;
        }

        #endregion

        #region Private Methods

        private static void Log(bool enabled, Action<string, Exception> logAction, string message, Exception exception = null)
        {
            if (!enabled)
            {
                return;
            }

            if (indent)
            {
                message = string.Concat(Enumerable.Repeat(indentChar, indentLevel)) + message;
                indent = false;
            } 
 
            logAction(message, exception);

        }

        private void SetLoggingLevelContants() 
        {
            _isDebugEnabled = true;
            _isInfoEnabled = true;
            _isWarnEnabled = true;
            _isErrorEnabled = true;
            _isFatalEnabled = true;
        }

        #endregion
    }
}
