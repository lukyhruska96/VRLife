using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Common.Core.Services
{
    /// <summary>
    /// Interface služby na straně serveru
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Zpracování přijaté zprávy
        /// </summary>
        /// <param name="msg">Přijatá hlavní zpráva.</param>
        /// <returns>Odpověď jako hlavní zpráva.</returns>
        MainMessage HandleMessage(MainMessage msg);
    }
}
