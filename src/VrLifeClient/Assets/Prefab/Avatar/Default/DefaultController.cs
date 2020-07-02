using Assets.Scripts.Core.Character;
using Assets.Scripts.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Prefab.Avatar.Default
{


    class DefaultController : MonoBehaviour
    {
        private GameObject[] SkeletonParts = new GameObject[Skeleton.PartCount];
        private static float hipsHeight;

        public void Start()
        {
            SkeletonParts[(int)SkeletonEnum.BODY_LOCATION] = gameObject;
            SkeletonParts[(int)SkeletonEnum.HEAD] = gameObject.GetChildGameObject("Head");
            SkeletonParts[(int)SkeletonEnum.HIPS] = gameObject.GetChildGameObject("Root");
            SkeletonParts[(int)SkeletonEnum.NECK] = gameObject.GetChildGameObject("Neck");
            SkeletonParts[(int)SkeletonEnum.SPINE] = gameObject.GetChildGameObject("Ribs");
            SkeletonParts[(int)SkeletonEnum.L_SHOULDER] = gameObject.GetChildGameObject("Left_Shoulder_Joint_01");
            SkeletonParts[(int)SkeletonEnum.L_ARM] = gameObject.GetChildGameObject("Left_Upper_Arm_Joint_01");
            SkeletonParts[(int)SkeletonEnum.L_HAND] = gameObject.GetChildGameObject("Left_Forearm_Joint_01");
            SkeletonParts[(int)SkeletonEnum.R_SHOULDER] = gameObject.GetChildGameObject("Right_Shoulder_Joint_01");
            SkeletonParts[(int)SkeletonEnum.R_ARM] = gameObject.GetChildGameObject("Right_Upper_Arm_Joint_01");
            SkeletonParts[(int)SkeletonEnum.R_HAND] = gameObject.GetChildGameObject("Right_Forearm_Joint_01");
            SkeletonParts[(int)SkeletonEnum.L_KNEE] = gameObject.GetChildGameObject("Left_Thigh_Joint_01");
            SkeletonParts[(int)SkeletonEnum.L_FOOT] = gameObject.GetChildGameObject("Left_Knee_Joint_01");
            SkeletonParts[(int)SkeletonEnum.R_KNEE] = gameObject.GetChildGameObject("Right_Thigh_Joint_01");
            SkeletonParts[(int)SkeletonEnum.R_FOOT] = gameObject.GetChildGameObject("Right_Knee_Joint_01");
            hipsHeight = SkeletonParts[(int)SkeletonEnum.HIPS].transform.localPosition.y;
        }

        public Skeleton GetSkeleton()
        {
            return new Skeleton
            {
                BodyLocation = SkeletonParts[(int)SkeletonEnum.HIPS].transform.position.ToNumeric() - new System.Numerics.Vector3(0, hipsHeight, 0),
                Head = SkeletonParts[(int)SkeletonEnum.HEAD].transform.rotation.eulerAngles.ToNumeric(),
                Spine = SkeletonParts[(int)SkeletonEnum.SPINE].transform.eulerAngles.ToNumeric(),
                Hips = SkeletonParts[(int)SkeletonEnum.HIPS].transform.eulerAngles.ToNumeric(),
                Neck = SkeletonParts[(int)SkeletonEnum.NECK].transform.rotation.eulerAngles.ToNumeric(),
                LeftShoulder = SkeletonParts[(int)SkeletonEnum.L_SHOULDER].transform.rotation.eulerAngles.ToNumeric(),
                LeftArm = SkeletonParts[(int)SkeletonEnum.L_ARM].transform.rotation.eulerAngles.ToNumeric(),
                LeftHand = SkeletonParts[(int)SkeletonEnum.L_HAND].transform.rotation.eulerAngles.ToNumeric(),
                RightShoulder = SkeletonParts[(int)SkeletonEnum.R_SHOULDER].transform.rotation.eulerAngles.ToNumeric(),
                RightArm = SkeletonParts[(int)SkeletonEnum.R_ARM].transform.rotation.eulerAngles.ToNumeric(),
                RightHand = SkeletonParts[(int)SkeletonEnum.R_HAND].transform.rotation.eulerAngles.ToNumeric(),
                LeftKnee = SkeletonParts[(int)SkeletonEnum.L_KNEE].transform.rotation.eulerAngles.ToNumeric(),
                LeftFoot = SkeletonParts[(int)SkeletonEnum.L_FOOT].transform.rotation.eulerAngles.ToNumeric(),
                RightKnee = SkeletonParts[(int)SkeletonEnum.R_KNEE].transform.rotation.eulerAngles.ToNumeric(),
                RightFoot = SkeletonParts[(int)SkeletonEnum.R_FOOT].transform.rotation.eulerAngles.ToNumeric()
            };
        }

        public void SetSkeleton(Skeleton skeleton)
        {
            SkeletonParts[(int)SkeletonEnum.BODY_LOCATION].transform.position = skeleton.BodyLocation.ToUnity();
            SkeletonParts[(int)SkeletonEnum.HEAD].transform.eulerAngles = skeleton.Head.ToUnity();
            SkeletonParts[(int)SkeletonEnum.HIPS].transform.eulerAngles = skeleton.Hips.ToUnity();
            SkeletonParts[(int)SkeletonEnum.SPINE].transform.eulerAngles = skeleton.Spine.ToUnity();
            SkeletonParts[(int)SkeletonEnum.NECK].transform.eulerAngles = skeleton.Neck.ToUnity();
            SkeletonParts[(int)SkeletonEnum.L_SHOULDER].transform.eulerAngles = skeleton.LeftShoulder.ToUnity();
            SkeletonParts[(int)SkeletonEnum.L_ARM].transform.eulerAngles = skeleton.LeftArm.ToUnity();
            SkeletonParts[(int)SkeletonEnum.L_HAND].transform.eulerAngles = skeleton.LeftHand.ToUnity();
            SkeletonParts[(int)SkeletonEnum.R_SHOULDER].transform.eulerAngles = skeleton.RightShoulder.ToUnity();
            SkeletonParts[(int)SkeletonEnum.R_ARM].transform.eulerAngles = skeleton.RightArm.ToUnity();
            SkeletonParts[(int)SkeletonEnum.R_HAND].transform.eulerAngles = skeleton.RightHand.ToUnity();
            SkeletonParts[(int)SkeletonEnum.L_KNEE].transform.eulerAngles = skeleton.LeftKnee.ToUnity();
            SkeletonParts[(int)SkeletonEnum.L_FOOT].transform.eulerAngles = skeleton.LeftFoot.ToUnity();
            SkeletonParts[(int)SkeletonEnum.R_KNEE].transform.eulerAngles = skeleton.RightKnee.ToUnity();
            SkeletonParts[(int)SkeletonEnum.R_FOOT].transform.eulerAngles = skeleton.RightFoot.ToUnity();
        }
    }
}
