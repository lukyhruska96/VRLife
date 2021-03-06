﻿using System;
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

        private Task<T> t = null;
        public T Result { get => t.Result; }

        public Exception Exception { get; private set; } = null;
        public bool HasException { get => Exception != null; }
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
            t = new Task<T>(Action);
            t.RunSynchronously();
            if (HasException)
            {
                throw Exception;
            }
            return t.Result;
        }

        public ServiceCoroutine<T> WaitCoroutine()
        {
            t = Task<T>.Run(Action);
            return new ServiceCoroutine<T>(t);
        }

        public ServiceCallback<T> Exec()
        {
            t = Task<T>.Run(Action);
            return this;
        }

        private T Action()
        {
            T val;
            try
            {
                val = _func.Invoke();
            }
            catch (Exception ex)
            {
                Exception = ex;
                _err?.Invoke(ex);
                return default;
            }
            try
            {
                _succ?.Invoke(val);
            }
            catch(Exception) { }
            return val;
        }

    }
}
