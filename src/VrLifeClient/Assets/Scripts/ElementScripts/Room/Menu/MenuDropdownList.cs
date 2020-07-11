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
    private List<IMenuApp> _apps;
    private ulong? _lastActive = null;
    void Awake()
    {
        Dropdown dropdown = GetComponent<Dropdown>();
        dropdown.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnEnable()
    {
        _apps = VrLifeCore.API.Apps.AppManager.MenuApps;
        Dropdown dropdown = GetComponent<Dropdown>();
        dropdown.onValueChanged.AddListener(OnValueChanged);
        dropdown.options.Clear();
        dropdown.AddOptions(_apps.Select(x => x.GetInfo().Name).ToList());
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

    private void OnValueChanged(int idx)
    {
        _lastActive = _apps[idx].GetInfo().ID;
        MenuAppContainer.current.SetView(_apps[idx].GetRootItem());
    }
}
