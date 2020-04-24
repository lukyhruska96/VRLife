using System;
namespace VrLifeServer.Logging
{
    public interface ILogger : IDisposable
    {
        void Info(String msg);
        void Error(String msg);
        void Error(Exception ex);
        void Debug(String msg);
        void Debug(Exception ex);
        
        ILogger Wrap(String className)
        {
            return new LoggerWrapper(className, this);
        }
    }
}
