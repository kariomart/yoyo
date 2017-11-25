using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

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
	public bool beingHeld;
	public bool chillin;
	public bool grounded;

	public GameObject grapplePoint;

	public AudioClip bounce;
	InputDevice dev;


	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody2D> ();
		player = GameObject.Find ("Player");
		playerController = player.GetComponent<PlayerMovement> ();
		dev = InputManager.ActiveDevice;

		beingHeld = true;


	}

	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {
		//Debug.Log (dev.Action1.WasPressed);
		
		float dis = Vector2.Distance (player.transform.position, transform.position);
//		Debug.Log (dis);
		if (!beingHeld){
			vel.y -= gravity;
		}
		Vector2 pos = transform.position;

		dis = Vector2.Distance (player.transform.position, pos);


		if (dis > maxDistance) {
			pos = (Vector2)player.transform.position + ((pos - (Vector2)player.transform.position).normalized * maxDistance);
			vel += (pos - (Vector2)transform.position) * .1f;
		}

		if (comingBack && dis > .2f) {
			ReturnYoyo ();

		} 

		if ((comingBack && dis < 1f && vel != Vector2.zero && !beingHeld)) {
			Debug.Log ("recalled");
			vel = Vector2.zero;
			comingBack = false;
			beingHeld = true;
		}
			

		rb.MovePosition (pos + vel);

		if (beingHeld) {
			//vel = Vector2.zero;
			Debug.Log("held");
			transform.position = player.transform.position;
		}


//		Debug.Log (Input.GetAxis (rightTrigger));
		//Debug.Log ("coming back " + comingBack);
//		if (!playerController.grappling) {
//
//			dis = Vector2.Distance (player.transform.position, transform.position);
//
//			/*if (dis > maxDistance && !comingBack) {
//				comingBack = true;
//			}*/
//
//			if (comingBack) {
////				if (playerController.dir1 != Vector2.zero && dis > maxDistance - 1f) {
////
////					chillin = true;
////					dir = Vector2.zero;
////
////				} else {
//					chillin = false;
//					dir = (player.transform.position - transform.position).normalized;
//				//}
//			}
//
//			if (playerController.yoyoing) {
//
//				dir.y -= gravity;
//
//			}
//			//Debug.Log (dis);
//
//			vel = new Vector2 (dir.x + accel, dir.y + accel);
//
//
//
//			vel = vel.normalized * Mathf.Min (vel.magnitude, maxSpeed);
//
//			if (!chillin) {
//				rb.MovePosition ((Vector2)transform.position + vel);
//			}
//
//
//
//			if (dis < 1 && comingBack) {
//
//				DeactivateYoyo ();
//
//			}
//		}
//			
//
//		if (playerController.grappling) {
//
//			//transform.position = grapplePoint.transform.position;
//
//		}
//

	}

	void OnCollisionEnter2D(Collision2D coll) {
		//return;
		SoundController.me.PlaySound (bounce, .2f);
		//comingBack = true;

		if (coll.gameObject.tag == "Enemy") {

			if (playerController.takenObj == null) {
				EnemyController enemy = coll.gameObject.GetComponent<EnemyController> ();
				playerController.takenObj = this.gameObject;
				enemy.pulled = true;
			}

		}

		if (coll.gameObject.tag == "Stage") {
			//StageCollision ();
			grounded = true;
		}

		if (coll.gameObject.tag == "grapple") {

			playerController.grappling = true;
			playerController.yoyoing = false;
			grapplePoint = coll.gameObject;

		}

		if (coll.gameObject.tag == "Player" && comingBack) {
			Debug.Log ("collided with player");
			//DeactivateYoyo ();
			if (vel != Vector2.zero) {
				Debug.Log ("collided with player + velo killed");
				vel = Vector2.zero;
			}


		}

	}

	void OnCollisionExit2D(Collision2D coll) {

		grounded = false;

	}


	public void StageCollision() {
		Vector2 jumpDir;

		/*if (Input.GetAxis(rightTrigger) == 1) {

				playerController.grappling = true;
				playerController.yoyoing = false;

				//grapplePoint = this

			}

			else*/ if (dis < 1) {

			jumpDir = (player.transform.position - this.transform.position).normalized;
			//				Debug.Log (jumpDir);
			Instantiate (hitParticle, transform.position, Quaternion.identity);
			playerController.vel = jumpDir * jumpSpd;

		}




	}

	public void ReturnYoyo() {

		Vector2 playerDir = (player.transform.position - this.transform.position).normalized;
		vel = playerDir;


	}

	public void DeactivateYoyo() {
		
		vel = Vector2.zero;
		//			Debug.Log ("got back");
		comingBack = false;
		transform.position = player.transform.position;
		gameObject.SetActive (false);
		player.GetComponent<PlayerMovement> ().yoyoing = false;


	}



	public void SetVelo(Vector2 direction) {

		//dir = direction;
		vel = direction;

	}
}
