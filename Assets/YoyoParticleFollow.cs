using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoyoParticleFollow : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		transform.position = Master.me.player.transform.position;
		
	}
}
