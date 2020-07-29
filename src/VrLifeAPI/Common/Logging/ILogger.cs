using System;
namespace VrLifeAPI.Common.Logging.Logging
{
    /// <summary>
    /// Interface pro logger
    /// </summary>
    public interface ILogger : IDisposable
    {
        /// <summary>
        /// Zaloguje informační zprávu.
        /// </summary>
        /// <param name="msg">Zpráva.</param>
        void Info(String msg);

        /// <summary>
        /// Zaloguje chybovou zprávu.
        /// </summary>
        /// <param name="msg">Zpráva.</param>
        void Error(String msg);

        /// <summary>
        /// Zaloguje výjimku jako chybovou zprávu.
        /// </summary>
        /// <param name="ex">Výjimka.</param>
        void Error(Exception ex);

        /// <summary>
        /// Zaloguje varovnou zprávu.
        /// </summary>
        /// <param name="msg">Zpráva.</param>
        void Warn(String msg);

        /// <summary>
        /// Zaloguje zprávu v případě debug režimu.
        /// </summary>
        /// <param name="msg">Zpráva.</param>
        void Debug(String msg);

        /// <summary>
        /// Zaloguje výjimku v případě debug režimu.
        /// </summary>
        /// <param name="ex">Zpráva.</param>
        void Debug(Exception ex);

        /// <summary>
        /// Nastavení debug režimu.
        /// </summary>
        /// <param name="status">Stav debug režimu.</param>
        void SetDebug(bool status);
    }
}
