using Assets.Scripts.Core.Wrappers;
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

        public DefaultAvatar(String name, Vector3 position, Quaternion rotation)
        {
            UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath(_prefabPath, typeof(GameObject));
            if(prefab == null)
            {
                throw new CharacterLoadingException("Character's prefab could not be found.");
            }
            _avatarInstance = (GameObject) GameObject.Instantiate(prefab, position, rotation);
            _avatarInstance.name = name;
        }

        public Skeleton GetCurrentSkeleton()
        {
            throw new NotImplementedException();
        }

        public void SetSkeleton(Skeleton skeleton)
        {
            throw new NotImplementedException();
        }

        public GameObject GetGameObject()
        {
            return _avatarInstance;
        }
    }
}
