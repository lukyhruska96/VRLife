using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Core.Character
{
    class ControlHUD
    {
        private const string CAMERA_PREFAB_PATH = "Room/CameraWithHUD";
        private GameObject _cameraInstance;
        public ControlHUD(GameObject head)
        {
            GameObject prefab = Resources.Load<GameObject>(CAMERA_PREFAB_PATH);
            if (prefab == null)
            {
                throw new CharacterLoadingException("CameraHUD's prefab could not be found.");
            }
            _cameraInstance = (UnityEngine.GameObject)UnityEngine.GameObject.Instantiate(prefab);
            _cameraInstance.transform.parent = head.transform;
            _cameraInstance.transform.localPosition = Vector3.zero;
        }
    }
}
