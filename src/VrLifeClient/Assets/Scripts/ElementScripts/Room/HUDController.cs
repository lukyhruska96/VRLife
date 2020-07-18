using Assets.Scripts.API.HUDAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public static HUDController current = null;
    private const string PREFAB_PATH = "HUDItems/Notification";
    private GameObject _notificationPrefab = null;
    private GameObject _notificationsField;
    void Awake()
    {
        current = this;
        _notificationsField = gameObject.transform.Find("Notifications").gameObject;
    }

    private void Start()
    {
        _notificationPrefab = Resources.Load<GameObject>(PREFAB_PATH);
    }

    public void ShowNotification(Notification notification)
    {
        if(_notificationPrefab == null)
        {
            return;
        }
        GameObject instance = GameObject.Instantiate(_notificationPrefab);
        instance.transform.SetParent(_notificationsField.transform);
        instance.transform.localScale = Vector3.one;
        instance.transform.localRotation = Quaternion.identity;
        Vector3 position = instance.transform.localPosition;
        position.z = 0;
        instance.transform.localPosition = position;
        instance.transform.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        instance.GetComponent<NotificationController>().SetNotification(notification);
    }

    private void OnDestroy()
    {
        current = null;
    }
}
