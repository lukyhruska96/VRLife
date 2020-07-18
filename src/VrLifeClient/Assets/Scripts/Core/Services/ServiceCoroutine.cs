using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    class ServiceCoroutine<T> : CustomYieldInstruction
    {
        private Task<T> _t;
        public override bool keepWaiting
        {
            get => _t.IsCompleted;
        }

        public ServiceCoroutine(Task<T> t)
        {
            _t = t;
        }
    }
}
