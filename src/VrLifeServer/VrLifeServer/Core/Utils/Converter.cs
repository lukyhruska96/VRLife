using System;
using System.Collections.Generic;
using System.Net;
using System.Numerics;
using System.Text;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Utils
{
    public static class Converter
    {
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
    }
}
