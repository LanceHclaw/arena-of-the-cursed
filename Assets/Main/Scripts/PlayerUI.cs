using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : Bolt.EntityBehaviour<IPlayerCharacterState>
{
    public RectTransform[] buttonFillers;

    public List<WeaponSkillCD> weaponSkillFillers = new List<WeaponSkillCD>();
    public List<MagicSkillCD> magicSkillFillers = new List<MagicSkillCD>();

    [Header("Don't Assign")]
    public WeaponSkills weaponSkills;
    public MagicSkills magicSkills;
    public Status status;
    public Movement movement;
    public GameObject target;

    [Header("Assign")]
    public GameObject escapeMenu;
    public GameObject escapeMenuContinue;
    public GameObject escapeMenuLoser;
    public GameObject escapeMenuWinner;
    public RectTransform healthFiller;
    public RectTransform enemyHealthFiller;
    public RectTransform enduranceFiller;
    public TextMeshProUGUI healthDisplay;
    public TextMeshProUGUI enemyHealthDisplay;

    public GameObject healthUIelement;
    public GameObject enemyHealthUIelement;

    private float enemyHealthPercentage;

    // Start is called before the first frame update
    public override void Attached()
    {
        if (entity.IsOwner)
        {
            healthDisplay = healthUIelement.GetComponentInChildren<TextMeshProUGUI>();
            enemyHealthDisplay = enemyHealthUIelement.GetComponentInChildren<TextMeshProUGUI>();
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

        target = gameObject.GetComponentInParent<Targeting>().Enemy;
        enemyHealthDisplay.text = target.GetComponent<Status>().localHealth.ToString();
        enemyHealthPercentage = target.GetComponent<Status>().localHealth / 2000f;
        enemyHealthFiller.localScale = new Vector3(enemyHealthPercentage, 1f, 1f);

        #region escapeMenu
        if (Input.GetKeyDown(KeyCode.Escape) && (target.GetComponent<Status>().localHealth > 0 || (status.localHealth > 0)))
        {
            if (escapeMenu.activeSelf == false)
                escapeMenu.SetActive(true);
            else escapeMenu.SetActive(false);
        }
        if (status.localHealth <= 0)
        {
            escapeMenu.SetActive(true);
            escapeMenuContinue.SetActive(false);
            escapeMenuLoser.SetActive(true);
        }
        if (target.GetComponent<Status>().localHealth <= 0)
        {
            escapeMenu.SetActive(true);
            escapeMenuContinue.SetActive(false);
            escapeMenuWinner.SetActive(true);
        }
        #endregion
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

    public void Continue()
    {
        escapeMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
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
