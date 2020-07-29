
namespace VrLifeAPI.Provider.API.APIs
{
    /// <summary>
    /// Interface uživatelského API na straně Providera
    /// </summary>
    public interface IUserAPI
    {
        /// <summary>
        /// Převod klientského ID na ID uživatele.
        /// 
        /// null v případě, že žádný uživatel není přihlášený na daném klientovi.
        /// </summary>
        /// <param name="clientId">ID klienta.</param>
        /// <returns>ID uživatele.</returns>
        ulong? ClientId2UserId(ulong clientId);
    }
}
