using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Assets.Scripts.Core.Services
{
    class ServiceCallback
    {
        private Action _action;
        private UnityEvent _succ = null;
        private UnityEvent<Exception> _err = null;
        public ServiceCallback(Action action)
        {
            this._action = action;
        }

        public ServiceCallback SetSucc(UnityEvent succ)
        {
            this._succ = succ;
            return this;
        }

        public ServiceCallback SetErr(UnityEvent<Exception> err)
        {
            this._err = err;
            return this;
        }

        public ServiceCallback Exec()
        {
            Task.Run(() =>
            {
                try
                {
                    _action.Invoke();
                }
                catch (Exception ex)
                {
                    _err?.Invoke(ex);
                    return;
                }
                _succ?.Invoke();
            });
            return this;
        }
    }
}
