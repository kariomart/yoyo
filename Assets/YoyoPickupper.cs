using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoyoPickupper : MonoBehaviour {

	public YoyoController yoyo;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D coll) {

		if (coll.gameObject.name == "Player") {

			//yoyo.beingHeld = true;
			yoyo.transform.position = transform.position;
			yoyo.gameObject.SetActive (true);
			yoyo.enabled = true;
			Destroy (this.gameObject);

		}


	}
}
