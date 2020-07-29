using System.Collections;
using VrLifeAPI.Client.Applications.DefaultApps.AppManager;
using VrLifeAPI.Client.Applications.DefaultApps.ChatApp;
using VrLifeAPI.Client.Applications.DefaultApps.FriendsApp;
using VrLifeAPI.Client.Applications.DefaultApps.FriendsManagementApp;
using VrLifeAPI.Client.Applications.DefaultApps.PlaceObjectApp;
using VrLifeAPI.Client.Applications.DefaultApps.RoomListApp;
using VrLifeAPI.Client.Applications.DefaultApps.VoiceChatApp;

namespace VrLifeAPI.Client.Applications.DefaultApps
{
    /// <summary>
    /// Interface objektu poskytujícího instance výchozích aplikací
    /// </summary>
    public interface IDefaultApps : IEnumerable
    {
        /// <summary>
        /// Instance aplikace RoomList
        /// </summary>
        IRoomListApp RoomList { get; }
        /// <summary>
        /// Instance aplikace AppManager
        /// </summary>
        IAppManager AppManager { get; }
        /// <summary>
        /// Instance aplikace Friends
        /// </summary>
        IFriendsApp Friends { get; }
        /// <summary>
        /// Instance aplikace VoiceChat
        /// </summary>
        IVoiceChatApp VoiceChat { get; }
        /// <summary>
        /// Instance aplikace FriendsManagement
        /// </summary>
        IFriendsManagementApp FriendsManagement { get; }
        /// <summary>
        /// Instance aplikace Chat
        /// </summary>
        IChatApp Chat { get; }
        /// <summary>
        /// Instance aplikace PlaceObject
        /// </summary>
        IPlaceObjectApp PlaceObject { get; }
    }
}
