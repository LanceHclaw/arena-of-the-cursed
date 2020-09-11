using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionControl : StateMachineBehaviour
{
    public string RMboolName;
    public bool status;

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(RMboolName, status);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(RMboolName, !status);
    }
}
