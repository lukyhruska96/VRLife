﻿using System.Numerics;
using VrLifeAPI.Common.Core.Utils;
using VrLifeAPI.Networking.NetworkingModels;

namespace VrLifeAPI.Common.Core.Services.TickRateService
{
    /// <summary>
    /// Struktura popisující lokální rotace individuálních kloubů
    /// kostry postavy.
    /// </summary>
    public struct SkeletonState
    {

        /// <summary>
        /// ID uživatele používajícího danou postavu.
        /// </summary>
        public ulong UserId { get; set; }

        /// <summary>
        /// Globální pozice postavy.
        /// </summary>
        public Vector3 BodyLocation { get; set; }

        /// <summary>
        /// Globální rotace celé postavy.
        /// </summary>
        public Vector3 BodyRotation { get; set; }

        /// <summary>
        /// Lokální rotace hlavy.
        /// </summary>
        public Vector3 Head { get; set; }

        /// <summary>
        /// Lokální rotace krku.
        /// </summary>
        public Vector3 Neck { get; set; }

        /// <summary>
        /// Lokální rotace středu páteře.
        /// </summary>
        public Vector3 Spine { get; set; }

        /// <summary>
        /// Lokální rotace středu pánve.
        /// </summary>
        public Vector3 Hips { get; set; }

        /// <summary>
        /// Lokální rotace levého ramene.
        /// </summary>
        public Vector3 LeftShoulder { get; set; }

        /// <summary>
        /// Lokální rotace v levém loktu.
        /// </summary>
        public Vector3 LeftArm { get; set; }

        /// <summary>
        /// Lokální rotace v levém zápěstí.
        /// </summary>
        public Vector3 LeftHand { get; set; }

        /// <summary>
        /// Lokální rotace v pravém rameni.
        /// </summary>
        public Vector3 RightShoulder { get; set; }

        /// <summary>
        /// Lokální rotace v pravém loktu.
        /// </summary>
        public Vector3 RightArm { get; set; }

        /// <summary>
        /// Lokální rotace v pravém zápěstí.
        /// </summary>
        public Vector3 RightHand { get; set; }

        /// <summary>
        /// Lokální rotace v levém koleni.
        /// </summary>
        public Vector3 LeftKnee { get; set; }

        /// <summary>
        /// Lokální rotace v levém kotníku.
        /// </summary>
        public Vector3 LeftFoot { get; set; }

        /// <summary>
        /// Lokální rotace v pravém koleni.
        /// </summary>
        public Vector3 RightKnee { get; set; }

        /// <summary>
        /// Lokální rotace v pravém kotníku.
        /// </summary>
        public Vector3 RightFoot { get; set; }

        /// <summary>
        /// Konstruktor pomocí síťového objektu.
        /// </summary>
        /// <param name="skeleton">Síťový objekt kostry.</param>
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

        /// <summary>
        /// Převod na síťový objekt.
        /// </summary>
        /// <returns>Instance síťového objektu kostry k odeslání.</returns>
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

        /// <summary>
        /// Operace odčítání úhlů mezi danými klouby dvou koster k výpočtu delta kostry.
        /// </summary>
        /// <param name="a">Původní kostra.</param>
        /// <param name="b">Nová kostra.</param>
        /// <returns>Delta dvou koster.</returns>
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
