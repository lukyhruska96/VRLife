using VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels;

namespace VrLifeAPI.Common.Core.Applications.DefaultApps.FriendsApp
{
    /// <summary>
    /// Informace o uživateli ze seznamu přátel.
    /// </summary>
    public interface IFriendsAppUser
    {
        /// <summary>
        /// ID uživatele
        /// </summary>
        ulong UserId { get; }

        /// <summary>
        /// Uživatelské jméno
        /// </summary>
        string Username { get; }

        /// <summary>
        /// ID místnosti v které je aktuálně připojen.
        /// Null v případě, že uživatel není online.
        /// </summary>
        uint? CurrentRoomId { get; }

        /// <summary>
        /// Seznam přátel daného uživatele.
        /// </summary>
        ulong[] FriendsList { get; }

        /// <summary>
        /// Převod objektu na síťovací objekt k odeslání.
        /// </summary>
        /// <returns>Instance síťovacího objektu.</returns>
        FriendsAppUserMsg ToNetworkModel();
    }
}
