using Assets.Scripts.Core.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.Wrappers
{
    public interface IAvatar
    {
        ulong GetUserId();
        SkeletonState GetCurrentSkeleton();

        void SetSkeleton(SkeletonState skeleton);

        GameObject GetGameObject();

        void SetControls(bool enabled);

        void Destroy();
    }
}
