using UnityEngine;

public class MagicSkillsManager : MonoBehaviour
{
    public float coneOfColdRange = 10f;
    public float coneOfColdAngle = 20f;

    public int coneOfColdDamage;
    public float coneCrippleDuration = 1.5f;

    public float fireballRadius = 10f;
    public int fireballDamage;


    public void ConeOfCold(GameObject source, GameObject target)
    {
        if (CheckIfHits(source.transform, target.transform, coneOfColdRange, checkAngle: true, spellAngle: coneOfColdAngle))
        {
            coneOfColdDamage = Random.Range(130, 150);
            target.GetComponent<ReceiveDamage>().Damage(coneOfColdDamage);
            target.GetComponent<Status>().ApplyCondition(StatusEffects.Conditions.Crippled, coneCrippleDuration);
        }
    }

    public void Fireball(GameObject source, GameObject target)
    {
        if (CheckIfHits(source.transform, target.transform, fireballRadius, false))
        {
            fireballDamage = Random.Range(420, 560);
            target.GetComponent<ReceiveDamage>().Damage(fireballDamage);
        }
    }

    bool CheckIfHits(Transform spellsource, Transform target, float range, bool checkAngle, float? spellAngle = 360)
    {
        bool withinRange = Vector3.Distance(spellsource.position, target.position) <= range;
        bool angleHit = true;

        if (checkAngle)
        {
            Vector3 targetRange = target.position - spellsource.position;
            float angle = Vector3.Angle(targetRange, spellsource.forward);
            angleHit = angle <= spellAngle;
        }
        
        return withinRange && angleHit;
    }
}
