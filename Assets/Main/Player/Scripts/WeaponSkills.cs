using System.Collections.Generic;
using UnityEngine;

public class WeaponSkills : Bolt.EntityBehaviour<IPlayerCharacterState> {

    Animator animator;
    CharacterController controller;
    Status status;

    Dictionary<WeaponSkillNames, float> skillCooldowns = new Dictionary<WeaponSkillNames, float>();
    public float sweepingAttackCooldown = 7f;
    public float stunningAttackCooldown = 12f;
    public float cripplingAttackCooldown = 15f;

    [Header("UI EscapeMenu")]
    public GameObject escapeMenu;

    MeleeSkillsManager manager;
    

	public override void Attached () {
        skillCooldowns.Add(WeaponSkillNames.SweepingAttack, Time.time);
        skillCooldowns.Add(WeaponSkillNames.StunningAttack, Time.time);
        skillCooldowns.Add(WeaponSkillNames.CripplingAttack, Time.time);

        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        status = GetComponent<Status>();

        state.SetAnimator(animator);
        manager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<MeleeSkillsManager>();
    }
	
	// Update is called once per frame
	public override void SimulateOwner () {
        if (controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                UseSkill1();
            }
            else if (status.canAttack)
            {
                if (Input.GetKeyDown(KeyCode.Alpha2) && skillCooldowns[WeaponSkillNames.SweepingAttack] <= Time.time)
                {
                    UseSkill2();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3) && skillCooldowns[WeaponSkillNames.StunningAttack] <= Time.time)
                {
                    UseSkill3();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4) && skillCooldowns[WeaponSkillNames.CripplingAttack] <= Time.time)
                {
                    UseSkill4();
                }
            }
        }
	}

    public float GetCooldownPercentage(WeaponSkillNames name)
    {
        float setCD = skillCooldowns[name];
        switch (name){
            case WeaponSkillNames.SweepingAttack: 
                {
                    if (setCD < (Time.time - sweepingAttackCooldown)) return 0;
                    else return (setCD - Time.time) / sweepingAttackCooldown;
                }
            case WeaponSkillNames.CripplingAttack:
                {
                    if (setCD < (Time.time - cripplingAttackCooldown)) return 0;
                    else return (setCD - Time.time) / cripplingAttackCooldown;
                }
            case WeaponSkillNames.StunningAttack:
                {
                    if (setCD < (Time.time - stunningAttackCooldown)) return 0;
                    else return (setCD - Time.time) / stunningAttackCooldown;
                }
        }
        return 0;
    }

    void UseSkill1()
    {
        animator.SetInteger("autoChain", (GetAutoattackNumber(1) + 1) % 3);
        if (state.autoChain == 0 && status.canAttack)
        {
            animator.SetTrigger("StartChainAttack");
            //state.Animator.Play("Attack6", 1, 0);
        }
    }

    int GetAutoattackNumber(int layer)
    {
        if (animator.GetCurrentAnimatorStateInfo(layer).IsName("Attack6"))
        {
            return 0;
        }
        else if (animator.GetCurrentAnimatorStateInfo(layer).IsName("Attack4"))
        {
            return 1;
        }
        else if (animator.GetCurrentAnimatorStateInfo(layer).IsName("Attack2"))
        {
            return 2;
        }
        else return -1;
    }

    void UseSkill2()
    {
        skillCooldowns[WeaponSkillNames.SweepingAttack] = Time.time + sweepingAttackCooldown;
        animator.SetBool("attacking", true);
        animator.SetTrigger("SweepingAttack");
        //animator.SetInteger("UsingSkill", 2);
        //state.Animator.Play("SweepingAttack", 2, 0);
    }

    void UseSkill3()
    {
        skillCooldowns[WeaponSkillNames.StunningAttack] = Time.time + stunningAttackCooldown;
        animator.SetBool("attacking", true);
        animator.SetTrigger("StunningAttack");
        //animator.SetInteger("UsingSkill", 3);
        //state.Animator.Play("LungeAttack", 2, 0);
    }

    void UseSkill4()
    {
        skillCooldowns[WeaponSkillNames.CripplingAttack] = Time.time + cripplingAttackCooldown;
        animator.SetBool("attacking", true);
        animator.SetTrigger("CripplingAttack");
        //animator.SetInteger("UsingSkill", 4);
        //state.Animator.Play("CripplingAttack", 2, 0);
    }

    //Methods below just call combatManager, which executes results
    public void CallSkill1(int comboNum)
    {
        manager.ChainAttack(gameObject, GetComponent<Targeting>().Enemy, comboNum);
    }
    public void CallSweepingAttack(int attackNum)
    {
        manager.SweepingAttack(gameObject, GetComponent<Targeting>().Enemy, attackNum);
    }
    public void CallStunningAttack()
    {
        manager.StunningAttack(gameObject, GetComponent<Targeting>().Enemy);
    }
    public void CallCripplingAttack(int attackNum)
    {
        manager.CripplingAttack(gameObject, GetComponent<Targeting>().Enemy, attackNum);
    }

    public void ResetCurrentSkill()
    {
        animator.SetInteger("UsingSkill", 0);
    }
}

public enum WeaponSkillNames
{
    SweepingAttack = 0,
    StunningAttack = 1,
    CripplingAttack = 2
}
