using System.Linq;
using VrLifeAPI.Client.Applications.DefaultApps.FriendsApp;
using VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels;
using VrLifeAPI.Common.Core.Applications.DefaultApps.FriendsApp;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeShared.Core.Applications.DefaultApps.FriendsApp
{
    public class FriendsAppUser : IFriendsAppUser
    {
        public ulong UserId { get; private set; }
        public string Username { get; private set; }
        public uint? CurrentRoomId { get; private set; }
        public ulong[] FriendsList { get; private set; }

        public FriendsAppUser(UserDetailMsg userMsg, uint? roomId, ulong[] friendsList)
        {
            UserId = userMsg.UserId;
            Username = userMsg.Username;
            CurrentRoomId = roomId;
            FriendsList = friendsList;
        } 

        public FriendsAppUser(FriendsAppUserMsg msg)
        {
            UserId = msg.UserId;
            Username = msg.Username;
            CurrentRoomId = msg.InRoomCase == FriendsAppUserMsg.InRoomOneofCase.RoomId ? (uint?)msg.RoomId : null;
            FriendsList = msg.FriendList.ToArray();
        }

        public FriendsAppUserMsg ToNetworkModel()
        {
            FriendsAppUserMsg friendMsg = new FriendsAppUserMsg();
            friendMsg.UserId = UserId;
            friendMsg.Username = Username;
            if (CurrentRoomId.HasValue)
            {
                friendMsg.RoomId = CurrentRoomId.Value;
            }
            if (FriendsList != null)
            {
                friendMsg.FriendList.AddRange(FriendsList);
            }
            return friendMsg;
        }
    }
}
