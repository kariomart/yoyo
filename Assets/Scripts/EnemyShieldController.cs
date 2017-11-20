using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShieldController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnTriggerEnter2D (Collider2D coll) {

		if (coll.gameObject.tag == "Bullet" || coll.gameObject.tag == "Enemy") {

			Destroy (coll.gameObject);
			Destroy (this.transform.parent.gameObject);

		}




	}
}
