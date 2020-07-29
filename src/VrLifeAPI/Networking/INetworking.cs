using Google.Protobuf;
using System;
using System.Net;

namespace VrLifeAPI.Networking
{
    /// <summary>
    /// Interface síťovacího serveru.
    /// </summary>
    /// <typeparam name="T">Typ přijímaných a odesílaných dat.</typeparam>
    public interface INetworking<T>
    {
        /// <summary>
        /// Odeslání zprávy asynchronně se specifikovanými callbacky.
        /// </summary>
        /// <param name="req">Žádost k odeslání.</param>
        /// <param name="address">Adresa příjemce.</param>
        /// <param name="callback">Callback při úspěšném odeslání a přijetí odpovědi.</param>
        /// <param name="err">Callback v případě chyby.</param>
        void SendAsync(T req, IPEndPoint address, Action<T> callback, Action<Exception> err = null);

        /// <summary>
        /// Odeslání zprávy synchronně bez zachytávání exception a výsledkem jako návratovou hodnotou.
        /// </summary>
        /// <param name="req">Žádost k odeslání.</param>
        /// <param name="address">Adresa příjemce.</param>
        /// <returns>Odpověď od příjemce.</returns>
        T Send(T req, IPEndPoint address);

        /// <summary>
        /// Začít poslouchat jako server.
        /// </summary>
        void StartListening();
    }
}
