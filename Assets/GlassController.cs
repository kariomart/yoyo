using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassController : MonoBehaviour {

	GameObject fx;
	public bool breakable;


	// Use this for initialization
	void Start () {

		fx = transform.GetChild (0).gameObject;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D coll) {

		if (coll.gameObject.tag == "yoyo" && !Master.me.yoyoController.beingHeld) {

			if (breakable) {
				SoundController.me.PlaySound (Master.me.glass, .25f);
				fx.transform.parent = null;
				fx.SetActive(true);
				Destroy (this.gameObject);
			}

		}

	}
}
