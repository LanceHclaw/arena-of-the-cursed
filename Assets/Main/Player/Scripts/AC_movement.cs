using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AC_movement : Bolt.EntityBehaviour<IPlayerCharacterState>
{

    public Animator animator;
    CharacterController controller;
    Movement moveScript;

    Targeting  targetingScript;
    Transform enemyTarget;

    public bool enableRM;

    Vector3 currVector = new Vector3(0, 0, 0);

    // Use this for initialization
    public override void Attached()
    {
        controller = GetComponent<CharacterController>();
        moveScript = GetComponent<Movement>();
        animator = GetComponent<Animator>();
        state.SetAnimator(animator);
    }

    // Update is called once per frame
    public override void SimulateOwner()
    {
        enableRM = !animator.GetBool("canMove");
        animator.applyRootMotion = enableRM;
        if (enableRM) return;

        currVector = GetVector();
        currVector.y = 0;
        PlayAnim(currVector);

        animator.SetFloat("RunX", Input.GetAxis("Horizontal"));
        animator.SetFloat("RunZ", Input.GetAxis("Vertical"));
        
        /*
        animator.SetFloat("RunX", Input.GetAxis("Horizontal"));
        animator.SetFloat("RunZ", Input.GetAxis("Vertical"));
        */
    }

    void PlayAnim(Vector3 vector)
    {
        if (controller.isGrounded) { 
            if (vector == Vector3.zero)
            {
                animator.SetInteger("MoveState", 0);
            }
            else
            {
                animator.SetInteger("MoveState", 1);
            }
        }
    }

    Vector3 GetVector() { return moveScript.movement; }

    public void StopMotion()
    {
        animator.SetBool("canMove", false);
    }
}