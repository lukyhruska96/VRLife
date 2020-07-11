using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VrLifeShared.Core.Applications.DefaultApps.FriendsApp.NetworkingModels;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeShared.Core.Applications.DefaultApps.FriendsApp
{
    public class FriendsAppUser
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
            if(CurrentRoomId.HasValue)
            {
                friendMsg.RoomId = CurrentRoomId.Value;
            }
            friendMsg.FriendList.Add(FriendsList);
            return friendMsg;
        }
    }
}
