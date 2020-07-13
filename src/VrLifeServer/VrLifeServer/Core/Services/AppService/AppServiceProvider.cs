using System;
using System.Collections.Generic;
using System.Text;
using VrLifeServer.API.Provider;
using VrLifeServer.Applications;
using VrLifeServer.Core.Services.EventService;
using VrLifeServer.Core.Services.SystemService;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.AppService
{
    class AppServiceProvider : IAppServiceProvider
    {
        private ClosedAPI _api;
        private Dictionary<ulong, IApplicationProvider> _appInstances = 
            new Dictionary<ulong, IApplicationProvider>();
        public byte[] HandleEvent(MainMessage msg)
        {
            EventDataMsg eventData = msg.EventMsg.EventDataMsg;
            if (eventData == null)
            {
                throw new EventErrorException("Invalid EventData msg.");
            }
            ulong appId = eventData.AppId;
            if (!_appInstances.ContainsKey(appId))
            {
                throw new EventErrorException("No handler could be found for this event.");
            }
            try
            {
                return _appInstances[appId].HandleEvent(eventData, new MsgContext(msg));
            }
            catch(Exception e)
            {
                throw new EventErrorException(e.Message);
            }
        }

        public MainMessage HandleMessage(MainMessage msg)
        {
            AppMsg appMsg = msg.AppMsg;
            ulong appId = appMsg.AppId;
            if (!_appInstances.ContainsKey(appId))
            {
                return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, "No handler could be found for this event.");
            }
            MainMessage response = new MainMessage();
            byte[] data = appMsg.Data.ToByteArray();
            try
            {
                response.AppMsg = _appInstances[appId].HandleMessage(data, data.Length, new MsgContext(msg));
            }
            catch (Exception e)
            {
                return ISystemService.CreateErrorMessage(msg.MsgId, 0, 0, e.Message);
            }
            return response;
        }

        public void Init(ClosedAPI api)
        {
            _api = api;
            InitDefaultApps();
        }

        public void InitDefaultApps()
        {
            foreach(IApplicationProvider app in _api.OpenAPI.Apps)
            {
                app.Init(_api.OpenAPI, new AppDataService(app.GetInfo().ID));
                _appInstances.Add(app.GetInfo().ID, app);
            }
        }

        public void RegisterApp(IApplicationProvider app)
        {
            if (app == null)
            {
                throw new AppServiceException("Given app cannot be null.");
            }
            if (_appInstances.ContainsKey(app.GetInfo().ID))
            {
                app.Dispose();
                throw new AppServiceException("Another instance of this app is already registered.");
            }
            app.Init(_api.OpenAPI, new AppDataService(app.GetInfo().ID));
            _appInstances.Add(app.GetInfo().ID, app);
        }
    }
}
