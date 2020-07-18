using Assets.Scripts.API.HUDAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VrLifeClient;

public class MenuControl : MonoBehaviour
{
    public static MenuControl current;
    public UnityEvent ExitMenu { get; set; } = new UnityEvent();

    public void Awake()
    {
        current = this;
        gameObject.SetActive(false);
    }
    public void Logout()
    {
        VrLifeCore.API.User.Logout();
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnExitMenu()
    {
        gameObject.SetActive(false);
        ExitMenu?.Invoke();
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
        MenuAppContainer.current?.OnOpenMenu();
    }

    public bool IsMenuOpen()
    {
        return gameObject.activeSelf;
    }

    public void HandleNotification(Notification notification)
    {
        if(MenuDropdownList.current == null)
        {
            return;
        }
        if(!IsMenuOpen())
        {
            OpenMenu();
        }
        MenuDropdownList.current.HandleNotification(notification);
    }

    private void OnDestroy()
    {
        current = null;
    }
}
