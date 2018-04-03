using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

	public Vector2 vel;
	public float spd;
	Rigidbody2D rb;

	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody2D> ();
		Destroy (this.gameObject, 5);
		
	}

	
	// Update is called once per frame
	void Update () {


	}

	void FixedUpdate() {

		rb.MovePosition ((Vector2)transform.position + vel * spd);

	}

	void OnTriggerEnter2D(Collider2D coll) {
		Destroy (this.gameObject);


		if (coll.gameObject.tag == "Player") {
			Master.me.gameOver();
		}


	}
}
