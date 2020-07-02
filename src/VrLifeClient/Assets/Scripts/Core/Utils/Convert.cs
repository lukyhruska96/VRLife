using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Utils
{
    static class VectorConversions
    {
        public static System.Numerics.Vector3 ToNumeric(this UnityEngine.Vector3 obj)
        {
            return new System.Numerics.Vector3(obj.x, obj.y, obj.z);
        }

        public static UnityEngine.Vector3 ToUnity(this System.Numerics.Vector3 obj)
        {
            return new UnityEngine.Vector3(obj.X, obj.Y, obj.Z);
        }
        public static uint ToInt(this IPAddress addr)
        {
            byte[] bytes = addr.GetAddressBytes();

            // flip big-endian(network order) to little-endian
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return BitConverter.ToUInt32(bytes, 0);
        }
    }
}
