
using VrLifeAPI.Common.Core.Services.UserService;

namespace VrLifeAPI.Forwarder.Core.Services.UserService
{

    /// <summary>
    /// Interface uživatelské služby na straně Forwardera
    /// </summary>
    public interface IUserServiceForwarder : IUserService, IServiceForwarder
    {
        /// <summary>
        /// Získání uživatele podle ID.
        /// 
        /// Pokud je zvolena metoda cached,
        /// budou použita poslední načtená data.
        /// V případě neexistence objektu, budou
        /// načtena jako v případě, kdy cached je false.
        /// </summary>
        /// <param name="userId">ID uživatele.</param>
        /// <param name="cached">Použij stará data z cache.</param>
        /// <returns>Objekt uživatele.</returns>
        IUser GetUserById(ulong userId, bool cached = false);

        /// <summary>
        /// Získání ID uživatele podle ID klienta.
        /// 
        /// null v případě, že žádný uživatel na klientovi není příhlášený.
        /// </summary>
        /// <param name="clientId">ID klienta.</param>
        /// <param name="cached">Použij stará data z cache.</param>
        /// <returns>ID uživatele.</returns>
        ulong? GetUserIdByClientId(ulong clientId, bool cached = false);

        /// <summary>
        /// Kontrola shody userId a clientID.
        /// 
        /// Pokud se shodují, je okamžitě vráceno true,
        /// v opačném případě je poslán dotaz na Providera,
        /// kdy jsou přijatá data uložena do cache a poté je
        /// hodnota znovu vyhodnocena.
        /// </summary>
        /// <param name="clientId">ID klienta.</param>
        /// <param name="userId">ID uživatele.</param>
        /// <returns>true - shoda, false - neshoda.</returns>
        bool FastCheckUserId(ulong clientId, ulong userId);
    }
}
