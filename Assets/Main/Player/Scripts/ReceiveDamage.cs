using UnityEngine;

public class ReceiveDamage : Bolt.EntityBehaviour<IPlayerCharacterState> {

    Animator animator;

    public override void Attached()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    public void Damage(int damage)
    {
        if (!animator.GetBool("Dead") && !animator.GetBool("dodging"))
        {
            state.Health -= damage;
        }
    }
}
