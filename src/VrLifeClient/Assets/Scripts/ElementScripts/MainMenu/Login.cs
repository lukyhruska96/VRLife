﻿using Assets.Scripts.Core.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VrLifeClient;
using VrLifeClient.Core.Services.RoomService;

public class Login : MonoBehaviour
{
    private InputField _usernameField;
    private InputField _passwordField;
    private InputField _serverAddress;

    private ErrorUnityEvent _errorEvent = new ErrorUnityEvent();
    public ErrorUnityEvent ErrorEvent { get => _errorEvent; }


    void Start()
    {
        _errorEvent.AddListener(LogError);
        MenuEventSystem.current.SignedIn.AddListener(_ => LoggedIn());
        MenuEventSystem.current.RoomFound.AddListener(RoomFound);
        _usernameField = GameObject.Find("LoginGroup/Username")?.GetComponent<InputField>();
        _passwordField = GameObject.Find("LoginGroup/Password")?.GetComponent<InputField>();
        _serverAddress = GameObject.Find("MainServer")?.GetComponent<InputField>();
        if (_usernameField != null && _passwordField != null && _serverAddress != null)
        {
            MenuEventSystem.current.Login.AddListener(SignIn);
        }
    }

    public void OnButtonClick()
    {
        MenuEventSystem.current.Login.Invoke();
    }

    private void SignIn()
    {
        string username = _usernameField.text;
        string password = _passwordField.text;
        string[] address = _serverAddress.text.Split(':');
        VrLifeCore.API.Config.MainServer = new IPEndPoint(IPAddress.Parse(address[0]), int.Parse(address[1]));
        VrLifeCore.API.User.Login(username, password)
            .SetSucc(MenuEventSystem.current.SignedIn)
            .SetErr(_errorEvent)
            .Exec();
    }

    private void LoggedIn()
    {
        Debug.Log("Succesfuly logged in.");
        VrLifeCore.API.Room.QuickJoin().SetSucc(MenuEventSystem.current.RoomFound)
            .SetErr(_errorEvent).Exec();
    }

    private void RoomFound(Room room)
    {
        Debug.Log("Room found");
        Debug.Log($"{room.Name}[{room.Id}] with capacity {room.Capacity}");

        MenuEventSystem.current.AddMainThreadCallback(SceneController.current.ToRoom);
    }

    private void LogError(Exception e)
    {
        Debug.Log(e);
    }
}
