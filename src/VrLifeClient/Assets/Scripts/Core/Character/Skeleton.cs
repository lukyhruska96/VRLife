using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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

    public struct Skeleton
    {
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
    }
}
