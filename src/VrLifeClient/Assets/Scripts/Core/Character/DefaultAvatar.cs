using Assets.Prefab.Avatar.Default;
using Assets.Scripts.Core.Utils;
using Assets.Scripts.Core.Wrappers;
using Assets.Scripts.ElementScripts.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Core.Character
{
    class DefaultAvatar : IAvatar
    {
        private const string _prefabPath = "Assets/Prefab/Avatar/Default/Default.prefab";
        private GameObject _avatarInstance = null;
        private ulong _userId;

        public DefaultAvatar(ulong userId, string name, Vector3 position, Quaternion rotation)
        {
            this._userId = userId;
            UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath(_prefabPath, typeof(GameObject));
            if (prefab == null)
            {
                throw new CharacterLoadingException("Character's prefab could not be found.");
            }
            _avatarInstance = (GameObject) GameObject.Instantiate(prefab, position, rotation);
            _avatarInstance.name = name;
            _avatarInstance.GetComponent<PlayerState>().avatar = this;
        }

        public SkeletonState GetCurrentSkeleton()
        {
            return _avatarInstance.GetComponent<DefaultController>().GetSkeleton();
        }

        public void SetSkeleton(SkeletonState skeleton)
        {
            _avatarInstance.GetComponent<DefaultController>().SetSkeleton(skeleton);
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
            GameObject.Destroy(_avatarInstance);
        }
    }
}
