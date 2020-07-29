using System.Threading.Tasks;
using UnityEngine;

namespace VrLifeAPI.Client.Core.Services
{
    
    /// <summary>
    /// Vlastní yield instrukce pro Unity Coroutine
    /// </summary>
    /// <typeparam name="T">Návratový typ po vykonání dotazu.</typeparam>
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
