using System;
using UnityEngine;
using VrLifeAPI.Client.Applications.ObjectApp;
using VrLifeAPI.Client.Core.Wrappers;

namespace VrLifeAPI.Client.API.ObjectAPI
{
    public delegate void PlayerPointAtEventHandler(ulong userId, Ray ray);
    public delegate void PlayerSelectEventHandler(ulong userId, Ray ray);

    /// <summary>
    /// API pro objektové aplikace.
    /// </summary>
    public interface IObjectAPI
    {
    }
}
