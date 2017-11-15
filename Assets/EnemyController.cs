using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public bool pulled;
	public bool taken;
	public Transform yoyo;
	public GameObject player;
	public PlayerMovement playerController;
	BoxCollider2D collider;
	Rigidbody2D rigid;

	// Use this for initialization
	void Start () {
			
		yoyo = GameObject.Find ("yoyo").transform;
		player = GameObject.Find ("Player");
		playerController = player.GetComponent<PlayerMovement> ();
		collider = GetComponent<BoxCollider2D> ();
		rigid = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		float dis = Vector2.Distance (player.transform.position, transform.position);

		if (pulled && dis > 1) {

			transform.position = yoyo.position;
		} 

		else if (pulled && dis < 1){
			
			pulled = false;
			taken = true;
		}

		if (taken) {

			transform.position = (Vector2)player.transform.position + playerController.dir.normalized;
			playerController.takenObj = this.gameObject;
			collider.enabled = false;
			rigid.isKinematic = true;

		}


	}

	void OnCollisionEnter2D(Collision2D coll) {

		//Debug.Log (coll.gameObject.name + " " + coll.gameObject.tag);

		if (coll.gameObject.tag != "Stage" && coll.gameObject.name != "Player") {

			//Destroy (this.gameObject);

		}
			


	}

	public void Pulled() {




	}
}
