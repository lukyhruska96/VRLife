using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace VrLifeServer.Core.Services.TickRateService
{
    struct SkeletonState
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
    }
}
