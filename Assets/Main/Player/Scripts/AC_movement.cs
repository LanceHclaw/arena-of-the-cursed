using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AC_movement : MonoBehaviour
{

    Animator animator;
    CharacterController controller;
    Movement moveScript;

    Targeting  targetingScript;
    Transform enemyTarget;

    public bool enableRM;

    Vector3 currVector = new Vector3(0, 0, 0);

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        moveScript = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        enableRM = !animator.GetBool("canMove");
        animator.applyRootMotion = enableRM;
        if (enableRM) return;

        currVector = GetVector();
        currVector.y = 0;
        PlayAnim(currVector);

        animator.SetFloat("RunX", Input.GetAxis("Horizontal"));
        animator.SetFloat("RunZ", Input.GetAxis("Vertical"));
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

    void StopMotion()
    {
        animator.SetBool("canMove", false);
    }
}