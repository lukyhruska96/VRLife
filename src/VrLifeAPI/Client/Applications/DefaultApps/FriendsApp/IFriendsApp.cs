using System.Collections.Generic;
using System.Linq;
using VrLifeAPI.Client.Applications.BackgroundApp;
using VrLifeAPI.Client.Core.Services;
using VrLifeAPI.Common.Core.Applications.DefaultApps.FriendsApp;

namespace VrLifeAPI.Client.Applications.DefaultApps.FriendsApp
{
    public interface IFriendsApp : IBackgroundApp
    {

        IServiceCallback<bool> SendFriendRequest(ulong userId);

        IServiceCallback<bool> AcceptFriendRequest(ulong userId);

        IServiceCallback<bool> RemoveFriend(ulong userId);

        IServiceCallback<bool> UndoFriendRequest(ulong userId);

        IServiceCallback<bool> DeleteFriendRequest(ulong userId);

        IServiceCallback<IFriendsAppUser> GetFriendDetails(ulong userId);

        IServiceCallback<List<IFriendsAppUser>> ListFriends();

        IServiceCallback<List<IFriendsAppUser>> GetFriendRequests();
    }
}
