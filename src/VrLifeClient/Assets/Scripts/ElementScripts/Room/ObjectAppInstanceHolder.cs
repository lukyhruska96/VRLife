using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrLifeAPI.Client.Applications.ObjectApp;

public class ObjectAppInstanceHolder : MonoBehaviour
{
    private IObjectAppInstance _instance = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetInstance(IObjectAppInstance app)
    {
        _instance = app;
    }

    public IObjectAppInstance GetInstance()
    {
        return _instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
