using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttacksManager : MonoBehaviour {

    public float autoAtkRange = 3f;
    public float attackAngle = 75f;

	public void ChainAttack (GameObject target, int numAttack)
    {
        int damage = 0;

        switch (numAttack){
            case 0: damage = Random.Range(100, 140); break;
            case 1: damage = Random.Range(110, 150); break;
            case 2: damage = Random.Range(120, 160); break;
        }

        Vector3 targetRange = target.transform.position - transform.position;
        float angle = Vector3.Angle(targetRange, transform.forward);

        if (Vector3.Distance(transform.position, target.transform.position) <= autoAtkRange && angle <= attackAngle)
        {
            target.GetComponent<ReceiveDamage>().Damage(damage);
        } 
    }
}
