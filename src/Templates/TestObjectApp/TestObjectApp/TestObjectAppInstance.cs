using System;
using System.Collections.Generic;
using System.Text;
using VrLifeAPI.Client.Applications.ObjectApp;
using UnityEngine;
using VrLifeAPI.Common.Core.Utils;
using VrLifeAPI.Client.API.ObjectAPI;
using Google.Protobuf.WellKnownTypes;

namespace TestObjectApp
{
    class TestObjectAppInstance : IObjectAppInstance
    {
        private ObjectAppInfo _info;
        private Vector3 _center;
        private GameObject _go = null;
        private ulong _appInstance;
        private IObjectAPI _api;

        public TestObjectAppInstance(ulong appInstance, IObjectAPI api, ObjectAppInfo info, System.Numerics.Vector3 center)
        {
            _appInstance = appInstance;
            _info = info;
            _center = center.ToUnity();
            _api = api;
        }

        public void Dispose()
        {
            if(_go != null)
            {
                GameObject.Destroy(_go);
            }
        }

        public void FixGameObject()
        {
            _go.transform.position = _center;
        }

        public Vector3 GetCenter()
        {
            return _center;
        }

        public GameObject GetGameObject()
        {
            if(_go == null)
            {
                CreateGO();
            }
            return _go;
        }

        private void CreateGO()
        {
            _go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Vector3 scale = _go.transform.localScale;
            scale.y = 0.25f;
            _go.transform.localScale = scale;
            _go.AddComponent<TestObjectAppScript>();
            _go.GetComponent<MeshRenderer>().material = _api.GetDefaultMaterial();
        }

        public ObjectAppInfo GetObjectAppInfo()
        {
            return _info;
        }

        public void PlayerPointAt(ulong userId, Ray ray)
        {
        }

        public void PlayerSelect(ulong userId, Ray ray)
        {
        }
    }
}
