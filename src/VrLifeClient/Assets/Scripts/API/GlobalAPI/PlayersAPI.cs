using Assets.Scripts.Core.Character;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VrLifeAPI.Client.API.GlobalAPI;
using VrLifeAPI.Client.Core.Wrappers;

namespace VrLifeClient.API.GlobalAPI
{
    class PlayersAPI : IPlayersAPI
    {
        private ClosedAPI _api;
        private Dictionary<ulong, IAvatar> _avatars = new Dictionary<ulong, IAvatar>();
        private ControlHUD _hud = null;
        private IAvatar _mainAvatar = null;

        public PlayersAPI(ClosedAPI api)
        {
            this._api = api;
            this._api.Services.Room.RoomExited += Reset;
        }

        public ReadOnlyDictionary<ulong, IAvatar> GetAvatars()
        {
            return new ReadOnlyDictionary<ulong, IAvatar>(_avatars);
        }

        public IAvatar GetMainAvatar()
        {
            return _mainAvatar;
        }

        public ControlHUD GetControlHUD()
        {
            return _hud;
        }

        public void Init(IAvatar mainAvatar)
        {
            _mainAvatar = mainAvatar;
        }

        public void AddAvatar(ulong userId, IAvatar avatar)
        {
            if(_avatars.ContainsKey(userId))
            {
                avatar.Destroy();
                throw new PlayersAPIException("Already contains avatar with this userId.");
            }
            avatar.SetControls(false);
            _avatars.Add(userId, avatar);
        }

        public void AddMainAvatar(IAvatar avatar)
        {
            if(_mainAvatar != null)
            {
                _mainAvatar.Destroy();
            }
            _mainAvatar = avatar;
            _mainAvatar.SetControls(true);
            _hud = new ControlHUD(_mainAvatar.GetHead());
        }

        public IAvatar GetAvatar(ulong userId)
        {
            if (_avatars.TryGetValue(userId, out IAvatar val))
            {
                return val;
            }
            return null;
        }

        public void DeleteAvatar(ulong userId)
        {
            if(!_avatars.ContainsKey(userId))
            {
                return;
            }
            IAvatar avatar = _avatars[userId];
            _avatars.Remove(userId);
            avatar.Destroy();
        }

        private void Reset()
        {
            _mainAvatar.Destroy();
            _mainAvatar = null;
            _avatars.Values.ToList().ForEach(x => x.Destroy());
            _avatars.Clear();
        }
    }
}
