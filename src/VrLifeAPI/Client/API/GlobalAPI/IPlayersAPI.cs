using System.Collections.ObjectModel;
using VrLifeAPI.Client.Core.Wrappers;

namespace VrLifeAPI.Client.API.GlobalAPI
{
    /// <summary>
    /// API pro přístup k objektům jednotlivých uživatelů v místnosti
    /// včetně aktuálně přihlášeného uživatele na lokálním klientovi
    /// </summary>
    public interface IPlayersAPI
    {
        /// <summary>
        /// Getter k získání seznamu objektů všech uživatelů v místnosti 
        /// kromě lokálního uživatele.
        /// </summary>
        /// <returns>Seznam objektů všech uživatelů</returns>
        ReadOnlyDictionary<ulong, IAvatar> GetAvatars();
        
        /// <summary>
        /// Getter k získání objektu lokálního uživatele.
        /// </summary>
        /// <returns>Objekt lokálního uživatele</returns>
        IAvatar GetMainAvatar();

        /// <summary>
        /// Inicializace API
        /// </summary>
        /// <param name="mainAvatar">Objekt lokálního uživatele k uložení.</param>
        void Init(IAvatar mainAvatar);

        /// <summary>
        /// Uloží objekt nově vytvořeného uživatele v místnosti.
        /// </summary>
        /// <param name="userId">Identifikační číslo uživatele.</param>
        /// <param name="avatar">Objekt uživatele.</param>
        void AddAvatar(ulong userId, IAvatar avatar);

        /// <summary>
        /// Přepis objektu lokálního uživatele (pro případ možnosti změny
        /// postavy do budoucna)
        /// </summary>
        /// <param name="avatar">Objekt lokálního uživatele.</param>
        void AddMainAvatar(IAvatar avatar);

        /// <summary>
        /// Getter k získání konkrétního uživatele podle jeho indentifikačního čísla.
        /// </summary>
        /// <param name="userId">Identifikační číslo uživatele.</param>
        /// <returns>Objekt uživatele, null v případě neexistujícího uživatele v aktuální místnosti.</returns>
        IAvatar GetAvatar(ulong userId);

        /// <summary>
        /// Smazání uživatele podle jeho identifikačního čisla.
        /// </summary>
        /// <param name="userId">Identifikační číslo uživatele.</param>
        void DeleteAvatar(ulong userId);
    }
}
