using Assets.Scripts.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using VrLifeShared.Networking.NetworkingModels;

namespace Assets.Scripts.Core.Character
{
    enum SkeletonEnum
    {
        BODY_LOCATION,
        HEAD,
        NECK,
        SPINE,
        HIPS,
        L_SHOULDER,
        L_ARM,
        L_HAND,
        R_SHOULDER,
        R_ARM,
        R_HAND,
        L_KNEE,
        L_FOOT,
        R_KNEE,
        R_FOOT
    }

    public struct SkeletonState
    {
        public ulong UserId { get; set; }
        public Vector3 BodyLocation { get; set; }
        public Vector3 Head { get; set; }
        public Vector3 Neck { get; set; }
        public Vector3 Spine { get; set; }
        public Vector3 Hips { get; set; }
        public Vector3 LeftShoulder { get; set; }
        public Vector3 LeftArm { get; set; }
        public Vector3 LeftHand { get; set; }
        public Vector3 RightShoulder { get; set; }
        public Vector3 RightArm { get; set; }
        public Vector3 RightHand { get; set; }
        public Vector3 LeftKnee { get; set; }
        public Vector3 LeftFoot { get; set; }
        public Vector3 RightKnee { get; set; }
        public Vector3 RightFoot { get; set; }

        public static int PartCount = Enum.GetNames(typeof(SkeletonEnum)).Length;

        public SkeletonState(Skeleton skeleton)
        {
            UserId = skeleton.UserId;
            BodyLocation = skeleton.BodyLocation.ToVector();
            Head = skeleton.Head.ToVector();
            Neck = skeleton.Neck.ToVector();
            Spine = skeleton.Spine.ToVector();
            Hips = skeleton.Hips.ToVector();
            LeftShoulder = skeleton.LeftShoulder.ToVector();
            LeftArm = skeleton.LeftArm.ToVector();
            LeftHand = skeleton.LeftHand.ToVector();
            RightShoulder = skeleton.RightShoulder.ToVector();
            RightArm = skeleton.RightArm.ToVector();
            RightHand = skeleton.RightHand.ToVector();
            LeftKnee = skeleton.LeftKnee.ToVector();
            LeftFoot = skeleton.LeftFoot.ToVector();
            RightKnee = skeleton.RightKnee.ToVector();
            RightFoot = skeleton.RightFoot.ToVector();
        }

        public Skeleton ToNetworkModel()
        {
            Skeleton s = new Skeleton();
            s.UserId = UserId;
            s.BodyLocation = BodyLocation.ToCoord();
            s.Head = Head.ToCoord();
            s.Neck = Neck.ToCoord();
            s.Spine = Spine.ToCoord();
            s.Hips = Hips.ToCoord();
            s.LeftShoulder = LeftShoulder.ToCoord();
            s.LeftArm = LeftArm.ToCoord();
            s.LeftHand = LeftHand.ToCoord();
            s.RightShoulder = RightShoulder.ToCoord();
            s.RightArm = RightArm.ToCoord();
            s.RightHand = RightHand.ToCoord();
            s.LeftKnee = LeftKnee.ToCoord();
            s.LeftFoot = LeftFoot.ToCoord();
            s.RightKnee = RightKnee.ToCoord();
            s.RightFoot = RightFoot.ToCoord();
            return s;
        }

        public static SkeletonState Interpolate(SkeletonState from, SkeletonState to, float percent)
        {
            SkeletonState s = new SkeletonState();
            s.UserId = from.UserId;
            s.BodyLocation = Vector3.Lerp(from.BodyLocation, to.BodyLocation, percent);
            s.Head = Vector3.Lerp(from.Head, to.Head, percent);
            s.Neck = Vector3.Lerp(from.Neck, to.Neck, percent);
            s.Spine = Vector3.Lerp(from.Spine, to.Spine, percent);
            s.Hips = Vector3.Lerp(from.Hips, to.Hips, percent);
            s.LeftShoulder = Vector3.Lerp(from.LeftShoulder, to.LeftShoulder, percent);
            s.LeftArm = Vector3.Lerp(from.LeftArm, to.LeftArm, percent);
            s.LeftHand = Vector3.Lerp(from.LeftHand, to.LeftHand, percent);
            s.RightShoulder = Vector3.Lerp(from.RightShoulder, to.RightShoulder, percent);
            s.RightArm = Vector3.Lerp(from.RightArm, to.RightArm, percent);
            s.RightHand = Vector3.Lerp(from.RightHand, to.RightHand, percent);
            s.LeftKnee = Vector3.Lerp(from.LeftKnee, to.LeftKnee, percent);
            s.LeftFoot = Vector3.Lerp(from.LeftFoot, to.LeftFoot, percent);
            s.RightKnee = Vector3.Lerp(from.RightKnee, to.RightKnee, percent);
            s.RightFoot = Vector3.Lerp(from.RightFoot, to.RightFoot, percent);
            return s;
        }
    }
}
