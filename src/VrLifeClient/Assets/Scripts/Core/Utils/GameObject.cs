using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.Utils
{
    static class GameObjectFunc
    {
        public static GameObject GetChildGameObject(this GameObject fromGameObject, string withName)
        {
            //Author: Isaac Dart, June-13.
            Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
            foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
            return null;
        }

        public static Vector3 ClampAngles(this Vector3 angle, Vector3 minAngle, Vector3 maxAngle)
        {
            Vector3 absoluteRotation = GeneralizeAngles(angle);
            absoluteRotation.x = Mathf.Clamp(absoluteRotation.x, minAngle.x, maxAngle.x);
            absoluteRotation.y = Mathf.Clamp(absoluteRotation.y, minAngle.y, maxAngle.y);
            absoluteRotation.z = Mathf.Clamp(absoluteRotation.z, minAngle.z, maxAngle.z);
            return absoluteRotation;
        }

        /// <summary>
        /// Transform eulerRotation vector into range <-180, 180>
        /// </summary>
        /// <param name=""></param>
        public static Vector3 GeneralizeAngles(this Vector3 eulerVector)
        {
            return new Vector3(
                GeneralizeAngle(eulerVector.x),
                GeneralizeAngle(eulerVector.y),
                GeneralizeAngle(eulerVector.z)
                );
        }

        /// <summary>
        /// Transform angle into range <-180, 180>
        /// </summary>
        /// <param name=""></param>
        public static float GeneralizeAngle(float angle)
        {
            float val = angle % 360;
            return val > 180f ? val - 360f : val;
        }
    }
}
