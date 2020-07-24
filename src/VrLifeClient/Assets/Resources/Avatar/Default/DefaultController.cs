using Assets.Scripts.Core.Character;
using Assets.Scripts.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VrLifeAPI.Client.Core.Character;
using VrLifeAPI.Common.Core.Utils;
using VrLifeClient;
using VrLifeClient.Core.Services.RoomService;

namespace Assets.Prefab.Avatar.Default
{


    class DefaultController : MonoBehaviour
    {
        public GameObject[] SkeletonParts { get; private set; } = new GameObject[SkeletonState.PartCount];


        public void Awake()
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
        }

        public SkeletonState GetSkeleton()
        {
            return new SkeletonState
            {
                BodyLocation = SkeletonParts[(int)SkeletonEnum.BODY_LOCATION].transform.position.ToNumeric(),
                BodyRotation = SkeletonParts[(int)SkeletonEnum.BODY_LOCATION].transform.localEulerAngles.ToNumeric(),
                Head = SkeletonParts[(int)SkeletonEnum.HEAD].transform.localEulerAngles.ToNumeric(),
                Spine = SkeletonParts[(int)SkeletonEnum.SPINE].transform.localEulerAngles.ToNumeric(),
                Hips = SkeletonParts[(int)SkeletonEnum.HIPS].transform.localEulerAngles.ToNumeric(),
                Neck = SkeletonParts[(int)SkeletonEnum.NECK].transform.localEulerAngles.ToNumeric(),
                LeftShoulder = SkeletonParts[(int)SkeletonEnum.L_SHOULDER].transform.localEulerAngles.ToNumeric(),
                LeftArm = SkeletonParts[(int)SkeletonEnum.L_ARM].transform.localEulerAngles.ToNumeric(),
                LeftHand = SkeletonParts[(int)SkeletonEnum.L_HAND].transform.localEulerAngles.ToNumeric(),
                RightShoulder = SkeletonParts[(int)SkeletonEnum.R_SHOULDER].transform.localEulerAngles.ToNumeric(),
                RightArm = SkeletonParts[(int)SkeletonEnum.R_ARM].transform.localEulerAngles.ToNumeric(),
                RightHand = SkeletonParts[(int)SkeletonEnum.R_HAND].transform.localEulerAngles.ToNumeric(),
                LeftKnee = SkeletonParts[(int)SkeletonEnum.L_KNEE].transform.localEulerAngles.ToNumeric(),
                LeftFoot = SkeletonParts[(int)SkeletonEnum.L_FOOT].transform.localEulerAngles.ToNumeric(),
                RightKnee = SkeletonParts[(int)SkeletonEnum.R_KNEE].transform.localEulerAngles.ToNumeric(),
                RightFoot = SkeletonParts[(int)SkeletonEnum.R_FOOT].transform.localEulerAngles.ToNumeric()
            };
        }

        public void SetSkeleton(SkeletonState skeleton)
        {
            SkeletonParts[(int)SkeletonEnum.BODY_LOCATION].transform.position = skeleton.BodyLocation.ToUnity();
            SkeletonParts[(int)SkeletonEnum.BODY_LOCATION].transform.localEulerAngles = skeleton.BodyRotation.ToUnity();
            SkeletonParts[(int)SkeletonEnum.HEAD].transform.localEulerAngles = skeleton.Head.ToUnity();
            SkeletonParts[(int)SkeletonEnum.HIPS].transform.localEulerAngles = skeleton.Hips.ToUnity();
            SkeletonParts[(int)SkeletonEnum.SPINE].transform.localEulerAngles = skeleton.Spine.ToUnity();
            SkeletonParts[(int)SkeletonEnum.NECK].transform.localEulerAngles = skeleton.Neck.ToUnity();
            SkeletonParts[(int)SkeletonEnum.L_SHOULDER].transform.localEulerAngles = skeleton.LeftShoulder.ToUnity();
            SkeletonParts[(int)SkeletonEnum.L_ARM].transform.localEulerAngles = skeleton.LeftArm.ToUnity();
            SkeletonParts[(int)SkeletonEnum.L_HAND].transform.localEulerAngles = skeleton.LeftHand.ToUnity();
            SkeletonParts[(int)SkeletonEnum.R_SHOULDER].transform.localEulerAngles = skeleton.RightShoulder.ToUnity();
            SkeletonParts[(int)SkeletonEnum.R_ARM].transform.localEulerAngles = skeleton.RightArm.ToUnity();
            SkeletonParts[(int)SkeletonEnum.R_HAND].transform.localEulerAngles = skeleton.RightHand.ToUnity();
            SkeletonParts[(int)SkeletonEnum.L_KNEE].transform.localEulerAngles = skeleton.LeftKnee.ToUnity();
            SkeletonParts[(int)SkeletonEnum.L_FOOT].transform.localEulerAngles = skeleton.LeftFoot.ToUnity();
            SkeletonParts[(int)SkeletonEnum.R_KNEE].transform.localEulerAngles = skeleton.RightKnee.ToUnity();
            SkeletonParts[(int)SkeletonEnum.R_FOOT].transform.localEulerAngles = skeleton.RightFoot.ToUnity();
        }
    }
}
