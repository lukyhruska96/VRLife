using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VrLifeShared.Networking.NetworkingModels;

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
            //if (BitConverter.IsLittleEndian)
            //{
            //    Array.Reverse(bytes);
            //}

            return BitConverter.ToUInt32(bytes, 0);
        }

        public static Coord ToCoord(this Vector3 vec)
        {
            Coord c = new Coord();
            c.X = vec.X;
            c.Y = vec.Y;
            c.Z = vec.Z;
            return c;
        }

        public static Vector3 ToVector(this Coord coord)
        {
            Vector3 vec = new Vector3();
            vec.X = coord.X;
            vec.Y = coord.Y;
            vec.Z = coord.Z;
            return vec;
        }

        public static bool[] ToBinary(this uint val)
        {
            bool[] arr = new bool[32];
            for(int i = 0; i < 32; ++i)
            {
                arr[i] = ((val >> 31 - i) & 0x01) == 0x01;
            }
            return arr;
        }
    }
}
