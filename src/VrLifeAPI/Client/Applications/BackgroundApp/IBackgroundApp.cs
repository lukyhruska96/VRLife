using VrLifeAPI.Client.API;

namespace VrLifeAPI.Client.Applications.BackgroundApp
{
    /// <summary>
    /// Interface pro aplikaci v pozadí.
    /// </summary>
    public interface IBackgroundApp : IApplication
    {
        /// <summary>
        /// Inicializace aplikace.
        /// </summary>
        /// <param name="api">Instance OpenAPI.</param>
        void Init(IOpenAPI api);
    }
}
