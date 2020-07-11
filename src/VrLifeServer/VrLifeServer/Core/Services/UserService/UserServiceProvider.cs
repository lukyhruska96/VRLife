using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VrLifeServer.API.Provider;
using VrLifeServer.Core.Services.SystemService;
using VrLifeServer.Database;
using VrLifeServer.Database.DbModels;
using VrLifeShared.Logging;
using VrLifeShared.Networking;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.UserService
{

    class UserServiceProvider : IUserServiceProvider
    {
        private ClosedAPI _api;
        private ILogger _log;

        private List<User> _clientMachines = new List<User>();

        public void Init(ClosedAPI api)
        {
            this._api = api;
            _log = api.OpenAPI.CreateLogger(this.GetType().Name);
            // empty object to fill 0 index
            _clientMachines.Clear();
            _clientMachines.Add(null);
        }

        public User GetUserByClientId(ulong clientId)
        {
            if(clientId >=(ulong)_clientMachines.Count)
            {
                return null;
            }
            return _clientMachines[(int)clientId];
        }
           
        /// <summary>
        /// Message Handling of UserService on Provider's side.
        /// 
        /// - AuthMsg
        /// 
        /// - UserMsg
        ///   - CreateQuery
        ///   - ListQuery
        ///   - UpdateQuery
        ///   - UserDetail
        /// 
        /// </summary>
        /// <param name="msg">The received message.</param>
        /// <returns>Response message.</returns>

        public MainMessage HandleMessage(MainMessage msg)
        {
            UserMngMsg userMngMsg = msg.UserMngMsg;
            switch (userMngMsg.UserMngMsgTypeCase)
            {
                case UserMngMsg.UserMngMsgTypeOneofCase.AuthMsg:
                    return HandleAuthMsg(msg);
                case UserMngMsg.UserMngMsgTypeOneofCase.UserMsg:
                    return HandleUserMsg(msg);
                default:
                    return ISystemService.CreateErrorMessage(0, 0, 0,
                        this.GetType().Name + ": Cannot handle this type of message.");
            }
        }

        private MainMessage HandleAuthMsg(MainMessage msg)
        {
            _log.Debug("Received authorization message.");
            AuthMsg authMsg = msg.UserMngMsg.AuthMsg;
            string username = authMsg.Username;
            string password = authMsg.Password;

            User user = User.Get(username);
            if(user == null) 
            { 
                _log.Debug("Could not find user with this username.");
                return ISystemService.CreateErrorMessage(0, 0, 0,
                    "Could not find user with this username.");
            }
            if(!user.CheckPassword(password))
            {
                _log.Debug("Invalid password.");
                return ISystemService.CreateErrorMessage(0, 0, 0,
                    "Invalid password.");
            }
            MainMessage response = new MainMessage();
            response.UserMngMsg = new UserMngMsg();
            response.UserMngMsg.UserMsg = new UserMsg();
            response.UserMngMsg.UserMsg.UserDetailMsg = user.ToMessage();
            response.UserMngMsg.UserMsg.UserDetailMsg.Password = "";

            // clientId == 0 --> unsassigned ID
            if(msg.ClientId != 0 && msg.ClientId < (ulong)_clientMachines.Count)
            {
                _clientMachines[(int)msg.ClientId] = user;
                response.ClientId = msg.ClientId;
            }
            else
            {
                response.ClientId = (uint)_clientMachines.Count;
                _clientMachines.Add(user);
            }
            return response;
        }

        private MainMessage HandleUserMsg(MainMessage msg)
        {
            UserMsg userMsg = msg.UserMngMsg.UserMsg;
            switch(userMsg.UserMsgTypeCase)
            {
                case UserMsg.UserMsgTypeOneofCase.UserRequestMsg:
                    return HandleUserRequest(msg);
                default:
                    return ISystemService.CreateErrorMessage(0, 0, 0,
                        this.GetType().Name + ": Cannot handle this type of message.");
            }
        }

        private MainMessage HandleUserRequest(MainMessage msg)
        {
            _log.Debug("In HandleUserRequest method.");
            try
            {
                // TODO needs enhancemnets
                UserRequestMsg userRequest = msg.UserMngMsg.UserMsg.UserRequestMsg;
                switch (userRequest.UserRequestTypeCase)
                {
                    // create new User
                    case UserRequestMsg.UserRequestTypeOneofCase.CreateQuery:
                        _log.Debug("Creating new user: " + userRequest.CreateQuery.Username);
                        User user = User.Register(userRequest.CreateQuery.Username, userRequest.CreateQuery.Password);
                        UserMsg userMsg = new UserMsg();
                        userMsg.UserDetailMsg = user.ToMessage();
                        UserMngMsg userMngMsg = new UserMngMsg();
                        userMngMsg.UserMsg = userMsg;
                        _log.Debug("User sucessfuly created.");
                        return new MainMessage { UserMngMsg = userMngMsg };

                    // List users (Username LIKE '%msg.username%')
                    case UserRequestMsg.UserRequestTypeOneofCase.ListQuery:
                        _log.Debug("Looking for user like: " + userRequest.ListQuery.Username);
                        UserDetailMsg userDetail = userRequest.ListQuery;
                        UserDetailMsg[] users = User.List(userDetail)
                            .Select(x => new UserDetailMsg { Password = "", UserId = x.Id, Username = x.Username })
                            .ToArray();
                        UserListMsg userList = new UserListMsg();
                        userList.Users.AddRange(users);
                        UserMngMsg userMngMsg1 = new UserMngMsg();
                        userMngMsg1.UserMsg = new UserMsg { UserListMsg = userList };
                        _log.Debug($"Found {users.Length} records.");
                        return new MainMessage { UserMngMsg = userMngMsg1 };

                    // Change password, where old and new password are in same filed, separated by newline
                    case UserRequestMsg.UserRequestTypeOneofCase.UpdateQuery:
                        _log.Debug("Changing password to user: " + userRequest.UpdateQuery.Username);
                        UserDetailMsg updateQuery = userRequest.UpdateQuery;
                        User user1 = User.Get(updateQuery.UserId);
                        string[] passwords = updateQuery.Password.Split('\n');
                        user1.ChangePassword(passwords[0], passwords[1]);
                        _log.Debug("Password sucessfuly changed.");
                        return ISystemService.CreateOkMessage(msg.MsgId);

                    // show user by its Id
                    case UserRequestMsg.UserRequestTypeOneofCase.UserIdDetail:
                        _log.Debug($"Looking for user with id {userRequest.UserIdDetail}");
                        User user2 = User.Get(userRequest.UserIdDetail);
                        UserDetailMsg userDetail1 = new UserDetailMsg();
                        userDetail1.UserId = user2.Id;
                        userDetail1.Password = "";
                        userDetail1.Username = user2.Username;
                        UserMsg userMsg1 = new UserMsg();
                        userMsg1.UserDetailMsg = userDetail1;
                        return new MainMessage { UserMngMsg = new UserMngMsg { UserMsg = userMsg1 } };
                    case UserRequestMsg.UserRequestTypeOneofCase.UserByClientId:
                        _log.Debug($"Looking for user with client id {userRequest.UserByClientId}");
                        if(msg.SenderIdCase != MainMessage.SenderIdOneofCase.ServerId)
                        {
                            return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Not enough permissions.");
                        }
                        ulong clientId = userRequest.UserByClientId;
                        if(clientId >= (ulong)_clientMachines.Count || _clientMachines[(int)clientId] == null)
                        {
                            return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "Unable to find user with this client ID");
                        }
                        MainMessage respUserDetail = new MainMessage();
                        respUserDetail.UserMngMsg = new UserMngMsg();
                        respUserDetail.UserMngMsg.UserMsg = new UserMsg();
                        respUserDetail.UserMngMsg.UserMsg.UserDetailMsg = _clientMachines[(int)clientId].ToMessage();
                        return respUserDetail;
                    default:
                        throw new ErrorMsgException("This message cannot be empty.");
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return ISystemService.CreateErrorMessage(0, 0, 0, ex.Message);
            }
        }
    }
}
