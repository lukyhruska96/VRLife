using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using VrLifeAPI.Client.API.ObjectAPI;
using VrLifeAPI.Client.Applications.ObjectApp;
using VrLifeAPI.Client.Core.Wrappers;
using VrLifeClient.API;

namespace VrLifeClient.API.ObjectAPI
{
    class ObjectAPI : IObjectAPI
    {
        private ClosedAPI _api;
        public ObjectAPI(ClosedAPI api)
        {
            this._api = api;
        }

        public string DefaltShaderPath()
        {
            return "Universal Render Pipeline/Lit";
        }

        public Shader FindShader(string shaderPath)
        {
            return ObjectAPIController.current?.FindShader(shaderPath);
        }

        public Material GetDefaultMaterial()
        {
            return ObjectAPIController.current?.DefaultMaterial;
        }
    }
}
