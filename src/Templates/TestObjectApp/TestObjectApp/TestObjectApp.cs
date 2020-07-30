using System;
using System.Numerics;
using VrLifeAPI;
using VrLifeAPI.Client.API;
using VrLifeAPI.Client.API.ObjectAPI;
using VrLifeAPI.Client.Applications.ObjectApp;

namespace TestObjectApp
{
    public class TestObjectApp : IObjectApp
    {
        private AppInfo _info = new AppInfo(43, "TestObjectApp", "Testing object app.",
            new AppVersion(new int[] { 1, 0, 0 }), AppType.APP_OBJECT);
        private ObjectAppInfo _objectInfo = new ObjectAppInfo(3, 3);
        private IOpenAPI _api;
        private IObjectAPI _objectAPI;

        public void Init(IOpenAPI api, IObjectAPI objectAPI)
        {
            _api = api;
            _objectAPI = objectAPI;
        }

        public ObjectAppInfo GetObjectAppInfo()
        {
            return _objectInfo;
        }

        public IObjectAppInstance CreateInstance(ulong appInstance, Vector3 center)
        {
            return new TestObjectAppInstance(appInstance, _objectAPI, _objectInfo, center);
        }

        public AppInfo GetInfo()
        {
            return _info;
        }

        public void Dispose()
        {

        }
    }
}
