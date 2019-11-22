using UnityEngine;

public class ReceiveDamage : MonoBehaviour {

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Damage(int damage)
    {
        if (!animator.GetBool("Dead") && !animator.GetBool("dodging"))
        {
            GetComponent<Status>().localHealth -= damage;
            animator.Play("GetHit", 0, 0);
        }
    }
}
