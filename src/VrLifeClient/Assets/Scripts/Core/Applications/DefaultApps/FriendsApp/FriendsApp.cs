using Assets.Scripts.Core.Services;
using Assets.Scripts.Core.Services.EventService;
using System.Collections.Generic;
using System.Linq;
using VrLifeAPI;
using VrLifeAPI.Client.API;
using VrLifeAPI.Client.Applications.BackgroundApp;
using VrLifeAPI.Client.Applications.DefaultApps.FriendsApp;
using VrLifeAPI.Client.Core.Services;
using VrLifeAPI.Common.Applications.DefaultApps.FriendsApp.NetworkingModels;
using VrLifeAPI.Common.Core.Applications.DefaultApps.FriendsApp;
using VrLifeAPI.Networking.NetworkingModels;
using VrLifeClient.API.OpenAPI;
using VrLifeShared.Core.Applications.DefaultApps.FriendsApp;

namespace Assets.Scripts.Core.Applications.DefaultApps.FriendsApp
{
    class FriendsApp : IFriendsApp
    {
        public const ulong APP_ID = 1;
        private const string NAME = "Friends";
        private const string DESC = "Provides ability add some user to friend list.";
        private AppInfo _info = new AppInfo(APP_ID, NAME, DESC, new AppVersion(new int[] { 1, 0, 0 }), AppType.APP_BACKGROUND);
        private IOpenAPI _api;

        public void Dispose()
        {

        }

        public AppInfo GetInfo()
        {
            return _info;
        }

        public void Init(IOpenAPI api)
        {
            _api = api;
        }

        public IServiceCallback<bool> SendFriendRequest(ulong userId)
        {
            return SendAddFriend(userId);
        }

        public IServiceCallback<bool> AcceptFriendRequest(ulong userId)
        {
            return SendAddFriend(userId);
        }

        public IServiceCallback<bool> RemoveFriend(ulong userId)
        {
            return SendRmFriend(userId);
        }

        public IServiceCallback<bool> UndoFriendRequest(ulong userId)
        {
            return SendRmFriend(userId);
        }

        public IServiceCallback<bool> DeleteFriendRequest(ulong userId)
        {
            return SendRmFriend(userId);
        }

        public IServiceCallback<IFriendsAppUser> GetFriendDetails(ulong userId)
        {
            return new ServiceCallback<IFriendsAppUser>(() => 
            {
                EventDataMsg msg = new EventDataMsg();
                msg.AppId = APP_ID;
                msg.EventType = (uint)FriendsAppEvents.GET_FRIEND;
                msg.LongValue = userId;
                byte[] data;
                try
                {
                    data = _api.Event.SendEvent(msg, EventRecipient.PROVIDER).Wait();
                }
                catch(EventServiceException e)
                {
                    throw new FriendsAppException(e.Message);
                }
                FriendsAppMsg friendsAppMsg = FriendsAppMsg.Parser.ParseFrom(data);
                if(friendsAppMsg == null || friendsAppMsg.FriendDetail == null)
                {
                    throw new FriendsAppException("Unknown response.");
                }
                return new FriendsAppUser(friendsAppMsg.FriendDetail);
            });
        }

        public IServiceCallback<List<IFriendsAppUser>> ListFriends()
        {
            return new ServiceCallback<List<IFriendsAppUser>>(() =>
            {
                EventDataMsg msg = new EventDataMsg();
                msg.AppId = APP_ID;
                msg.EventType = (uint)FriendsAppEvents.LIST_FRIENDS;
                byte[] data;
                try
                {
                    data = _api.Event.SendEvent(msg, EventRecipient.PROVIDER).Wait();
                }
                catch(EventServiceException e)
                {
                    throw new FriendsAppException(e.Message);
                }
                FriendsAppMsg friendsAppMsg = FriendsAppMsg.Parser.ParseFrom(data);
                if (friendsAppMsg == null || friendsAppMsg.FriendsList == null)
                {
                    throw new FriendsAppException("Unknown response.");
                }
                return friendsAppMsg.FriendsList.FriendsList
                    .Where(x => x != null)
                    .Select(x => new FriendsAppUser(x))
                    .ToList<IFriendsAppUser>();
            });
        }

        public IServiceCallback<List<IFriendsAppUser>> GetFriendRequests()
        {
            return new ServiceCallback<List<IFriendsAppUser>>(() => {
                EventDataMsg msg = new EventDataMsg();
                msg.AppId = APP_ID;
                msg.EventType = (uint)FriendsAppEvents.LIST_FRIEND_REQUESTS;
                byte[] data;
                try
                {
                    data = _api.Event.SendEvent(msg, EventRecipient.PROVIDER).Wait();
                }
                catch(EventServiceException e)
                {
                    throw new FriendsAppException(e.Message);
                }
                FriendsAppMsg friendsAppMsg = FriendsAppMsg.Parser.ParseFrom(data);
                if (friendsAppMsg == null || friendsAppMsg.FriendRequests == null)
                {
                    throw new FriendsAppException("Unknown response.");
                }
                return friendsAppMsg.FriendRequests.FriendRequestsList
                .Select(x => new FriendsAppUser(x))
                .Where(x => x != null).ToList<IFriendsAppUser>();
            });
        }

        private IServiceCallback<bool> SendAddFriend(ulong userId)
        {
            return new ServiceCallback<bool>(() =>
            {
                EventDataMsg msg = new EventDataMsg();
                msg.AppId = APP_ID;
                msg.EventType = (uint)FriendsAppEvents.ADD_FRIEND;
                msg.LongValue = userId;
                try
                {
                    _api.Event.SendEvent(msg, EventRecipient.PROVIDER).Wait();
                }
                catch(EventServiceException e)
                {
                    throw new FriendsAppException(e.Message);
                }
                return true;
            });
        }

        private IServiceCallback<bool> SendRmFriend(ulong userId)
        {
            return new ServiceCallback<bool>(() =>
            {
                EventDataMsg msg = new EventDataMsg();
                msg.AppId = APP_ID;
                msg.EventType = (uint)FriendsAppEvents.DEL_FRIEND;
                msg.LongValue = userId;
                try
                {
                    _api.Event.SendEvent(msg, EventRecipient.PROVIDER).Wait();
                }
                catch(EventServiceException e)
                {
                    throw new FriendsAppException(e.Message);
                }
                return true;
            });
        }
    }
}
