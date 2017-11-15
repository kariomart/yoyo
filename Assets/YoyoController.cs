using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoyoController : MonoBehaviour {

	Rigidbody2D rb;
	GameObject player;
	PlayerMovement playerController;

	public Vector2 vel;
	public Vector2 dir;

	public float spd;
	public float accel;
	public float maxSpeed;
	public float maxDistance;
	public bool comingBack;


	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody2D> ();
		player = GameObject.Find ("Player");
		playerController = player.GetComponent<PlayerMovement> ();

	}

	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {

		//Debug.Log ("coming back " + comingBack);

		float dis = Vector2.Distance (player.transform.position, transform.position);

		if (dis > maxDistance && !comingBack) {
			comingBack = true;
		}

		if (comingBack) {
			dir = (player.transform.position - transform.position).normalized;
		}
		//Debug.Log (dis);
		vel = new Vector2 (dir.x + accel, dir.y + accel);



		vel.x = Mathf.Max(Mathf.Min(vel.x, maxSpeed), -maxSpeed);
		vel.y = Mathf.Max(Mathf.Min(vel.y, maxSpeed), -maxSpeed);

		rb.MovePosition ((Vector2)transform.position + vel);


		if (dis < 1 && comingBack) {

			vel = Vector2.zero;
			//			Debug.Log ("got back");
			comingBack = false;
			transform.position = player.transform.position;
			this.gameObject.SetActive (false);
			player.GetComponent<PlayerMovement> ().yoyoing = false;

		}




	}

	void OnTriggerEnter2D(Collider2D coll) {

		Vector2 jumpDir;
		comingBack = true;

		if (coll.gameObject.tag == "Enemy") {

			Destroy (coll.gameObject);

		}

		if (coll.gameObject.tag == "Stage") {

			Debug.Log ("Hit Stage");
			jumpDir = (player.transform.position - this.transform.position).normalized;
			Debug.Log ("jumpForce " + jumpDir * playerController.jumpSpd);
			playerController.vel = jumpDir * playerController.jumpSpd;

			//playerController.yoyoJump ();

		}

	}


	public void SetVelo(Vector2 direction) {

		dir = direction;


	}
}
