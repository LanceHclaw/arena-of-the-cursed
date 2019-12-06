using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : Bolt.EntityBehaviour<IPlayerCharacterState>
{
    public RectTransform[] buttonFillers;

    public List<WeaponSkillCD> weaponSkillFillers = new List<WeaponSkillCD>();
    public List<MagicSkillCD> magicSkillFillers = new List<MagicSkillCD>();

    public WeaponSkills weaponSkills;
    public MagicSkills magicSkills;
    public Status status;
    public Movement movement;

    public RectTransform healthFiller;
    public RectTransform enduranceFiller;
    public TextMeshProUGUI healthDisplay;

    // Start is called before the first frame update
    public override void Attached()
    {
        if (entity.IsOwner)
        {
            healthDisplay = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            movement = gameObject.GetComponentInParent<Movement>();
            status = gameObject.GetComponentInParent<Status>();
            weaponSkills = gameObject.GetComponentInParent<WeaponSkills>();
            magicSkills = gameObject.GetComponentInParent<MagicSkills>();

            SetupArrays();

            foreach (WeaponSkillCD filler in weaponSkillFillers)
            {
                filler.filler.localScale = new Vector3(1f, 0f, 1f);
            }

            foreach (MagicSkillCD filler in magicSkillFillers)
            {
                filler.filler.localScale = new Vector3(1f, 0f, 1f);
            }
        } 
    }

    // Update is called once per frame
    public override void SimulateOwner()
    {
        foreach (WeaponSkillCD filler in weaponSkillFillers)
        {
            float percentage = weaponSkills.GetCooldownPercentage(filler.name);
            filler.filler.localScale = new Vector3(1f, percentage, 1f);
        }

        foreach (MagicSkillCD filler in magicSkillFillers)
        {
            float percentage = magicSkills.GetCooldownPercentage(filler.name);
            filler.filler.localScale = new Vector3(1f, percentage, 1f);
        }

        healthDisplay.text = status.localHealth.ToString();
        enduranceFiller.localScale = new Vector3(movement.endurancePercentage, 1f, 1f);
        healthFiller.localScale = new Vector3(1f, status.healthPercentage, 1f);
    }

    private void SetupArrays()
    {
        weaponSkillFillers.Add(new WeaponSkillCD { filler = buttonFillers[0], name = WeaponSkillNames.SweepingAttack });
        //Debug.Log("Added weaponSkill");
        weaponSkillFillers.Add(new WeaponSkillCD { filler = buttonFillers[1], name = WeaponSkillNames.StunningAttack });
        //Debug.Log("Added weaponSkill");
        weaponSkillFillers.Add(new WeaponSkillCD { filler = buttonFillers[2], name = WeaponSkillNames.CripplingAttack });
        //Debug.Log("Added weaponSkill");

        magicSkillFillers.Add(new MagicSkillCD { filler = buttonFillers[3], name = MagicSkillNames.RocketBoots });
        //Debug.Log("Added magicSkill");
        magicSkillFillers.Add(new MagicSkillCD { filler = buttonFillers[4], name = MagicSkillNames.ConeOfCold });
        //Debug.Log("Added magicSkill");
        magicSkillFillers.Add(new MagicSkillCD { filler = buttonFillers[5], name = MagicSkillNames.Fireball });
        //Debug.Log("Added magicSkill");
        magicSkillFillers.Add(new MagicSkillCD { filler = buttonFillers[6], name = MagicSkillNames.Withstand });
        //Debug.Log("Added magicSkill");
    }
}
public struct WeaponSkillCD
{
    public WeaponSkillNames name;
    public RectTransform filler;
}

public struct MagicSkillCD
{
    public MagicSkillNames name;
    public RectTransform filler;
}
