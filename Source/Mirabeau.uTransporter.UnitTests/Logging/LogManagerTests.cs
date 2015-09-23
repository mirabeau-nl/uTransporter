using System;

using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

using Mirabeau.uTransporter.Interfaces;
using Mirabeau.uTransporter.Logging;

using NUnit.Framework;

namespace Mirabeau.uTransporter.UnitTests.Logging
{
    [TestFixture]
    public class LogManagerTests 
    {
        private const string _LOG_MESSAGE = "Test Message";
        private readonly Exception _ex = new Exception("Test Exception");
        private MemoryAppender _appender;

        [Test]
        public void LogDebugMessage_WhenDebugIsEnabled_ReturnMessage() 
        {
            // arrange
            var logManager = GetLogManager(Level.Debug);
            
            // act
            logManager.Debug(_LOG_MESSAGE);
            
            // assert
            Assert.AreEqual(_LOG_MESSAGE, GetLogMessage());
        }

        [Test]
        public void LogDebugException_WhenDebugIsEnabled_ReturnException() 
        {
            // arrange
            var logManager = GetLogManager(Level.Debug);

            // act
            logManager.Debug(_LOG_MESSAGE, _ex);

            // assert
            Assert.AreEqual(_LOG_MESSAGE, GetLogMessage());
            Assert.AreEqual(_ex, GetLogException());
        }

        [Test]
        public void NotLogDebugMessage_WhenDebugIsDisabled_ReturnNull()
        {
            // arrange
            var logManager = GetLogManager(Level.Info);

            // act
            logManager.Debug(_LOG_MESSAGE);

            // assert
            Assert.IsNull(GetLogMessage());
        }

        [Test]
        public void LogInfoMessage_WhenInfoIsEnabled_ReturnException()
        {
            // arrange
            var logManager = GetLogManager(Level.Info);

            // act
            logManager.Info(_LOG_MESSAGE);

            // assert
            Assert.AreEqual(_LOG_MESSAGE, GetLogMessage());
        }

        [Test]
        public void LogInfoException_WhenInfoIsEnabled_ReturnMessage() 
        {
            // arrange
            var logManager = GetLogManager(Level.Info);

            // act
            logManager.Info(_LOG_MESSAGE, _ex);

            // assert
            Assert.AreEqual(_LOG_MESSAGE, GetLogMessage());
            Assert.AreEqual(_ex, GetLogException());
        }

        [Test]
        public void NotLogInfoMessage_WhenInfoIsDisabled_ReturnNull()
        {
            // arrange
            var logManager = GetLogManager(Level.Warn);

            // act
            logManager.Info(_LOG_MESSAGE);

            // assert
            Assert.IsNull(GetLogMessage());
        }

        [Test]
        public void LogWarnMessage_WhenWarnIsEnabled_ReturnMessage() 
        {
            // arrange
            var logManager = GetLogManager(Level.Warn);

            // act
            logManager.Warn(_LOG_MESSAGE);

            // assert
            Assert.AreEqual(_LOG_MESSAGE, GetLogMessage());
        }

        [Test]
        public void LogWarnException_WhenWarnIsEnabled_ReturnException()
        {
            // arrange
            var logManager = GetLogManager(Level.Warn);

            // act
            logManager.Warn(_LOG_MESSAGE, _ex);

            // assert
            Assert.AreEqual(_LOG_MESSAGE, GetLogMessage());
            Assert.AreEqual(_ex, GetLogException());
        }

        [Test]
        public void NotLogWarnMessage_WhenWarnIsDisabled_ReturnNull() 
        {
            // arrange
            var logManager = GetLogManager(Level.Error);

            // act
            logManager.Warn(_LOG_MESSAGE);

            // assert
            Assert.IsNull(GetLogMessage());
        }

        [Test]
        public void LogErrorMessage_WhenErrorIsEnabled_ReturnMessage() 
        {
            // arrange
            var logManager = GetLogManager(Level.Error);

            // act
            logManager.Error(_LOG_MESSAGE);

            // assert
            Assert.AreEqual(_LOG_MESSAGE, GetLogMessage());
        }

        [Test]
        public void LogErrorException_WhenErrorIsEnabled_ReturnException() 
        {
            // arrange
            var logManager = GetLogManager(Level.Error);

            // act
            logManager.Error(_LOG_MESSAGE, _ex);

            // assert
            Assert.AreEqual(_LOG_MESSAGE, GetLogMessage());
            Assert.AreEqual(_ex, GetLogException());
        }

        [Test]
        public void NotLogErrorMessage_WhenErrorIsDisabled_ReturnNull()
        {
            // arrange
            var logManager = GetLogManager(Level.Fatal);

            // act
            logManager.Error(_LOG_MESSAGE);

            // assert
            Assert.IsNull(GetLogMessage());
        }

        [Test]
        public void LogFatalMessage_WhenFatalIsEnabled_ReturnMessage()
        {
            // arrange
            var logManager = GetLogManager(Level.Fatal);

            // act
            logManager.Fatal(_LOG_MESSAGE);

            // assert
            Assert.AreEqual(_LOG_MESSAGE, GetLogMessage());
        }

        [Test]
        public void LogFatalException_WhenFatalIsEnabled_ReturnException() 
        {
            // arrange
            var logManager = GetLogManager(Level.Fatal);

            // act
            logManager.Fatal(_LOG_MESSAGE, _ex);

            // assert
            Assert.AreEqual(_LOG_MESSAGE, GetLogMessage());
            Assert.AreEqual(_ex, GetLogException());
        }

        [Test]
        public void LogFatalMessage_WhenFatalIsOff_ReturnNull()
        {
            // arrange
            var logManager = GetLogManager(Level.Off);

            // act
            logManager.Fatal(_LOG_MESSAGE);

            // assert
            Assert.IsNull(GetLogMessage());
        }

        private ILog4NetWrapper GetLogManager(Level level) 
        {
            _appender = new MemoryAppender 
            {
                Name = "Unit testing Appender",
                Layout = new PatternLayout("%message"),
                Threshold = level
            };
            _appender.ActivateOptions();
            var root = ((Hierarchy)LogManager.GetRepository()).Root;
            root.AddAppender(_appender);
            root.Repository.Configured = true;

            var rootLogger = LogManager.GetLogger("root");
            
            return new Log4NetWrapper(rootLogger);
        }

        private string GetLogMessage() 
        {
            if (_appender.GetEvents().Length == 0) 
            {
                return null;
            }

            var logEvent = _appender.GetEvents()[0];
            
            return logEvent.MessageObject.ToString();
        }

        private Exception GetLogException() 
        {
            if (_appender.GetEvents().Length == 0) 
            {
                return null;
            }

            var logEvent = _appender.GetEvents()[0];
            
            return logEvent.ExceptionObject;
        }
    }
}