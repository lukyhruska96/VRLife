using System;
namespace VrLifeAPI.Common.Logging.Logging
{
    public interface ILogger : IDisposable
    {
        void Info(String msg);
        void Error(String msg);
        void Error(Exception ex);
        void Warn(String msg);
        void Debug(String msg);
        void Debug(Exception ex);
        void SetDebug(bool status);
    }
}
