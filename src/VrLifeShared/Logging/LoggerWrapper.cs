﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VrLifeShared.Logging
{
    public class LoggerWrapper : ILogger
    {
        private ILogger logger;
        private string wrapMsg = "";

        public LoggerWrapper(string className, ILogger logger)
        {
            this.wrapMsg = className;
            this.logger = logger;
        }

        public void Debug(string msg)
        {
            logger.Debug($"{wrapMsg}: {msg}");
        }

        public void Debug(Exception ex)
        {
            logger.Debug($"{wrapMsg}: {ex.Message}");
        }

        public void Dispose()
        {
            logger.Dispose();
        }

        public void Error(string msg)
        {
            logger.Error($"{wrapMsg}: {msg}");
        }

        public void Error(Exception ex)
        {
            logger.Error($"{wrapMsg}: {ex.GetType().Name}: { ex.Message}");
        }

        public void Info(string msg)
        {
            logger.Info($"{wrapMsg}: {msg}");
        }

        public void SetDebug(bool status)
        {
            logger.SetDebug(status);
        }

        public void Warn(string msg)
        {
            logger.Warn($"{wrapMsg}: {msg}");
        }
    }

    public static class LoggerExtension
    {
        public static ILogger Wrap(this ILogger logger, string className)
        {
            return new LoggerWrapper(className, logger);
        }
    }
}
