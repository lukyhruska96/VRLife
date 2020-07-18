using Assets.Scripts.API.HUDAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NotificationController : MonoBehaviour,
    IPointerClickHandler, IEventSystemHandler
{
    public static readonly int NOTIFICATION_TIMEOUT = 5;
    private Text _header = null;
    private Text _text = null;
    private Notification _notification;
    void Awake()
    {
        _header = transform.Find("Header").gameObject.GetComponent<Text>();
        _text = transform.Find("Text").gameObject.GetComponent<Text>();
        StartCoroutine(AutoDestruction());
    }

    public IEnumerator AutoDestruction()
    {
        yield return new WaitForSeconds(5);
        GameObject.Destroy(gameObject);
    }

    public void SetNotification(Notification notification)
    {
        _notification = notification;
        _header.text = notification.Header;
        _text.text = notification.Text;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MenuControl.current?.HandleNotification(_notification);
        GameObject.Destroy(gameObject);
    }
}
