using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Register : MonoBehaviour
{
    public void OnButtonClick()
    {
        MenuEventSystem.current.InvokeEvent("register");
    }
}
