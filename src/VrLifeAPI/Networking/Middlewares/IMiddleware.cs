using System;
using System.Collections.Generic;
using System.Text;

namespace VrLifeAPI.Networking.Middlewares
{
    /// <summary>
    /// Interface middlewaru.
    /// </summary>
    /// <typeparam name="T">Typ transformované hodnoty.</typeparam>
    public interface IMiddleware<T>
    {
        /// <summary>
        /// Transformace přijatého objektu.
        /// </summary>
        /// <param name="msg">Přijatý objekt.</param>
        /// <returns>Transformovaný objekt.</returns>
        T TransformInputMsg(T msg);

        /// <summary>
        /// Transformace objektu před odesláním.
        /// </summary>
        /// <param name="msg">Objekt k odeslání.</param>
        /// <returns>Transformovaný objekt.</returns>
        T TransformOutputMsg(T msg);
    }
}
