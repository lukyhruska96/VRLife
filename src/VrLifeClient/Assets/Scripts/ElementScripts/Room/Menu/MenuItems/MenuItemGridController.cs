using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class MenuItemGridController : MonoBehaviour,
    IPointerClickHandler, IEventSystemHandler
{
    public delegate void PointerClickEventHandler(Vector2 localPosition);
    public event PointerClickEventHandler PointerClicked;

    public delegate void EnableEventHandler();
    public event EnableEventHandler Enabled;

    public delegate void DisableEventHandler();
    public event DisableEventHandler Disabled;

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 localPoint;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), eventData.pressPosition, eventData.pressEventCamera, out localPoint))
        {
            PointerClicked(localPoint);
        }
        
    }

    private void OnEnable()
    {
        Enabled?.Invoke();
    }

    private void OnDisable()
    {
        Disabled?.Invoke();
    }
}
