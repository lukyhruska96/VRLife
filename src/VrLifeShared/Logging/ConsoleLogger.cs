using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeShared.Logging
{
    public class ConsoleLogger : ILogger
    {
        private bool _debug;
        public void Debug(string msg)
        {
            if (!_debug)
            {
                return;
            }
            Write("DEBUG", msg);
        }

        public void Debug(Exception ex)
        {
            if (!_debug)
            {
                return;
            }
            Write("DEBUG", ex.Message);
        }

        public void Dispose()
        {
            Console.Out.Flush();
        }

        public void Error(string msg)
        {
            Write("ERROR", msg);
        }

        public void Error(Exception ex)
        {
            Write("ERROR", ex.Message);
        }

        public void Info(string msg)
        {
            Write("INFO", msg);
        }

        public void SetDebug(bool status)
        {
            _debug = status;
        }

        public void Warn(string msg)
        {
            Write("WARN", msg);
        }

        private void Write(string type, string msg)
        {
            Console.WriteLine($"[{DateTime.Now}] {type}: {msg}");
        }
    }
}
