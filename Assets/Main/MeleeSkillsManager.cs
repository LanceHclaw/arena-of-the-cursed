using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSkillsManager : MonoBehaviour
{

    public float autoAtkRange = 3f;
    public float autoAttackAngle = 75f;
    public float stunningAttackAngle = 30f;
    public float stunDuration = 2f;
    public float crippleDuration = 1.5f;

    public void ChainAttack(GameObject source, GameObject target, int numAttack)
    {
        int damage = 0;

        switch (numAttack)
        {
            case 0: damage = Random.Range(100, 140); break;
            case 1: damage = Random.Range(110, 150); break;
            case 2: damage = Random.Range(120, 160); break;
        }

        if (CheckIfHits(source, target, autoAttackAngle, autoAtkRange))
        {
            target.GetComponent<ReceiveDamage>().Damage(damage);
        }
    }

    public void SweepingAttack(GameObject source, GameObject target, int numAttack)
    {
        int damage = 0;
        float sweepingAttackAngle = 0f;

        switch (numAttack)
        {
            case 1: damage = Random.Range(150, 200); sweepingAttackAngle = 120f; break;
            case 2: damage = Random.Range(300, 400); sweepingAttackAngle = 30f; break;
        }

        if (CheckIfHits(source, target, sweepingAttackAngle, autoAtkRange))
        {
            target.GetComponent<ReceiveDamage>().Damage(damage);
        }
    }

    public void StunningAttack(GameObject source, GameObject target)
    {
        int damage = Random.Range(100, 120);

        if (CheckIfHits(source, target, stunningAttackAngle, autoAtkRange))
        {
            target.GetComponent<ReceiveDamage>().Damage(damage);
            target.GetComponent<Status>().ApplyCondition(StatusEffects.Conditions.Stunned, stunDuration);
        }
    }

    public void CripplingAttack(GameObject source, GameObject target, int numAttack)
    {
        int damage;
        float cripplingAttackAngle;

        switch (numAttack)
        {
            case 1:
                {
                    damage = Random.Range(130, 200);
                    cripplingAttackAngle = 100f;

                    if (CheckIfHits(source, target, cripplingAttackAngle, autoAtkRange))
                    {
                        target.GetComponent<ReceiveDamage>().Damage(damage);
                        target.GetComponent<Status>().ApplyCondition(StatusEffects.Conditions.Crippled, crippleDuration);
                    }
                    break;
                }
            case 2:
                {
                    damage = Random.Range(130, 200);
                    cripplingAttackAngle = 90f;

                    if (CheckIfHits(source, target, cripplingAttackAngle, autoAtkRange))
                    {
                        target.GetComponent<ReceiveDamage>().Damage(damage);
                        target.GetComponent<Status>().ApplyCondition(StatusEffects.Conditions.Crippled, crippleDuration + 1.5f);
                    }
                    break;
                }
            case 3:
                {
                    damage = Random.Range(250, 350);
                    cripplingAttackAngle = 30f;

                    if (CheckIfHits(source, target, cripplingAttackAngle, autoAtkRange))
                    {
                        target.GetComponent<ReceiveDamage>().Damage(damage);
                        target.GetComponent<Status>().ApplyCondition(StatusEffects.Conditions.Crippled, crippleDuration + 3.5f);
                    }
                    break;
                }
        }
    }

    public bool CheckIfHits(GameObject source, GameObject target, float attackAngle, float attackRange)
    {
        Vector3 targetRange = target.transform.position - source.transform.position;
        float angle = Vector3.Angle(targetRange, source.transform.forward);

        return Vector3.Distance(source.transform.position, target.transform.position) <= attackRange && angle <= attackAngle;
    }
}
