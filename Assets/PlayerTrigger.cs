using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D coll) {

		if (coll.gameObject.name == "soundTrigger") {
			//Debug.Log("NEW CLIP = " SoundController.me.getSoundtrack().name);
			SoundController.me.soundtrackController.CrossFade(SoundController.me.getSoundtrack(), .5f, 10f, 0);
		}
	}
}
