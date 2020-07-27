using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILog : MonoBehaviour,
        IPointerClickHandler, IEventSystemHandler
{
    public static readonly int LOG_TIMEOUT = 5;
    private Text _header = null;
    private Text _text = null;
    private Image _bg = null;
    void Awake()
    {
        _bg = gameObject.GetComponent<Image>();
        _header = transform.Find("Header").gameObject.GetComponent<Text>();
        _text = transform.Find("Text").gameObject.GetComponent<Text>();
        StartCoroutine(AutoDestruction());
    }

    public IEnumerator AutoDestruction()
    {
        yield return new WaitForSeconds(LOG_TIMEOUT);
        GameObject.Destroy(gameObject);
    }

    public void SetLog(string header, string text, Color bgColor)
    {
        _bg.color = bgColor;
        _header.text = header;
        _text.text = text;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject.Destroy(gameObject);
    }
}
