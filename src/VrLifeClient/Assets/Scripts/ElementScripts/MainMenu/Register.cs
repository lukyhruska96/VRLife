using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using VrLifeAPI.Client.Core.Services;
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
        ErrorEvent.AddListener(OnError);
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
        UILogger.current.Info("Signed Up.");
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

    private void OnError(Exception ex)
    {
        Debug.Log(ex);
        if (ex.GetType() == typeof(SocketException))
        {
            UILogger.current?.Error("Server is not available.");
        }
        else
        {
            UILogger.current?.Error(ex);
        }
    }
}
