﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathNet : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll) {

		if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) {

			Master.me.playerController.GoToCheckPoint ();


		}


	}
}
