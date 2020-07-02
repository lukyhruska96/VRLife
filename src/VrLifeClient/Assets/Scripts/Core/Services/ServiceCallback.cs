using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Assets.Scripts.Core.Services
{
    public class ErrorUnityEvent : UnityEvent<Exception> { }

    class ServiceCallback<T>
    {
        private Func<T> _func;
        private UnityEvent<T> _succ = null;
        private ErrorUnityEvent _err = null;
        public ServiceCallback(Func<T> func)
        {
            this._func = func;
        }

        public ServiceCallback<T> SetSucc(UnityEvent<T> succ)
        {
            this._succ = succ;
            return this;
        }

        public ServiceCallback<T> SetErr(ErrorUnityEvent err)
        {
            this._err = err;
            return this;
        }

        public T Wait()
        {
            return _func.Invoke();
        }

        public ServiceCallback<T> Exec()
        {
            Task.Run(() =>
            {
                T val;
                try
                {
                    val = _func.Invoke();
                }
                catch (Exception ex)
                {
                    _err?.Invoke(ex);
                    return;
                }
                _succ?.Invoke(val);
            });
            return this;
        }

    }
}
