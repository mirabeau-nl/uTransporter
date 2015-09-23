using System;

namespace Mirabeau.uTransporter.Interfaces 
{
    public interface ILog4NetWrapper 
    {
        bool IsDebugEnabled { get; }

        void Debug(string message, params object[] args);

        void Info(string message, params object[] args);

        void Warn(string message, params object[] args);

        void Error(string message, params object[] args);

        void Fatal(string message, params object[] args);

        void Debug(string message, Exception e);

        void Info(string message, Exception e);

        void Warn(string message, Exception e);

        void Error(string message, Exception e);

        void Fatal(string message, Exception e);

        void Indent(int level = 1);
    }
}
