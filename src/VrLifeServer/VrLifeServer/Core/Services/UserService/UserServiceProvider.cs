using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VrLifeServer.API;
using VrLifeServer.Core.Services.SystemService;
using VrLifeServer.Database;
using VrLifeServer.Database.DbModels;
using VrLifeServer.Logging;
using VrLifeServer.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.UserService
{

    class UserServiceProvider : IUserService
    {
        private ClosedAPI _api;
        private ILogger _log;

        private List<User> _clientMachines = new List<User>();

        public MainMessage HandleMessage(MainMessage msg)
        {
            if(msg.MessageTypeCase != MainMessage.MessageTypeOneofCase.UserMngMsg)
            {
                _log.Error("Cannot handle this type of message.");
                return ISystemService.CreateErrorMessage(0, 0, 0, 
                    this.GetType().Name + ": Cannot handle this type of message.");
            }
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

        public void Init(ClosedAPI api)
        {
            this._api = api;
            _log = api.OpenAPI.CreateLogger(this.GetType().Name);
            // empty object to fill 0 index
            _clientMachines.Clear();
            _clientMachines.Add(null);
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
            MainMessage response = ISystemService.CreateOkMessage(msg.MsgId);

            // clientId == 0 --> unsassigned ID
            if(msg.ClientId != 0 && msg.ClientId < _clientMachines.Count)
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
            _log.Error("This server does not handle UserMsg.");
            return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, 
                "This server does not handle UserMsg.");
        }
    }
}
