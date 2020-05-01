using System;
using System.Collections.Generic;

namespace VrLifeServer.Logging
{
    public class LoggersContainer : ILogger
    {
        private List<ILogger> loggers = new List<ILogger>();

        public void Add(ILogger logger)
        {
            this.loggers.Add(logger);
        }

        public void Debug(string msg)
        {
            foreach (ILogger logger in loggers)
            {
                logger.Debug(msg);
            }
        }

        public void Debug(Exception ex)
        {
            foreach (ILogger logger in loggers)
            {
                logger.Debug(ex);
            }
        }

        public void Dispose()
        {
            foreach (ILogger logger in loggers)
            {
                logger.Dispose();
            }
        }

        public void Error(string msg)
        {
            foreach (ILogger logger in loggers)
            {
                logger.Error(msg);
            }
        }

        public void Error(Exception ex)
        {
            foreach (ILogger logger in loggers)
            {
                logger.Error(ex);
            }
        }

        public void Info(string msg)
        {
            foreach (ILogger logger in loggers)
            {
                logger.Info(msg);
            }
        }

        public void SetDebug(bool status)
        {
            foreach (ILogger logger in loggers)
            {
                logger.SetDebug(status);
            }
        }

        public void Warn(string msg)
        {
            foreach (ILogger logger in loggers)
            {
                logger.Warn(msg);
            }
        }
    }
}
