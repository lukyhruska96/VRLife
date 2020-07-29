using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using VrLifeAPI.Client.API;
using VrLifeAPI.Client.API.ObjectAPI;
using VrLifeClient.API;

namespace VrLifeAPI.Client.Applications.ObjectApp
{
    /// <summary>
    /// Interface objektové aplikace.
    /// </summary>
    public interface IObjectApp : IApplication
    {
        /// <summary>
        /// Inicializace aplikace.
        /// </summary>
        /// <param name="api">Instance OpenAPI.</param>
        /// <param name="objectAPI">Instance ObjectAPI.</param>
        void Init(IOpenAPI api, IObjectAPI objectAPI);

        /// <summary>
        /// Getter ObjectAppInfo objektu popisujícího danou objektovou aplikaci.
        /// </summary>
        /// <returns>Instance ObjectAppInfo.</returns>
        ObjectAppInfo GetObjectAppInfo();

        /// <summary>
        /// Vytvoření nové instance objektové aplikace v prostoru.
        /// </summary>
        /// <param name="appInstance">Nastavený identifikátor aplikační instance k uložení.</param>
        /// <param name="center">Střed aplikace k vykreslení s vymezeným vyžádaným prostorem v ObjectAppInfo.</param>
        /// <returns></returns>
        IObjectAppInstance CreateInstance(ulong appInstance, Vector3 center);
    }
}
