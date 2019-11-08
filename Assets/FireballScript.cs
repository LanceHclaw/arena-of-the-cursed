using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    public GameObject caster;
    public GameObject enemy;

    public float speed = 10f;

    public GameObject explosionPrefab;

    public GameObject combatManager;

    private void Start()
    {
        combatManager = GameObject.FindGameObjectWithTag("CombatManager");
    }

    private void Update()
    {
        Vector3 targetPosition = enemy.transform.position;
        this.transform.LookAt(targetPosition);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        if(otherCollider.gameObject == caster || otherCollider.gameObject.tag == "Weapon")
        {
            Physics.IgnoreCollision(otherCollider.gameObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
        }
        else
        {
            var instanceExplosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            combatManager.GetComponent<MagicSkillsManager>().Fireball(gameObject, enemy);
            Destroy(gameObject);
            Destroy(instanceExplosion, 2f);
        } 
    }
}
