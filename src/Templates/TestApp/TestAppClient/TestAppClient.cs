using Assets.Scripts.API.HUDAPI;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using VrLifeAPI;
using VrLifeAPI.Client.API;
using VrLifeAPI.Client.API.MenuAPI;
using VrLifeAPI.Client.Applications.MenuApp;
using VrLifeAPI.Client.Applications.MenuApp.MenuItems;
using VrLifeAPI.Client.Core.Services;
using VrLifeClient.API.HUDAPI;

namespace TestAppClient
{
    public class TestAppClient : IMenuApp
    {
        private AppInfo _info;
        private IMenuAPI _menuAPI;
        private IOpenAPI _api;
        private IHUDAPI _hudAPI;
        private IMenuItemGrid _root;
        private ulong _coroutine = ulong.MaxValue;
        private string[] notificationText = new string[] { "Do you want to know?", "Just click on me.",
                                                            "Don't be shy.", "Come on.", "Just one click."};

        public TestAppClient()
        {
            _info = new AppInfo(42, GetType().Name, "Super useful app.", 
                new AppVersion(new int[] { 1, 0, 0 }), AppType.APP_MENU);
        }

        public void Dispose()
        {
            if(_coroutine != ulong.MaxValue)
            {
                _menuAPI.StopCoroutine(_coroutine);
            }
        }

        public AppInfo GetInfo()
        {
            return _info;
        }

        public IMenuItem GetRootItem()
        {
            return _root;
        }

        public void HandleNotification(Notification notification)
        {

        }

        public void Init(IOpenAPI api, IMenuAPI menuAPI, IHUDAPI hudAPI)
        {
            _api = api;
            _menuAPI = menuAPI;
            _hudAPI = hudAPI;
            InitMenuItems();
            _menuAPI.StartCoroutine(AnnoyingFunction());
        }

        private void InitMenuItems()
        {
            _root = _menuAPI.CreateGrid("testGrid", 5, 5);
            _root.Enabled += OnEnabled;
            ReRender();
        }

        private void OnEnabled()
        {
            ReRender();
        }

        private void ReRender()
        {
            _root.GetChildren().ForEach(x => { _root.RemoveChild(x); x.Dispose(); });
            IMenuItemButton button = _menuAPI.CreateButton("button");
            _root.AddChild(1, 2, 3, 1, button);
            button.SetText("Tell me the secret");
            IMenuItemText secret = _menuAPI.CreateText("secret");
            _root.AddChild(2, 3, 1, 1, secret);
            secret.SetText("");
            button.Clicked += () =>
            {
                byte[] request = new byte[0];
                byte[] response = _api.App.SendAppMsg(_info, request, AppMsgRecipient.PROVIDER).Wait();
                int val = BitConverter.ToInt32(response, 0);
                secret.SetText(val.ToString());
                secret.SetFontSize(5, 30);
                secret.SetTextStyle(FontStyle.Bold);
                secret.SetTextColor(Color.black);
                secret.SetAlignment(TextAnchor.MiddleCenter);
            };
        }
        
        private IEnumerator AnnoyingFunction()
        {
            System.Random r = new System.Random();
            yield return _menuAPI.WaitForSeconds(5);
            Notification notif = new Notification(_info, "Hi");
            _hudAPI.ShowNotification(notif);
            while(true)
            {
                string text = notificationText[r.Next() % notificationText.Length];
                notif = new Notification(_info, text);
                yield return _menuAPI.WaitForSeconds(5);
            }
        }
    }
}
