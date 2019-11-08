using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyController : MonoBehaviour {

    public float health;
    Animator animator;

	// Use this for initialization
	void Start () {
        health = 2000f;
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (health <= 0) animator.SetBool("Dead", true);
        //Debug.Log(health);
	}
}
