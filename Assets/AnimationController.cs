using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

	Animator anim;
	SpriteRenderer spr;
	PlayerMovement player;

	void Start () {

		anim = GetComponent<Animator>();
		spr = GetComponent<SpriteRenderer>();
		player = GetComponentInParent<PlayerMovement>();
		

	}
	
	// Update is called once per frame
	void Update () {

		anim.SetFloat("Speed", Mathf.Abs(player.vel.x));
		anim.SetBool("Grounded", player.grounded);

		if (player.face == -1) {
			spr.flipX = true;
		} else {
			spr.flipX = false;
		}
		
	}
}
