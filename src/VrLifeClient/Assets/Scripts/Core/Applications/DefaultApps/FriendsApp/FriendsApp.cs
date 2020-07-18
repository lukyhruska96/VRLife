using Assets.Scripts.Core.Applications.BackgroundApp;
using Assets.Scripts.Core.Services;
using Assets.Scripts.Core.Services.EventService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VrLifeClient.API;
using VrLifeClient.API.OpenAPI;
using VrLifeClient.Core.Services.EventService;
using VrLifeClient.Core.Services.SystemService;
using VrLifeShared.Core.Applications;
using VrLifeShared.Core.Applications.DefaultApps.FriendsApp;
using VrLifeShared.Core.Applications.DefaultApps.FriendsApp.NetworkingModels;
using VrLifeShared.Networking.NetworkingModels;

namespace Assets.Scripts.Core.Applications.DefaultApps.FriendsApp
{
    class FriendsApp : IBackgroundApp
    {
        public const ulong APP_ID = 1;
        private const string NAME = "Friends";
        private const string DESC = "Provides ability add some user to friend list.";
        private AppInfo _info = new AppInfo(APP_ID, NAME, DESC, AppType.APP_BACKGROUND);
        private OpenAPI _api;

        public void Dispose()
        {

        }

        public AppInfo GetInfo()
        {
            return _info;
        }

        public void Init(OpenAPI api)
        {
            _api = api;
        }

        public ServiceCallback<bool> SendFriendRequest(ulong userId)
        {
            return SendAddFriend(userId);
        }

        public ServiceCallback<bool> AcceptFriendRequest(ulong userId)
        {
            return SendAddFriend(userId);
        }

        public ServiceCallback<bool> RemoveFriend(ulong userId)
        {
            return SendRmFriend(userId);
        }

        public ServiceCallback<bool> UndoFriendRequest(ulong userId)
        {
            return SendRmFriend(userId);
        }

        public ServiceCallback<bool> DeleteFriendRequest(ulong userId)
        {
            return SendRmFriend(userId);
        }

        public ServiceCallback<FriendsAppUser> GetFriendDetails(ulong userId)
        {
            return new ServiceCallback<FriendsAppUser>(() => 
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

        public ServiceCallback<List<FriendsAppUser>> ListFriends()
        {
            return new ServiceCallback<List<FriendsAppUser>>(() =>
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
                    .ToList();
            });
        }

        public ServiceCallback<List<FriendsAppUser>> GetFriendRequests()
        {
            return new ServiceCallback<List<FriendsAppUser>>(() => {
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
                .Where(x => x != null).ToList();
            });
        }

        private ServiceCallback<bool> SendAddFriend(ulong userId)
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

        private ServiceCallback<bool> SendRmFriend(ulong userId)
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
