using Assets.Prefab.Avatar.Default;
using Assets.Scripts.Core.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DefaultController))]
public class Test : MonoBehaviour
{
    private DefaultController thisController;
    private DefaultAvatar avatar;
    private DefaultController controller;

    private void Awake()
    {
        avatar = new DefaultAvatar(0, "test", Vector3.zero, Quaternion.identity);
    }

    void Start()
    {
        thisController = gameObject.GetComponent<DefaultController>();
        controller = avatar.GetGameObject().GetComponent<DefaultController>();
    }

    // Update is called once per frame
    void Update()
    {
        SkeletonState skel = thisController.GetSkeleton();
        skel.BodyLocation = new System.Numerics.Vector3(skel.BodyLocation.X - 4, skel.BodyLocation.Y, skel.BodyLocation.Z);
        controller.SetSkeleton(skel);
    }
}
