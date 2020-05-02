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

        public MainMessage HandleMessage(MainMessage msg)
        {
            if(msg.MessageTypeCase != MainMessage.MessageTypeOneofCase.UserMngMsg)
            {
                _log.Error("Cannot handle this type of message.");
                return ISystemService.CreateErrorMessage(0, 0, 0, 
                    this.GetType().Name + ": Cannot handle this type of message.");
            }
            UserMngMsg userMngMsg = msg.UserMngMsg;
            switch(userMngMsg.UserMngMsgTypeCase)
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
        }

        private MainMessage HandleAuthMsg(MainMessage msg)
        {
            _log.Debug("Received authorization message.");
            AuthMsg authMsg = msg.UserMngMsg.AuthMsg;
            string username = authMsg.Username;
            string password = authMsg.Password;

            // TODO !!!
            // Even client have to send hi msg first to obtain clientId.
            // After identification of device, the client can send authorization message.

            if (msg.ClientId == 0)
            {
                _log.Debug("Client does not have ID. Send Hi msg first.");
                return ISystemService.CreateErrorMessage(0, 0, 0,
                    "Client does not have ID. Send Hi msg first.");
            }

            using (var context = new VrLifeDbContext())
            {
                Account acc = context.Accounts.Single(x => x.Username == username);
                if(acc == null)
                {
                    _log.Debug("Could not find user with this username.");
                    return ISystemService.CreateErrorMessage(0, 0, 0,
                        "Could not find user with this username.");
                }
                if(acc.Passphrase != password)
                {
                    _log.Debug("Invalid password.");
                    return ISystemService.CreateErrorMessage(0, 0, 0,
                        "Invalid password.");
                }
                return ISystemService.CreateOkMessage(msg.MsgId);
            }
        }

        private MainMessage HandleUserMsg(MainMessage msg)
        {

        }
    }
}
