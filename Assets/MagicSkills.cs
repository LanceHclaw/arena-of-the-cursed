using System;
using System.Collections.Generic;
using UnityEngine;

public class MagicSkills : Bolt.EntityBehaviour<IPlayerCharacterState>
{
    Animator animator;
    CharacterController controller;

    Dictionary<MagicSkillNames, float> skillCooldowns = new Dictionary<MagicSkillNames, float>();

    public float coneOfColdCooldown = 15f;
    public float fireballCooldown = 20f;
    public float WithstandCooldown = 12f;
    public float rocketBootsCooldown = 14f;

    [SerializeField]
    public GameObject spellOrigin;

    Status selfStatus;
    public const int withstandHealing = 1500;

    public const float rocketBootsDuration = 4f;

    MagicSkillsManager manager;

    [SerializeField]
    public GameObject fireballPrefab;
    public float fireballVelocity = 100f;

    [SerializeField]
    public GameObject coneOfColdParticles;

    // Start is called before the first frame update
    public override void Attached()
    {
        skillCooldowns.Add(MagicSkillNames.ConeOfCold, Time.time);
        skillCooldowns.Add(MagicSkillNames.Fireball, Time.time);
        skillCooldowns.Add(MagicSkillNames.RocketBoots, Time.time);
        skillCooldowns.Add(MagicSkillNames.Withstand, Time.time);

        selfStatus = GetComponent<Status>();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        manager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<MagicSkillsManager>();
    }

    // Update is called once per frame
    public override void SimulateOwner()
    {
        if (selfStatus.canAttack)
        {
            if (Input.GetKeyDown(KeyCode.Q) && skillCooldowns[MagicSkillNames.ConeOfCold] <= Time.time)
            {
                UseMagicSkill1();
            }
            else if (Input.GetKeyDown(KeyCode.E) && skillCooldowns[MagicSkillNames.Fireball] <= Time.time)
            {
                UseMagicSkill2();
            }
            else if (Input.GetKeyDown(KeyCode.R) && skillCooldowns[MagicSkillNames.Withstand] <= Time.time)
            {
                UseMagicSkill3();
            }
        }

        //this skill has no activation time, so it can be used at any moment
        if (Input.GetKeyDown(KeyCode.LeftShift) && skillCooldowns[MagicSkillNames.RocketBoots] <= Time.time)
        {
            UseMagicSkill4();
        }
    }

    public float GetCooldownPercentage(MagicSkillNames name)
    {
        float setCD = skillCooldowns[name];
        switch (name)
        {
            case MagicSkillNames.ConeOfCold:
                {
                    if (setCD < (Time.time - coneOfColdCooldown)) return 0;
                    else return (setCD - Time.time) / coneOfColdCooldown;
                }
            case MagicSkillNames.Fireball:
                {
                    if (setCD < (Time.time - fireballCooldown)) return 0;
                    else return (setCD - Time.time) / fireballCooldown;
                }
            case MagicSkillNames.RocketBoots:
                {
                    if (setCD < (Time.time - rocketBootsCooldown)) return 0;
                    else return (setCD - Time.time) / rocketBootsCooldown;
                }
            case MagicSkillNames.Withstand:
                {
                    if (setCD < (Time.time - WithstandCooldown)) return 0;
                    else return (setCD - Time.time) / WithstandCooldown;
                }
        }
        return 0;
    }

    void UseMagicSkill1()
    {
        skillCooldowns[MagicSkillNames.ConeOfCold] = Time.time + coneOfColdCooldown;
        animator.SetBool("attacking", true);
        animator.SetTrigger("ConeOfCold");
        //state.Animator.Play("ConeOfCold", 1, 0);
    }

    void UseMagicSkill2()
    {
        skillCooldowns[MagicSkillNames.Fireball] = Time.time + fireballCooldown;
        animator.SetBool("attacking", true);
        animator.SetTrigger("Fireball");
        //state.Animator.Play("Fireball", 1, 0);
    }
    void UseMagicSkill3()
    {
        skillCooldowns[MagicSkillNames.Withstand] = Time.time + WithstandCooldown;
        animator.SetBool("attacking", true);
        animator.SetTrigger("Withstand");
        //state.Animator.Play("Withstand", 1, 0);
    }

    void UseMagicSkill4()
    {
        skillCooldowns[MagicSkillNames.RocketBoots] = rocketBootsCooldown + Time.time;
        selfStatus.ApplyCondition(StatusEffects.Conditions.Quickened, rocketBootsDuration);
    }

    public void CastFireball()
    {
        GameObject fireballClone;
        fireballClone = Instantiate(fireballPrefab, spellOrigin.transform.position, Quaternion.identity);
        fireballClone.GetComponent<FireballScript>().enemy = gameObject.GetComponent<Targeting>().Enemy;
        fireballClone.GetComponent<FireballScript>().caster = gameObject;
    }

    public void CastConeOfCold()
    {
        //initiate the particles and call the manager for damage and conditions
        coneOfColdParticles.SetActive(true);
        manager.ConeOfCold(spellOrigin, gameObject.GetComponent<Targeting>().Enemy);
    }

    public void DeactivateConeOfCold()
    {
        coneOfColdParticles.SetActive(false);
    }

    public void CastWithstand()
    {
        //add playing the particle effects somewhere
        selfStatus.localHealth = Math.Min(selfStatus.localHealth + withstandHealing, Status.maxHealth);
        selfStatus.RemoveCondition(StatusEffects.Conditions.Crippled);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(gameObject.transform.position, gameObject.transform.forward * 10);
    }
}

public enum MagicSkillNames
{
    ConeOfCold = 0,
    Fireball = 1,
    RocketBoots = 2,
    Withstand = 3
}
