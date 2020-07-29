using System.Collections.Generic;
using System.Linq;
using VrLifeAPI.Client.Applications.BackgroundApp;
using VrLifeAPI.Client.Core.Services;
using VrLifeAPI.Common.Core.Applications.DefaultApps.FriendsApp;

namespace VrLifeAPI.Client.Applications.DefaultApps.FriendsApp
{
    /// <summary>
    /// Interface pro výchozí aplikaci FriendsApp
    /// </summary>
    public interface IFriendsApp : IBackgroundApp
    {
        /// <summary>
        /// Dotaz pro odeslání žádosti o přátelství
        /// </summary>
        /// <param name="userId">ID příjemce žádosti</param>
        /// <returns>ServiceCallback bez hodnoty, exception v případě problémů.</returns>
        IServiceCallback<bool> SendFriendRequest(ulong userId);

        /// <summary>
        /// Přijetí žádosti o přátelství.
        /// </summary>
        /// <param name="userId">ID odesílatele žádosti</param>
        /// <returns>ServiceCallback bez hodnoty, exception v případě problémů.</returns>
        IServiceCallback<bool> AcceptFriendRequest(ulong userId);

        /// <summary>
        /// Smazání uživatele ze seznamu přátel.
        /// </summary>
        /// <param name="userId">ID uživatele ke smazání.</param>
        /// <returns>ServiceCallback bez hodnoty, exception v případě problémů.</returns>
        IServiceCallback<bool> RemoveFriend(ulong userId);

        /// <summary>
        /// Zrušení žádosti o přítelství.
        /// </summary>
        /// <param name="userId">ID příjemce žádosti.</param>
        /// <returns>ServiceCallback bez hodnoty, exception v případě problémů.</returns>
        IServiceCallback<bool> UndoFriendRequest(ulong userId);

        /// <summary>
        /// Smazání žádosti o přátelství.
        /// </summary>
        /// <param name="userId">ID odesílatele žádosti.</param>
        /// <returns>ServiceCallback bez hodnoty, exception v případě problémů.</returns>
        IServiceCallback<bool> DeleteFriendRequest(ulong userId);

        /// <summary>
        /// Získání detailů o uživatele ze seznamu přátel.
        /// </summary>
        /// <param name="userId">ID uživatele.</param>
        /// <returns>ServiceCallback s návratovou hodnotou detailu uživatele.</returns>
        IServiceCallback<IFriendsAppUser> GetFriendDetails(ulong userId);

        /// <summary>
        /// Dotaz na seznam všech přítel.
        /// </summary>
        /// <returns>ServiceCallback s návratovou hodnotou seznamu všech přátel.</returns>
        IServiceCallback<List<IFriendsAppUser>> ListFriends();

        /// <summary>
        /// Dotaz na seznam žádostí o přátelství.
        /// </summary>
        /// <returns>ServiceCallback s návratovou hodnotou seznamu všech žádostí o přítelství.</returns>
        IServiceCallback<List<IFriendsAppUser>> GetFriendRequests();
    }
}
