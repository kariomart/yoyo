using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D coll) {

		//Debug.Log (coll.gameObject.name + " " + coll.gameObject.tag);

		if (coll.gameObject.tag != "Stage" && coll.gameObject.name != "Player") {

			Destroy (this.gameObject);

		}

		if (coll.gameObject.tag == "Enemy") {

			Destroy (this.gameObject);

		}


	}
}
