using Assets.Scripts.ElementScripts.Room;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrLifeAPI;
using VrLifeAPI.Client.API;
using VrLifeAPI.Client.Applications.ObjectApp;
using VrLifeAPI.Common.Core.Utils;
using VrLifeClient;

public class ObjectAPIController : MonoBehaviour
{
    private AppInfo _info = new AppInfo(ulong.MaxValue, "ObjectAPIController", null,
           new AppVersion(new int[] { 1, 0, 0 }), AppType.APP_GLOBAL);
    private IClosedAPI _api;

    public int speed;
    public static ObjectAPIController current = null;
    private bool _placing = false;
    private float _width = 0;
    private float _height = 0;
    private IObjectApp _app = null;
    private Vector3 _planePosition = Vector3.zero;

    public event Action<Vector3> Placed;
    private GameObject _placeObject;
    private MeshCollider _plane;

    private const string PREFAB_PATH = "Room/AppField";
    private GameObject _prefab = null;

    private bool _start = false;

    private void Awake()
    {
        current = this;
        _prefab = Resources.Load<GameObject>(PREFAB_PATH);
    }

    public void StartPlacing(IObjectApp app)
    {
        MenuControl.current.OnExitMenu();
        _app = app;
        ObjectAppInfo info = app.GetObjectAppInfo();
        _placing = true;
        _width = info.Width;
        _height = info.Height;
        Vector3 scale = _placeObject.transform.localScale;
        scale.x = info.Width;
        scale.z = info.Height;
        _placeObject.transform.localScale = scale;
        _start = true;
        _placeObject.SetActive(true);
        PlayerControls.current.Selected += OnSelect;
    }

    public void StopPlacing()
    {
        _placing = false;
        _placeObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        PlayerControls.current.PointAt(null);
        _placeObject.SetActive(false);
        PlayerControls.current.Selected -= OnSelect;
    }

    private void Start()
    {
        if(VrLifeCore.API != null)
        {
            _api = VrLifeCore.GetClosedAPI(_info);
            _api.Services.App.AddedNewObjectAppInstance += OnObjectAppInstance;
        }
        _placeObject = transform.Find("PlaceApp").gameObject;
        _plane = GameObject.Find("/Room/Plane").GetComponent<MeshCollider>();
    }

    private void OnDestroy()
    {
        _api.Services.App.AddedNewObjectAppInstance -= OnObjectAppInstance;
    }

    private void FixedUpdate()
    {
        if (!_placing)
        {
            return;
        }
        Vector3 planePosition = GetPlanePosition(PlayerControls.current.LookingAt());
        PlayerControls.current.PointAt(planePosition);
        if (_start)
        {
            Vector3 pos = _placeObject.transform.position;
            pos.x = planePosition.x;
            pos.z = planePosition.z;
            _placeObject.transform.position = pos;
            _start = false;
            return;
        }
        _placeObject.GetComponent<Rigidbody>().velocity = (planePosition - _placeObject.transform.position) / Time.deltaTime;
    }

    private void OnSelect(Vector3 headVector)
    {
        StopPlacing();
        Vector3 pos = _placeObject.transform.position;
        pos.y = 0f;
        _api.Services.App.CreateObjectAppInstance(_app, pos.ToNumeric());
        Placed?.Invoke(GetPlanePosition(headVector));
    }

    private Vector3 GetPlanePosition(Vector3 headVector)
    {
        GameObject head = _api.GlobalAPI.Players.GetMainAvatar().GetHead();
        if(head == null)
        {
            return _planePosition;
        }
        Ray ray = new Ray(head.transform.position, headVector);
        if(_plane.Raycast(ray, out var enter, 1000f))
        {
            _planePosition = enter.point;
        }
        return _planePosition;
    }

    private void OnObjectAppInstance(IObjectAppInstance instance)
    {
        ObjectAppInfo info = instance.GetObjectAppInfo();
        Vector3 center = instance.GetCenter();
        GameObject appPlace = GameObject.Instantiate(_prefab);
        center.y = 0.01f;
        appPlace.transform.position = center;
        Vector3 scale = appPlace.transform.localScale;
        scale.x = info.Width;
        scale.z = info.Height;
        appPlace.transform.localScale = scale;
        instance.GetGameObject().transform.SetParent(appPlace.transform);
        instance.FixGameObject();
    }
}
