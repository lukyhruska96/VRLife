using Assets.Prefab.Avatar.Default;
using Assets.Scripts.Core.Utils;
using Assets.Scripts.ElementScripts.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using VrLifeAPI.Client.Core.Character;
using VrLifeAPI.Client.Core.Wrappers;

namespace Assets.Scripts.Core.Character
{
    class DefaultAvatar : IAvatar
    {
        private const string PREFAB_PATH = "Avatar/Default/Default";
        private UnityEngine.GameObject _avatarInstance = null;
        private ulong _userId;
        private DefaultController _defaultController;
        private SoundPlayer _player;

        public DefaultAvatar(ulong userId, string name, Vector3 position, Quaternion rotation)
        {
            this._userId = userId;
            GameObject prefab = Resources.Load<GameObject>(PREFAB_PATH);
            if (prefab == null)
            {
                throw new CharacterLoadingException("Character's prefab could not be found.");
            }
            _avatarInstance = (UnityEngine.GameObject)UnityEngine.GameObject.Instantiate(prefab, position, rotation);
            _avatarInstance.name = name;
            _defaultController = _avatarInstance.GetComponent<DefaultController>();
            _player = _avatarInstance.GetComponent<SoundPlayer>();
            _avatarInstance.GetComponent<PlayerState>().Avatar = this;
        }

        public ISoundPlayer GetSoundPlayer()
        {
            return _player;
        }

        public SkeletonState GetCurrentSkeleton()
        {
            return _defaultController.GetSkeleton();
        }

        public void SetSkeleton(SkeletonState skeleton)
        {
            _defaultController.SetSkeleton(skeleton);
        }

        public GameObject GetGameObject()
        {
            return _avatarInstance;
        }

        public void SetControls(bool enabled)
        {
            _avatarInstance.GetComponent<PlayerControls>().enabled = enabled;
            _avatarInstance.GetComponent<PlayerState>().enabled = enabled;
            _avatarInstance.GetComponent<Animator>().enabled = enabled;
        }

        public ulong GetUserId()
        {
            return _userId;
        }

        public void Destroy()
        {
            UnityEngine.GameObject.Destroy(_avatarInstance);
        }

        public GameObject GetHead()
        {
            return _defaultController.SkeletonParts[(int)SkeletonEnum.HEAD];
        }

        public GameObject[] GetSkeletonParts()
        {
            return _defaultController.GetSkeletonParts();
        }
    }
}
