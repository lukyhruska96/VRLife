using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class MenuItemInputController : MonoBehaviour, 
    ISubmitHandler
{
    public delegate void SubmitEventHandler();
    public event SubmitEventHandler onSubmit;

    private InputField _inputField;

    void Awake()
    {
        _inputField = GetComponent<InputField>();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        onSubmit?.Invoke();
    }
}
