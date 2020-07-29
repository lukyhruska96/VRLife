using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Common.Core.Utils
{
    /// <summary>
    /// Extension třída pro převod datových typů
    /// </summary>
    public static class VectorConversions
    {
        /// <summary>
        /// Převod Unity vektoru na System.Numerics vektor.
        /// </summary>
        /// <param name="obj">Vektor k převodu.</param>
        /// <returns>Převedný vektor.</returns>
        public static System.Numerics.Vector3 ToNumeric(this UnityEngine.Vector3 obj)
        {
            return new System.Numerics.Vector3(obj.x, obj.y, obj.z);
        }

        /// <summary>
        /// Převod System.Numerics vektoru na Unity vektor.
        /// </summary>
        /// <param name="obj">Vektor k převodu.</param>
        /// <returns>Převedný vektor.</returns>
        public static UnityEngine.Vector3 ToUnity(this System.Numerics.Vector3 obj)
        {
            return new UnityEngine.Vector3(obj.X, obj.Y, obj.Z);
        }

        /// <summary>
        /// Převod IPv4 adresy na int.
        /// </summary>
        /// <param name="addr">Adresa k převodu.</param>
        /// <returns>Převedená adresa.</returns>
        public static int ToInt(this IPAddress addr)
        {
            byte[] bytes = addr.GetAddressBytes();

            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Převod System.Numerics vektoru na síťovací vektor k odeslání.
        /// </summary>
        /// <param name="vec">Vektor k převodu.</param>
        /// <returns>Výsledná hodnota.</returns>
        public static Coord ToCoord(this Vector3 vec)
        {
            Coord c = new Coord();
            c.X = vec.X;
            c.Y = vec.Y;
            c.Z = vec.Z;
            return c;
        }

        /// <summary>
        /// Převod síťového vektoru na System.Numerics vektor.
        /// </summary>
        /// <param name="coord">Hodnota k převodu.</param>
        /// <returns>Převedený vektor.</returns>
        public static Vector3 ToVector(this Coord coord)
        {
            Vector3 vec = new Vector3();
            vec.X = coord.X;
            vec.Y = coord.Y;
            vec.Z = coord.Z;
            return vec;
        }

        /// <summary>
        /// Převod uint na binární bool array.
        /// </summary>
        /// <param name="val">Hodnota k převodu.</param>
        /// <returns>Převedená hodnota.</returns>
        public static bool[] ToBinary(this uint val)
        {
            bool[] arr = new bool[32];
            for(int i = 0; i < 32; ++i)
            {
                arr[i] = ((val >> 31 - i) & 0x01) == 0x01;
            }
            return arr;
        }

        /// <summary>
        /// Výpočet výsledného snapshotu pomocí minulého a delta k novému.
        /// </summary>
        /// <param name="obj">Minulý snapshot.</param>
        /// <param name="diff">Přijatý delta snapshot.</param>
        /// <returns>Nový snapshot.</returns>
        public static SnapshotData AddDiff(this SnapshotData obj, SnapshotData diff)
        {
            Dictionary<ulong, Skeleton> tmpDict = obj.Skeletons.ToDictionary(x => x.UserId, x => x);
            SnapshotData retVal = new SnapshotData();
            retVal.TickNum = diff.TickNum;
            retVal.Skeletons.AddRange(
                diff.Skeletons
                .Select(x => tmpDict.TryGetValue(x.UserId, out Skeleton val) ? x.Add(val) : x)
                );
            retVal.Objects.AddRange(diff.Objects);
            return retVal;
        }

        /// <summary>
        /// Součet Coord vektoru.
        /// </summary>
        /// <param name="a">a</param>
        /// <param name="b">b</param>
        /// <returns>a+b</returns>
        public static Coord Add(this Coord a, Coord b)
        {
            return new Coord { X = a.X + b.X, Y = a.Y + b.Y, Z = a.Z + b.Z };
        }

        /// <summary>
        /// Součet stavu kostry postavy s deltou kostry.
        /// </summary>
        /// <param name="a">Kostra.</param>
        /// <param name="b">Delta kostry.</param>
        /// <returns>Výsledná kostra.</returns>
        public static Skeleton Add(this Skeleton a, Skeleton b)
        {
            Skeleton s = new Skeleton();
            s.UserId = a.UserId;
            s.BodyLocation = a.BodyLocation.Add(b.BodyLocation);
            s.BodyRotation = a.BodyRotation.Add(b.BodyRotation);
            s.Head = a.Head.Add(b.Head);
            s.Neck = a.Neck.Add(b.Neck);
            s.Spine = a.Spine.Add(b.Spine);
            s.Hips = a.Hips.Add(b.Hips);
            s.LeftShoulder = a.LeftShoulder.Add(b.LeftShoulder);
            s.LeftArm = a.LeftArm.Add(b.LeftArm);
            s.LeftHand = a.LeftHand.Add(b.LeftHand);
            s.RightShoulder = a.RightShoulder.Add(b.RightShoulder);
            s.RightArm = a.RightArm.Add(b.RightArm);
            s.RightHand = a.RightHand.Add(b.RightHand);
            s.LeftKnee = a.LeftKnee.Add(b.LeftKnee);
            s.LeftFoot = a.LeftFoot.Add(b.LeftFoot);
            s.RightKnee = a.RightKnee.Add(b.RightKnee);
            s.RightFoot = a.RightFoot.Add(b.RightFoot);
            return s;
        }
    }
}
