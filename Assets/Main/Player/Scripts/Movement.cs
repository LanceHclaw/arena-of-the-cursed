using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Movement : Bolt.EntityBehaviour<IPlayerCharacterState>
{

    [SerializeField] private Transform target;

    Animator animator;
    Status status;

    public float moveSpeed = 4.0f;
    public float dodgeSpeed = 4.5f;
    private CharacterController controller;

    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;

    public float maxEndurance = 100f;
    public float endurance = 100f;
    public float endurancePercentage;
    public float enduranceRecoveryRate;

    private float vertSpeed;
    public Vector3 movement;
    public Vector3 prevMovement;

    public override void Attached()
    {
        state.SetTransforms(state.PlayerCharacterTransform, gameObject.transform);
        state.SetAnimator(GetComponent<Animator>());
        state.Animator.applyRootMotion = entity.IsOwner;

        if (entity.IsOwner)
        {
            vertSpeed = minFall;
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            status = GetComponent<Status>();
        }
    }

    private void FixedUpdate()
    {
        endurance = Mathf.Clamp(endurance + enduranceRecoveryRate, 0, maxEndurance);
        Debug.Log(endurance);
    }

    public override void SimulateOwner()
    {
        endurancePercentage = endurance / maxEndurance;
        //if switches to AddForce, need to remove dodge and jump functions from Update if already in the process (otw will turn into a space rocket)
        if (animator.GetBool("dodging"))
        {
            Vector3 dodgeMovement;
            dodgeMovement = prevMovement;
            dodgeMovement.y = 0;
            controller.Move(dodgeMovement);
        }
        else if (animator.GetBool("canMove"))
        {
            Move();
            Jump();
            Dodge();

            movement *= BoltNetwork.FrameDeltaTime;

            if (animator.GetBool("Back") && !animator.GetBool("dodging"))
            {
                movement.x *= 0.65f;
                movement.z *= 0.45f;
            }

            controller.Move(movement);
            prevMovement = movement;
        }
    }
    
    void Jump()
    {
        //try using AddForce for jumping - may work better with dodging
        if (controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.Play("2Hand-Sword-Jump", 0, 0);
                vertSpeed = jumpSpeed;
            }
            else
            {
                vertSpeed = minFall;

                if (animator.GetBool("inAir"))
                {
                    animator.SetBool("inAir", false);
                }
            }
        }
        else
        {
            animator.SetBool("inAir", true);
            vertSpeed += gravity * 5 * BoltNetwork.FrameDeltaTime;
            if (vertSpeed < terminalVelocity)
            {
                vertSpeed = terminalVelocity;
            }
        }
        movement.y = vertSpeed;
    }

    void Move()
    {
        Vector3 cameraRot = Camera.main.transform.forward;
        cameraRot.y = 0;
        movement = Vector3.zero;
        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        if (horInput != 0 || vertInput != 0)
        {

            movement.x = horInput * moveSpeed;
            movement.z = vertInput * moveSpeed;
            movement = Vector3.ClampMagnitude(movement, moveSpeed);

            Quaternion tmp = target.rotation;
            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);

            movement = target.TransformDirection(movement);
            target.rotation = tmp;

            if (vertInput < 0)
            {
                animator.SetBool("Back", true);
            }
            else
            { 
                animator.SetBool("Back", false);
            }
            transform.rotation = Quaternion.LookRotation(cameraRot);
        }
    }

    void Dodge()
    {
        //try using AddForce instead of speed changes - may be more consistent and remove jump+dodge bug
        if (Input.GetKeyDown(KeyCode.V) && !animator.GetBool("inAir"))
        {
            if (endurance >= 50)
            {
                endurance -= 50f;
                animator.SetBool("dodging", true);

                if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
                {
                    movement.z = (-1) * moveSpeed;
                    movement = Vector3.ClampMagnitude(movement, dodgeSpeed);

                    Quaternion tmp = target.rotation;
                    target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);

                    movement = target.TransformDirection(movement);
                    target.rotation = tmp;

                    transform.rotation = Quaternion.LookRotation(-movement);
                    animator.SetBool("Back", true);
                }
            }
            else Debug.Log("Not enough endurance");
            
        }
    }

    void StopDodge()
    {
        animator.SetBool("dodging", false);
    }
}
