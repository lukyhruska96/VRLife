using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace VrLifeAPI.Client.Core.Services
{
    /// <summary>
    /// UnityEvent volaný v případě volání pomocí Exec
    /// při výskytu chyby.
    /// </summary>
    public class ErrorUnityEvent : UnityEvent<Exception> { }

    /// <summary>
    /// Interface objektu ServiceCallback
    /// </summary>
    /// <typeparam name="T">Typ návratové hodnoty v případě úspěchu dotazu.</typeparam>
    public interface IServiceCallback<T>
    {
        /// <summary>
        /// Návratová hodnota
        /// </summary>
        T Result { get; }

        /// <summary>
        /// Exception v případě chyby, jinak null
        /// </summary>
        Exception Exception { get; }

        /// <summary>
        /// Informace, zda nastala chyba
        /// </summary>
        bool HasException { get; }

        /// <summary>
        /// Nastavení události zavolané v případě úspěchu.
        /// </summary>
        /// <param name="succ">Event k zavolání.</param>
        /// <returns>Vlastní instanci k řetězení nastavení.</returns>
        IServiceCallback<T> SetSucc(UnityEvent<T> succ);

        /// <summary>
        /// Nastavení události zavolané v případě chyby.
        /// </summary>
        /// <param name="err">Event k zavolání.</param>
        /// <returns>Vlastní instanci k řetězení nastavení.</returns>
        IServiceCallback<T> SetErr(ErrorUnityEvent err);

        /// <summary>
        /// Synchornní volání metody.
        /// V tomto případě je v případě chyby výjimka zpropagována.
        /// 
        /// Nastavené události v případě úspěchu, nebo chyby jsou ignorovány.
        /// </summary>
        /// <returns>Návratová hodnota v případě úspěchu.</returns>
        T Wait();

        /// <summary>
        /// Volání v Tasku uvnitř struktury, při které 
        /// Unity Coroutine čeká na ukončení vlákna
        /// bez blokování hlavního vlákna.
        /// 
        /// Podobné WaitUntil.
        /// 
        /// Návratová hodnota je k nalezení v property Result.
        /// Případná exception v případě neúsěchu v property Exception.
        /// </summary>
        /// <returns>Vlastní Coroutine k čekání na dokončení vlákna.</returns>
        ServiceCoroutine<T> WaitCoroutine();

        /// <summary>
        /// Spuštění události asynchronně.
        /// Vyvolává poté předem nastavené událostí
        /// specifikované pomocí SetSucc a SetErr.
        /// </summary>
        /// <returns>Vlastní instanci k řetězení nastavení.</returns>
        IServiceCallback<T> Exec();

    }
}
