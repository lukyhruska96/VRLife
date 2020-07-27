using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace VrLifeAPI.Client.Core.Services
{
    public class ErrorUnityEvent : UnityEvent<Exception> { }

    public interface IServiceCallback<T>
    {
        T Result { get; }

        Exception Exception { get; }
        bool HasException { get; }

        IServiceCallback<T> SetSucc(UnityEvent<T> succ);
         IServiceCallback<T> SetErr(ErrorUnityEvent err);

        T Wait();

        ServiceCoroutine<T> WaitCoroutine();

        IServiceCallback<T> Exec();

    }
}
