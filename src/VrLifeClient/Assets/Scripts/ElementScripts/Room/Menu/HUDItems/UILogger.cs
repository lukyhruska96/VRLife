using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using VrLifeAPI.Common.Core.Applications;

public class UILogger : MonoBehaviour, VrLifeAPI.Common.Logging.Logging.ILogger
{
    public static UILogger current;
    private const string PREFAB_PATH = "HUDItems/Log";
    private GameObject _logPrefab = null;
    private bool _debug = false;
    private ConcurrentQueue<(string, string, Color)> _logs = 
        new ConcurrentQueue<(string, string, Color)>();
    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        _logPrefab = Resources.Load<GameObject>(PREFAB_PATH);
    }

    public void Debug(string msg)
    {
        if(_debug)
        {
            CreateLog("Debug", msg, Color.cyan);
        }
    }

    public void Debug(Exception ex)
    {
        if (_debug)
        {
            CreateLog("Debug Exception", $"{ex.GetType().FullName}: {ex.Message}\n{ex.StackTrace}", Color.cyan);
        }
    }

    public void Dispose()
    {

    }

    public void Error(string msg)
    {
        CreateLog("Error", msg, Color.red);
    }

    public void Error(Exception ex)
    {
        CreateLog("Error Exception", $"{ex.GetType().FullName}: {ex.Message}\n{ex.StackTrace}", Color.red);
    }

    public void Info(string msg)
    {
        CreateLog("Info", msg, Color.white);
    }

    public void SetDebug(bool status)
    {
        _debug = status;
    }

    public void Warn(string msg)
    {
        CreateLog("Warn", msg, Color.yellow);
    }

    private void CreateLog(string header, string text, Color bgColor)
    {
        _logs.Enqueue((header, text, bgColor));
    }

    public void Update()
    {
        while(_logs.TryDequeue(out var item))
        {
            GameObject instance = GameObject.Instantiate(_logPrefab);
            instance.transform.SetParent(gameObject.transform);
            instance.transform.localScale = Vector3.one;
            instance.transform.localRotation = Quaternion.identity;
            Vector3 position = instance.transform.localPosition;
            position.z = 0;
            instance.transform.localPosition = position;
            instance.transform.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            instance.GetComponent<UILog>().SetLog(item.Item1, item.Item2, item.Item3);
        }
    }
}
