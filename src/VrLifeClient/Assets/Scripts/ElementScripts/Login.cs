using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login : MonoBehaviour
{
    public void OnButtonClick()
    {
        MenuEventSystem.current.InvokeEvent("login");
    }
}
