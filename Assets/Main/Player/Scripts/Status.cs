using System;
using System.Collections.Generic;
using UnityEngine;

public class Status : Bolt.EntityBehaviour<IPlayerCharacterState>
{
    public static int maxHealth = 2000;
    public int localHealth = 2000;
    public float healthPercentage;

    public StatusEffects.Conditions currentConditions;
    public Dictionary<StatusEffects.Conditions, float> conditionTimers = new Dictionary<StatusEffects.Conditions, float>();

    Animator animator;
    public GameObject combatManager;

    public GameObject quicknessLF;
    public GameObject quicknessRF;

    public bool canAttack;

    public override void Attached()
    {
        state.Health = localHealth;

        state.AddCallback("Health", HealthCallback);

        if (entity.IsOwner)
        {
            animator = GetComponent<Animator>();
            combatManager = GameObject.FindGameObjectWithTag("CombatManager");
        }
    }

    private void HealthCallback()
    {
        localHealth = state.Health;
        
        /*if (localHealth <= 0)
        {
            BoltNetwork.Destroy(gameObject);
        }*/
    }

    public override void SimulateOwner()
    {
        healthPercentage = localHealth / (float)maxHealth;

        canAttack = animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded") &&
            (animator.GetCurrentAnimatorStateInfo(1).IsName("2Hand-Sword-Idle") || 
            animator.GetCurrentAnimatorStateInfo(1).IsName("Run") ||
            animator.GetCurrentAnimatorStateInfo(1).IsName("2Hand-Sword-Strafe-Backward")
            ) &&
            animator.GetCurrentAnimatorStateInfo(2).IsName("New State");

        List<StatusEffects.Conditions> condisToRemove = new List<StatusEffects.Conditions>();
        foreach (var key in conditionTimers.Keys)
        {
            if (conditionTimers[key] <= Time.time)
            {
                condisToRemove.Add(key);
            }
        }

        foreach (var key in condisToRemove)
        {
            RemoveCondition(key);
        }

        if (localHealth <= 0) animator.SetBool("Dead", true);

        if (IsConditionPresent(StatusEffects.Conditions.Stunned) && !animator.GetBool("stunned"))
        {
            //animator.Play("Stunned", 0, 0);
            animator.SetBool("stunned", true);
        }
        else if (!IsConditionPresent(StatusEffects.Conditions.Stunned))
        {
            animator.SetBool("stunned", false);
        }
    }
    
    public void ApplyCondition(StatusEffects.Conditions condition, float durationInSeconds)
    {
        if (!IsConditionPresent(condition)) 
        {
            currentConditions |= condition;
            conditionTimers.Add(condition, Time.time + durationInSeconds);

            if (condition == StatusEffects.Conditions.Crippled) GetComponent<Movement>().moveSpeed -= 2f;
            else if (condition == StatusEffects.Conditions.Quickened)
            {
                GetComponent<Movement>().moveSpeed += 2f;
                quicknessLF.SetActive(true);
                quicknessRF.SetActive(true);
            }
        }
        else conditionTimers[condition] += durationInSeconds;
    }

    public void RemoveCondition(StatusEffects.Conditions condition)
    {
        if (IsConditionPresent(condition))
        {
            currentConditions &= ~condition;
            conditionTimers.Remove(condition);

            if (condition == StatusEffects.Conditions.Crippled) GetComponent<Movement>().moveSpeed += 2f;
            else if (condition == StatusEffects.Conditions.Quickened)
            {
                GetComponent<Movement>().moveSpeed -= 2f;
                quicknessLF.SetActive(false);
                quicknessRF.SetActive(false);
            }
        }
    }

    public bool IsConditionPresent(StatusEffects.Conditions condition)
    {
        return (currentConditions & condition) != 0;
    }
}