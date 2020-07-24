using Adrenak.UniMic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using VrLifeAPI.Client.Core.Character;
using VrLifeClient.API.DeviceAPI.MicrophoneDevice;

namespace Assets.Scripts.Core.Character
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundPlayer : MonoBehaviour, ISoundPlayer
    {

        private AudioSource _source;

        void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        private void Start()
        {
        }

        public void Play(AudioClip clip)
        {
            if(clip != null)
            {
                _source.PlayOneShot(clip);
            }
        }
    }
}
