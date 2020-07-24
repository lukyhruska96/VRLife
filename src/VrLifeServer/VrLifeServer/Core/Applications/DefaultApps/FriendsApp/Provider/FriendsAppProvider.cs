using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using VrLifeAPI;
using VrLifeAPI.Provider.API;
using VrLifeAPI.Common.Core.Applications;
using VrLifeAPI.Common.Core.Services.UserService;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeAPI.Provider.Core.Applications.DefaultApps.FriendsApp;
using VrLifeAPI.Provider.Core.Services.AppService;
using VrLifeServer.API.Provider;
using VrLifeServer.Core.Services.UserService;
using VrLifeShared.Core.Applications.DefaultApps.FriendsApp;
using VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels;
using VrLifeAPI.Client.Applications.DefaultApps.FriendsApp;
using VrLifeAPI.Common.Core.Applications.DefaultApps.FriendsApp;
using VrLifeAPI.Common.Core.Services;

namespace VrLifeServer.Core.Applications.DefaultApps.FriendsApp.Provider
{
    class FriendsAppProvider : IFriendsAppProvider
    {
        public const ulong APP_ID = 1;
        private const string NAME = "Friends";
        private const string DESC = "Provides ability add some user to friend list.";
        private IClosedAPI _api;
        private FriendsAppData _friendsData;
        private AppInfo _info = new AppInfo(APP_ID, NAME, DESC, new AppVersion(new int[] { 1, 0, 0 }), AppType.APP_BACKGROUND);

        public void Dispose()
        {

        }

        public AppInfo GetInfo()
        {
            return _info;
        }

        public byte[] HandleEvent(EventDataMsg eventData, MsgContext ctx)
        {
            switch ((FriendsAppEvents)eventData.EventType)
            {
                case FriendsAppEvents.ADD_FRIEND:
                    return HandleAddFriend(eventData, ctx);
                case FriendsAppEvents.DEL_FRIEND:
                    return HandleDelFriend(eventData, ctx);
                case FriendsAppEvents.GET_FRIEND:
                    return HandleGetFriend(eventData, ctx);
                case FriendsAppEvents.LIST_FRIEND_REQUESTS:
                    return HandleListFriendRequests(eventData, ctx);
                case FriendsAppEvents.LIST_FRIENDS:
                    return HandleListFriends(eventData, ctx);
                default:
                    throw new FriendsAppProviderException("Unknown event type.");
            }
        }

        private byte[] HandleAddFriend(EventDataMsg eventMsg, MsgContext ctx)
        {
            if(ctx.senderType != SenderType.USER)
            {
                throw new FriendsAppProviderException("Only client machine can call this event.");
            }
            IUser userReq = _api.Services.User.GetUserByClientId(ctx.senderId);
            if(userReq == null)
            {
                throw new FriendsAppProviderException("You must be signedIn to add some friends.");
            }
            User userTo = User.Get(eventMsg.LongValue);
            if(userTo == null)
            {
                throw new FriendsAppProviderException("User with this ID could not be found.");
            }
            List<ulong> requests = _friendsData.GetFriendRequests(userReq.Id);
            if(requests.Contains(userTo.Id))
            {
                _friendsData.AcceptFriendRequest(userTo.Id, userReq.Id);
            }
            else
            {
                _friendsData.SendFriendRequest(userReq.Id, userTo.Id);
            }
            return null;
        }

        private byte[] HandleDelFriend(EventDataMsg eventMsg, MsgContext ctx)
        {
            IUser userReq = _api.Services.User.GetUserByClientId(ctx.senderId);
            if (userReq == null)
            {
                throw new FriendsAppProviderException("You must be signedIn to add some friends.");
            }
            User userTo = User.Get(eventMsg.LongValue);
            if (userTo == null)
            {
                throw new FriendsAppProviderException("User with this ID could not be found.");
            }
            List<ulong> friendList = _friendsData.GetFriendsList(userReq.Id);
            if(friendList.Contains(userTo.Id))
            {
                _friendsData.RemoveFriend(userReq.Id, userTo.Id);
            }
            else
            {
                List<ulong> fromRequests = _friendsData.GetFriendRequests(userReq.Id);
                if(fromRequests.Contains(userTo.Id))
                {
                    _friendsData.RemoveFriendRequest(userTo.Id, userReq.Id);
                }
                else
                {
                    _friendsData.RemoveFriendRequest(userReq.Id, userTo.Id);
                }
            }
            return null;
        }

        private byte[] HandleListFriendRequests(EventDataMsg eventMsg, MsgContext ctx)
        {
            IUser userReq = _api.Services.User.GetUserByClientId(ctx.senderId);
            if (userReq == null)
            {
                throw new FriendsAppProviderException("You must be signedIn to add some friends.");
            }
            List<ulong> requests = _friendsData.GetFriendRequests(userReq.Id);
            List<FriendsAppUser> requestUsers = requests
                .Select(x => User.Get(x))
                .Where(x => x != null)
                .Select(x => new FriendsAppUser(x.ToMessage(), null, null))
                .ToList<FriendsAppUser>();
            FriendsAppMsg msg = new FriendsAppMsg();
            msg.FriendRequests = new FriendsAppRequestsMsg();
            msg.FriendRequests.ToUser = userReq.Id;
            msg.FriendRequests.FriendRequestsList.AddRange(requestUsers.Select(x => x.ToNetworkModel()));
            return msg.ToByteArray();
        }

        private byte[] HandleListFriends(EventDataMsg eventMsg, MsgContext ctx)
        {
            IUser userReq = _api.Services.User.GetUserByClientId(ctx.senderId);
            if (userReq == null)
            {
                throw new FriendsAppProviderException("You must be signedIn to add some friends.");
            }
            List<ulong> friendIds = _friendsData.GetFriendsList(userReq.Id);
            if(friendIds == null)
            {
                friendIds = new List<ulong>();
            }
            List<FriendsAppUser> friends = friendIds.Select(x => ToFriendsAppUser(x)).ToList();
            FriendsAppMsg msg = new FriendsAppMsg();
            msg.FriendsList = new FriendsAppListMsg();
            msg.FriendsList.FriendsList.AddRange(friends.Select(x => x.ToNetworkModel()));
            return msg.ToByteArray();
        }

        private byte[] HandleGetFriend(EventDataMsg eventMsg, MsgContext ctx)
        {
            IUser userReq = _api.Services.User.GetUserByClientId(ctx.senderId);
            if (userReq == null)
            {
                throw new FriendsAppProviderException("You must be signedIn to add some friends.");
            }
            User userTo = User.Get(eventMsg.LongValue);
            if (userTo == null)
            {
                throw new FriendsAppProviderException("User with this ID could not be found.");
            }
            List<ulong> friends = _friendsData.GetFriendsList(userReq.Id);
            if(!friends.Contains(userTo.Id))
            {
                throw new FriendsAppProviderException("This user is not your friend.");
            }
            FriendsAppUser friendDetail = ToFriendsAppUser(userTo.Id);
            FriendsAppMsg msg = new FriendsAppMsg();
            msg.FriendDetail = friendDetail.ToNetworkModel();
            return msg.ToByteArray();
        }

        public byte[] HandleMessage(byte[] data, int size, MsgContext context)
        {
            throw new FriendsAppProviderException("FriendsAppProvider accepts requests only through event system.");
        }

        public void Init(IOpenAPI api, IAppDataService appDataService, IAppDataStorage dataStorage)
        {
            _api = api.GetClosedAPI(_info);
            _friendsData = new FriendsAppData(appDataService);
        }

        private FriendsAppUser ToFriendsAppUser(ulong userId)
        {
            User user = User.Get(userId);
            if(user == null)
            {
                return null;
            }
            uint? roomId = _api.Services.Room.RoomIdByUserId(userId);
            ulong[] friends = _friendsData.GetFriendsList(userId).ToArray();
            return new FriendsAppUser(user.ToMessage(), roomId, friends);
        }
    }
}
