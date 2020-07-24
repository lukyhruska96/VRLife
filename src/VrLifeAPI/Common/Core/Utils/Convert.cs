using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Common.Core.Utils
{
    public static class VectorConversions
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

        public static Coord Add(this Coord a, Coord b)
        {
            return new Coord { X = a.X + b.X, Y = a.Y + b.Y, Z = a.Z + b.Z };
        }

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
