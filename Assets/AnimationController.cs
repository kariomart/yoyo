using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

	Animator anim;
	PlayerMovement player;

	void Start () {

		anim = GetComponent<Animator>();
		player = GetComponentInParent<PlayerMovement>();
		

	}
	
	// Update is called once per frame
	void Update () {

		anim.SetFloat("Movement", player.vel.x);
		
	}
}
