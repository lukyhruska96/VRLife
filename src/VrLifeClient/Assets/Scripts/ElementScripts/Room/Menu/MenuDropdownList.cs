using Assets.Scripts.API.HUDAPI;
using Assets.Scripts.Core.Applications.DefaultApps.RoomListApp;
using Assets.Scripts.Core.Applications.MenuApp;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VrLifeClient;

public class MenuDropdownList : MonoBehaviour
{
    public static MenuDropdownList current = null;
    private List<IMenuApp> _apps;
    private ulong? _lastActive = null;
    private Dropdown _dropdown;
    
    void Awake()
    {
        current = this;
        _dropdown = GetComponent<Dropdown>();
        _dropdown.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnEnable()
    {
        _apps = VrLifeCore.API.DefaultApps.AppManager.MenuApps;
        _dropdown.onValueChanged.AddListener(OnValueChanged);
        _dropdown.options.Clear();
        _dropdown.AddOptions(_apps.Select(x => x.GetInfo().Name).ToList());
        if (_apps.Count != 0)
        {
            if(_lastActive.HasValue)
            {
                MenuAppContainer.current?.SetView(_apps.Find(x => x.GetInfo().ID == _lastActive).GetRootItem());
            }
            else
            {
                MenuAppContainer.current?.SetView(_apps[0].GetRootItem());
            }
        }
    }

    public void Start()
    {
        if (_apps.Count != 0)
        {
            MenuAppContainer.current?.SetView(_apps[0].GetRootItem());
        }
    }

    public void HandleNotification(Notification notification)
    {
        IMenuApp app = _apps.Find(x => x.GetInfo().ID == notification.AppId);
        if (MenuAppContainer.current == null || app == null)
        {
            return;
        }
        _dropdown.value = _apps.IndexOf(app);
        bool ok = MenuAppContainer.current.SetView(app.GetRootItem());
        if(ok)
        {
            app.HandleNotification(notification);
        }
    }

    private void OnValueChanged(int idx)
    {
        _lastActive = _apps[idx].GetInfo().ID;
        MenuAppContainer.current.SetView(_apps[idx].GetRootItem());
    }

    private void OnDestroy()
    {
        current = null;
    }
}
