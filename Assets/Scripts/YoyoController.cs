using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoyoController : MonoBehaviour {

	Rigidbody2D rb;
	GameObject player;
	PlayerMovement playerController;

	public string rightTrigger;

	public GameObject hitParticle;

	public Vector2 vel;
	public Vector2 dir;
	public float dis;
	public float gravity;

	public float jumpSpd;
	public float spd;
	public float accel;
	public float maxSpeed;
	public float maxDistance;

	public bool comingBack;
	public bool chillin;

	public GameObject grapplePoint;

	public AudioClip bounce;


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

		Debug.Log (Input.GetAxis (rightTrigger));
		//Debug.Log ("coming back " + comingBack);
		if (!playerController.grappling) {

			dis = Vector2.Distance (player.transform.position, transform.position);

			if (dis > maxDistance && !comingBack) {
				comingBack = true;
			}

			if (comingBack) {
				if (playerController.dir1 != Vector2.zero && dis > maxDistance - 1f) {

					chillin = true;
					dir = Vector2.zero;

				} else {
					chillin = false;
					dir = (player.transform.position - transform.position).normalized;
				}
			}

			if (playerController.yoyoing) {

				dir.y -= gravity;

			}
			//Debug.Log (dis);

			vel = new Vector2 (dir.x + accel, dir.y + accel);



			vel = vel.normalized * Mathf.Min (vel.magnitude, maxSpeed);

			if (!chillin) {
			rb.MovePosition ((Vector2)transform.position + vel);
			}



			if (dis < 1 && comingBack) {

				deactivateYoyo ();

			}
		}
			

		if (playerController.grappling) {

			//transform.position = grapplePoint.transform.position;

		}


	}

	void OnTriggerEnter2D(Collider2D coll) {

		Vector2 jumpDir;
		SoundController.me.PlaySound (bounce, .2f);
		comingBack = true;

		if (coll.gameObject.tag == "Enemy") {

			if (playerController.takenObj == null) {
				EnemyController enemy = coll.GetComponent<EnemyController> ();
				playerController.takenObj = this.gameObject;
				enemy.pulled = true;
			}

		}

		if (coll.gameObject.tag == "Stage") {

			if (Input.GetAxis(rightTrigger) == 1) {

				playerController.grappling = true;
				playerController.yoyoing = false;

				//grapplePoint = this

			}

			else if (dis < 1) {
				
				jumpDir = (player.transform.position - this.transform.position).normalized;
//				Debug.Log (jumpDir);
				Instantiate (hitParticle, transform.position, Quaternion.identity);
				playerController.vel = jumpDir * jumpSpd;

			}

		}

		if (coll.gameObject.tag == "grapple") {

			playerController.grappling = true;
			playerController.yoyoing = false;
			grapplePoint = coll.gameObject;

		}

		if (coll.gameObject.tag == "Player" && comingBack) {

			deactivateYoyo ();

		}

	}

	public void deactivateYoyo() {
		
		vel = Vector2.zero;
		//			Debug.Log ("got back");
		comingBack = false;
		transform.position = player.transform.position;
		this.gameObject.SetActive (false);
		player.GetComponent<PlayerMovement> ().yoyoing = false;


	}



	public void SetVelo(Vector2 direction) {

		dir = direction;


	}
}
