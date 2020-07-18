using Assets.Scripts.Core.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VrLifeShared.Networking.NetworkingModels;

namespace Assets.Scripts.Core.Wrappers
{
    public interface IAvatar
    {
        ulong GetUserId();
        SkeletonState GetCurrentSkeleton();

        void SetSkeleton(SkeletonState skeleton);

        UnityEngine.GameObject GetGameObject();

        void SetControls(bool enabled);

        UnityEngine.GameObject GetHead();

        SoundPlayer GetSoundPlayer();

        void Destroy();
    }
}
