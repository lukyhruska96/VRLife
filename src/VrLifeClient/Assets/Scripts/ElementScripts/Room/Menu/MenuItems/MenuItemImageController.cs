using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MenuItemImageController : MonoBehaviour
{
    private Coroutine _runningCoroutine = null;
    private Image _img;
    private Sprite[] _frames = null;
    private int _fps = 0;

    public void Start()
    {
        _img = GetComponent<Image>();
    }

    public void SetImage(Sprite img)
    {
        _frames = new Sprite[] { img };
        _fps = 0;
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        if (_img == null)
        {
            _img = GetComponent<Image>();
        }
        if (_runningCoroutine != null)
        {
            StopCoroutine(_runningCoroutine);
            _runningCoroutine = null;
        }
        _img.sprite = img;
    }

    public void SetGif(Sprite[] img, int fps)
    {
        _frames = img;
        _fps = fps;
        if(!gameObject.activeInHierarchy)
        {
            return;
        }
        if(_img == null)
        {
            _img = GetComponent<Image>();
        }
        if (_runningCoroutine != null)
        {
            StopCoroutine(_runningCoroutine);
        }
        _runningCoroutine = StartCoroutine(Animate(img, fps));
    }

    private IEnumerator Animate(Sprite[] img, int fps)
    {
        int i = 0;
        if(img.Length == 0)
        {
            yield break;
        }
        while(true)
        {
            _img.sprite = img[i];
            i = (i + 1) % img.Length;
            yield return new WaitForSeconds(1f / fps);
        }
    }

    private void OnEnable()
    {
        if(_frames == null)
        {
            return;
        }
        if(_frames.Length == 1)
        {
            SetImage(_frames[0]);
        }
        else
        {
            SetGif(_frames, _fps);
        }
    }

    private void OnDisable()
    {
        if(_runningCoroutine != null)
        {
            StopCoroutine(_runningCoroutine);
        }
    }

    private void OnDestroy()
    {
        if (_runningCoroutine != null)
        {
            StopCoroutine(_runningCoroutine);
        }
    }
}
