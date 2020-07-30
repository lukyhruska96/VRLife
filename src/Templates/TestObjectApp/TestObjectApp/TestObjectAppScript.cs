using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TestObjectApp
{
    class TestObjectAppScript : MonoBehaviour
    {
        void OnCollisionEnter(Collision collision)
        {
            Rigidbody rig = collision.gameObject.GetComponent<Rigidbody>();
            if(rig == null)
            {
                return;
            }
            Vector3 velocity = rig.velocity;
            velocity.y = 10;
            rig.velocity = velocity;
        }
    }
}
