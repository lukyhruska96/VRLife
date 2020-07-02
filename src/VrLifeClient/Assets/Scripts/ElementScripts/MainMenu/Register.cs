using Assets.Scripts.Core.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VrLifeClient;

public class Register : MonoBehaviour
{
    private InputField _usernameField;
    private InputField _passwordField;
    private InputField _serverAddress;

    private ErrorUnityEvent _errorEvent = new ErrorUnityEvent();
    public ErrorUnityEvent ErrorEvent { get => _errorEvent; }

    void Start()
    {
        MenuEventSystem.current.SignedUp.AddListener(_ => SignedUp());
        _usernameField = GameObject.Find("RegisterGroup/Username")?.GetComponent<InputField>();
        _passwordField = GameObject.Find("RegisterGroup/Password")?.GetComponent<InputField>();
        _serverAddress = GameObject.Find("MainServer")?.GetComponent<InputField>();
        if(_usernameField != null && _passwordField != null && _serverAddress != null)
        {
            MenuEventSystem.current.Register.AddListener(SignUp);
        }
    }

    public void OnButtonClick()
    {
        MenuEventSystem.current.Register.Invoke();
    }

    public void SignedUp()
    {
        Debug.Log("Signed Up");
    }

    private void SignUp()
    {
        string username = _usernameField.text;
        string password = _passwordField.text;
        string[] address = _serverAddress.text.Split(':');
        VrLifeCore.API.Config.MainServer = new IPEndPoint(IPAddress.Parse(address[0]), int.Parse(address[1]));
        VrLifeCore.API.User.Register(username, password)
            .SetSucc(MenuEventSystem.current.SignedUp)
            .SetErr(_errorEvent)
            .Exec();
    }
}
