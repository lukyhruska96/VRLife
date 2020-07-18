using Adrenak.UniMic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VrLifeClient.API.DeviceAPI.MicrophoneDevice
{
    class MicrophoneListener : MonoBehaviour
    {
        public static MicrophoneListener current = null;
        private string _deviceName = null;
        private AudioClip _clip;
        private bool _muted = false;

        public delegate void MicrophoneDataEventHandler(ulong sampleNum, float[] data);
        public event MicrophoneDataEventHandler MicrophoneData;
        private void Awake()
        {
            current = this;
            if (Microphone.devices.Length != 0)
            {
                _deviceName = Microphone.devices[0];
            }
        }

        private void Start()
        {
            InitMic();
        }

        private void InitMic()
        {
            Mic.Instance.StartRecording(16000, 100);
            Mic.Instance.OnSampleReady += OnMicrophoneData;
        }

        public void SetMute(bool state)
        {
            _muted = state;
        }

        public void SetMic(int idx)
        {
            Mic.Instance.ChangeDevice(idx);
        }

        private void StopMic()
        {
            Mic.Instance.StopRecording();
        }

        private void OnDestroy()
        {

        }

        private void OnMicrophoneData(ulong id, float[] data)
        {
            if(!_muted)
            {
                MicrophoneData?.Invoke(id, data);
            }
        }
    }
}
