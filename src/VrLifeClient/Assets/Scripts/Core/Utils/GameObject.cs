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
    }
}
