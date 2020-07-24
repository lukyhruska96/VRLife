using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using VrLifeAPI.Provider.Core.Services.AppService;
using VrLifeServer.Core.Services.AppService;
using VrLifeServer.Core.Utils;
using VrLifeServer.Database.DbModels;

namespace VrLifeServer.Core.Applications.DefaultApps.FriendsApp.Provider
{
    class FriendsAppData
    {
        private IAppDataService _appDataService;
        private const string FRIEND_REQUESTS_FIELD = "FriendRequests";
        private const string FRIENDS_LIST_FIELD = "FriendsList";
        private Dictionary<ulong, object> _friendLocks = new Dictionary<ulong, object>();
        public FriendsAppData(IAppDataService appDataService)
        {
            _appDataService = appDataService;
        }

        public List<ulong> GetFriendRequests(ulong userId)
        {
            string requests = _appDataService.Get(FRIEND_REQUESTS_FIELD, userId).StringVal;
            if(requests == null)
            {
                return new List<ulong>();
            }
            JArray arrRequests = JArray.Parse(requests);
            if(arrRequests == null)
            {
                return new List<ulong>();
            }
            return arrRequests.Select(x => (ulong)x).ToList();
        }

        public void SendFriendRequest(ulong fromUser, ulong toUser)
        {
            (object firstLock, object secLock) = GetTwoLocks(fromUser, toUser);
            lock(firstLock)
            {
                lock(secLock)
                {
                    List<ulong> requests = GetFriendRequests(toUser);
                    if (requests.Contains(fromUser))
                    {
                        return;
                    }
                    requests.Add(fromUser);
                    JArray jRequests = new JArray(requests);
                    DataValue val = new DataValue(FRIEND_REQUESTS_FIELD, jRequests.ToString());
                    _appDataService.UpdateOrInsert(toUser, val);
                }
            }
        }

        public List<ulong> GetFriendsList(ulong userId)
        {
            string strFriendsList = _appDataService.Get(FRIENDS_LIST_FIELD, userId).StringVal;
            if(strFriendsList == null)
            {
                return new List<ulong>();
            }
            JArray jFriendsList = JArray.Parse(strFriendsList);
            if(jFriendsList == null)
            {
                return new List<ulong>();
            }
            return jFriendsList.Select(x => (ulong)x).ToList();
        }

        public void AcceptFriendRequest(ulong fromUser, ulong toUser)
        {
            (object firstLock, object secondLock) = GetTwoLocks(fromUser, toUser);
            lock(firstLock)
            {
                lock(secondLock)
                {
                    List<ulong> requests = GetFriendRequests(toUser);
                    if (!requests.Contains(fromUser))
                    {
                        throw new FriendsAppProviderException("Cannot accept not existing friend request.");
                    }
                    requests.Remove(fromUser);
                    List<ulong> toFriendList = GetFriendsList(toUser);
                    if(toFriendList == null)
                    {
                        toFriendList = new List<ulong>();
                    }
                    if (!toFriendList.Contains(fromUser))
                    {
                        toFriendList.Add(fromUser);
                    }
                    List<ulong> fromFriendList = GetFriendsList(fromUser);
                    if(fromFriendList == null)
                    {
                        fromFriendList = new List<ulong>();
                    }
                    if(!fromFriendList.Contains(toUser))
                    {
                        fromFriendList.Add(toUser);
                    }
                    JArray jRequests = new JArray(requests);
                    DataValue requestValue = new DataValue(FRIEND_REQUESTS_FIELD, jRequests.ToString());
                    JArray jToFriendList = new JArray(toFriendList);
                    DataValue toFriendListValue = new DataValue(FRIENDS_LIST_FIELD, jToFriendList.ToString());
                    JArray jFromFriendList = new JArray(fromFriendList);
                    DataValue fromFriendListValue = new DataValue(FRIENDS_LIST_FIELD, jFromFriendList.ToString());
                    _appDataService.UpdateOrInsert(toUser, requestValue);
                    _appDataService.UpdateOrInsert(toUser, toFriendListValue);
                    _appDataService.UpdateOrInsert(fromUser, fromFriendListValue);
                }
            }
        }

        public void RemoveFriendRequest(ulong fromUser, ulong toUser)
        {
            lock(GetLock(toUser))
            {
                List<ulong> requests = GetFriendRequests(toUser);
                if (!requests.Contains(fromUser))
                {
                    return;
                }
                requests.Remove(fromUser);
                JArray jRequests = new JArray(requests);
                DataValue requestsValue = new DataValue(FRIEND_REQUESTS_FIELD, jRequests.ToString());
                _appDataService.UpdateOrInsert(toUser, requestsValue);
            }
        }

        public void RemoveFriend(ulong user1, ulong user2)
        {
            (object firstLock, object secondLock) = GetTwoLocks(user1, user2);
            lock(firstLock)
            {
                lock(secondLock)
                {
                    List<ulong> firstFriendList = GetFriendsList(user1);
                    if(firstFriendList == null)
                    {
                        firstFriendList = new List<ulong>();
                    }
                    firstFriendList.Remove(user2);
                    List<ulong> secondFriendList = GetFriendsList(user2);
                    if(secondFriendList == null)
                    {
                        secondFriendList = new List<ulong>();
                    }
                    secondFriendList.Remove(user1);
                    JArray jFirst = new JArray(firstFriendList);
                    JArray jSecond = new JArray(secondFriendList);
                    DataValue firstValue = new DataValue(FRIENDS_LIST_FIELD, jFirst.ToString());
                    DataValue secondValue = new DataValue(FRIENDS_LIST_FIELD, jSecond.ToString());
                    _appDataService.UpdateOrInsert(user1, firstValue);
                    _appDataService.UpdateOrInsert(user2, secondValue);
                }
            }
        }

        private object GetLock(ulong userId)
        {
            if (!_friendLocks.ContainsKey(userId))
            {
                _friendLocks.TryAdd(userId, new object());
            }
            return _friendLocks[userId];
        }

        private (object first, object second) GetTwoLocks(ulong userId1, ulong userId2)
        {
            if(userId1 == userId2)
            {
                return (GetLock(userId1), new object());
            }
            if(userId1 > userId2)
            {
                return (GetLock(userId2), GetLock(userId1));
            }
            return (GetLock(userId1), GetLock(userId2));
        }
    }
}
