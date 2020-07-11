using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using VrLifeServer.Core.Utils;
using VrLifeShared.Networking.NetworkingModels;

namespace VrLifeServer.Core.Services.TickRateService
{
    struct SkeletonState
    {
        public ulong UserId { get; set; }
        public Vector3 BodyLocation { get; set; }
        public Vector3 BodyRotation { get; set; }
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

        public SkeletonState(Skeleton skeleton)
        {
            UserId = skeleton.UserId;
            BodyLocation = skeleton.BodyLocation.ToVector();
            BodyRotation = skeleton.BodyRotation.ToVector();
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
            s.BodyRotation = BodyRotation.ToCoord();
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

        public static SkeletonState operator-(SkeletonState a, SkeletonState b)
        {
            SkeletonState val = new SkeletonState();
            val.UserId = a.UserId;
            val.BodyLocation = a.BodyLocation - b.BodyLocation;
            val.BodyRotation = a.BodyRotation - b.BodyRotation;
            val.Head = a.Head - b.Head;
            val.Neck = a.Neck - b.Neck;
            val.Spine = a.Spine - b.Spine;
            val.Hips = a.Hips - b.Hips;
            val.LeftShoulder = a.LeftShoulder - b.LeftShoulder;
            val.LeftArm = a.LeftArm - b.LeftArm;
            val.LeftHand = a.LeftHand - b.LeftHand;
            val.RightShoulder = a.RightShoulder - b.RightShoulder;
            val.RightArm = a.RightArm - b.RightArm;
            val.RightHand = a.RightHand - b.RightHand;
            val.LeftKnee = a.LeftKnee - b.LeftKnee;
            val.LeftFoot = a.LeftFoot - b.LeftFoot;
            val.RightKnee = a.RightKnee - b.RightKnee;
            val.RightFoot = a.RightFoot - b.RightFoot;
            return val;
        }
    }
}
