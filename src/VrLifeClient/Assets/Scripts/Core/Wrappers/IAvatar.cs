using Assets.Scripts.Core.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.Wrappers
{
    interface IAvatar
    {
        Skeleton GetCurrentSkeleton();

        void SetSkeleton(Skeleton skeleton);

        GameObject GetGameObject();
    }
}
