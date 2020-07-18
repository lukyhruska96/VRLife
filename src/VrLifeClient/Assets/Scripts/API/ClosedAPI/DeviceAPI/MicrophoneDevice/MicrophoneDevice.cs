using Adrenak.UniMic;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.AudioClip;

namespace VrLifeClient.API.DeviceAPI.MicrophoneDevice
{
    internal class MicrophoneDevice : IDisposable
    {
        private static string PREFAB_PATH = "MicrophoneDevice/MicrophoneListener";
        private GameObject _golistener;
        private MicrophoneListener _listener;

        public int Frequency { get => Mic.Instance.Frequency; }
        public int SampleLength { get => Mic.Instance.SampleLength; }
        public int SampleDurationMS { get => Mic.Instance.SampleDurationMS; }

        public event MicrophoneListener.MicrophoneDataEventHandler MicrophoneData
        {
            add { _listener.MicrophoneData += value; }
            remove { _listener.MicrophoneData -= value; }
        }

        public MicrophoneDevice()
        {
            GameObject obj = Resources.Load<GameObject>(PREFAB_PATH);
            if(obj == null)
            {
                throw new MicrophoneDeviceException("Could not find MicrophoneDevice's prefab.");
            }
            _golistener = GameObject.Instantiate(obj);
            _golistener.name = obj.name;
            _listener = _golistener.GetComponent<MicrophoneListener>();
        }

        public void SetMute(bool state)
        {
            _listener.SetMute(state);
        }

        public string[] GetDevices()
        {
            return Microphone.devices;
        }

        public void SetMic(int idx)
        {
            if (idx < 0 || idx >= Microphone.devices.Length)
            {
                throw new MicrophoneDeviceException("Device index is out of range.");
            }
            _listener.SetMic(idx);
        }

        public void Dispose()
        {
            GameObject.Destroy(_golistener);
        }
    }
}
