using VrLifeAPI.Client.API;
using VrLifeAPI.Client.API.GlobalAPI;

namespace VrLifeAPI.Client.Applications.GlobalApp
{
    /// <summary>
    /// Interface globální aplikace
    /// </summary>
    public interface IGlobalAPp : IApplication
    {
        /// <summary>
        /// Inicializace globální aplikace
        /// </summary>
        /// <param name="api">Instance OpenAPI</param>
        /// <param name="globalAPI">Instance GlobalAPI</param>
        void Init(IOpenAPI api, IGlobalAPI globalAPI);
    }
}
