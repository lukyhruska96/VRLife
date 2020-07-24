using VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels;

namespace VrLifeAPI.Common.Core.Applications.DefaultApps.FriendsApp
{
    public interface IFriendsAppUser
    {
        ulong UserId { get; }
        string Username { get; }
        uint? CurrentRoomId { get; }
        ulong[] FriendsList { get; }

        FriendsAppUserMsg ToNetworkModel();
    }
}
