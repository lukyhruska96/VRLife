using System.Threading.Tasks;
using UnityEngine;

namespace VrLifeAPI.Client.Core.Services
{
    public class ServiceCoroutine<T> : CustomYieldInstruction
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
