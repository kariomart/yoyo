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
	bool comingBack;


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
		
		float dis = Vector2.Distance (player.transform.position, transform.position);

		if (dis > maxDistance) {
			comingBack = true;
		}

		if (comingBack) {
			dir = (player.transform.position - transform.position).normalized;
		}
		//Debug.Log (dis);
		vel = new Vector2 (dir.x + accel, dir.y + accel);





		if (dis < 1 && comingBack) {
			player.GetComponent<PlayerMovement> ().yoyoing = false;
			comingBack = false;
			this.gameObject.SetActive (false);
			transform.position = player.transform.position;
	}


		vel.x = Mathf.Max(Mathf.Min(vel.x, maxSpeed), -maxSpeed);
		vel.y = Mathf.Max(Mathf.Min(vel.y, maxSpeed), -maxSpeed);

		rb.MovePosition ((Vector2)transform.position + vel);




	}

	void OnTriggerEnter2D(Collider2D coll) {

		comingBack = true;

		if (coll.gameObject.tag == "Enemy") {

			Destroy (coll.gameObject);

		}
			
	}


	public void SetVelo(Vector2 direction) {

		dir = direction;


	}
}
