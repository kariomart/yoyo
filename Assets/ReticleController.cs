using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		transform.position = (Vector2)Master.me.player.transform.position + Master.me.playerController.untouchedLDir.normalized;

	}
}
