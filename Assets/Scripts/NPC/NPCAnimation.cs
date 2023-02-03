using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimation : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    public void setAnimation(BehaviorStates currentState)
    {
        switch (currentState)
        {
            case BehaviorStates.Idle:
                skeletonAnimation.AnimationName = "Idle_01";
                break;

            case BehaviorStates.Blinking:
                skeletonAnimation.AnimationName = "Idle_02_blink";
                break;

            case BehaviorStates.Walking:
                skeletonAnimation.AnimationName = "Running";
                break;
        }
    }
}
