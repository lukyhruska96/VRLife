using System;
using System.IO;
using System.Text;

namespace VrLifeServer.Logging
{
    public class FileLogger : ILogger
    {
        private FileStream fs;
        private Encoding encoding = new UTF8Encoding(true);
        private bool _debug = false;
        public FileLogger(String filePath)
        {
            if (File.Exists(filePath))
            {
                fs = new FileStream(filePath, FileMode.Append);
            }
            else
            {
                fs = new FileStream(filePath, FileMode.Create);
            }
            if (!fs.CanWrite)
            {
                throw new UnauthorizedAccessException("Not enough permission to write into log file");
            }
        }

        public void Debug(string msg)
        {
            if (!_debug)
            {
                return;
            }
            Write(msg, "DEBUG");
        }

        public void Debug(Exception ex)
        {
            if(!_debug)
            {
                return;
            }
            Write(ex.Message, "DEBUG");
            byte[] bytes = encoding.GetBytes(ex.StackTrace + Environment.NewLine);
            fs.Write(bytes, 0, bytes.Length);
        }

        public void Dispose()
        {
            fs.Dispose();
        }

        public void Error(string msg)
        {
            Write(msg, "ERROR");
        }

        public void Error(Exception ex)
        {
            Write(ex.Message, "ERROR");
        }

        public void Info(string msg)
        {
            Write(msg, "INFO");
        }

        public void SetDebug(bool status)
        {
            this._debug = status;
        }

        public void Warn(string msg)
        {
            Write(msg, "WARN");
        }

        private void Write(string msg, string type)
        {
            string line = $"[{DateTime.Now}]\t{type}\t{msg}{Environment.NewLine}";
            byte[] bytes = encoding.GetBytes(line);
            fs.Write(bytes, 0, bytes.Length);
        }
    }
}
