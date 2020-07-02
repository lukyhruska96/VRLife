using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleChanging : StateMachineBehaviour
{
    private float nextIdleTime;

    public float nextIdleMin = 5f;

    public float nextIdleMax = 30f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        nextIdleTime = Time.time + Random.Range(nextIdleMin, nextIdleMax);
        animator.SetInteger("IdleNum", 0);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(nextIdleTime < Time.time)
        {
            nextIdleTime = Time.time + Random.Range(nextIdleMin, nextIdleMax);
            animator.SetInteger("IdleNum", Random.Range(1, 3));
        }
    }
}
