using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyController : MonoBehaviour {

    public float health;
    public Status status;
    Animator animator;

	// Use this for initialization
	void Start () {
        health = 2000f;
        animator = GetComponent<Animator>();
        status = GetComponent<Status>();
	}
	
	// Update is called once per frame
	void Update () {
        health = status.localHealth;
        if (health <= 0) animator.SetBool("Dead", true);
	}
}
