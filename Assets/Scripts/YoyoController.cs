using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class YoyoController : MonoBehaviour {

	Rigidbody2D rb;
	GameObject player;
	PlayerMovement playerController;
	public static YoyoController me;

	public string rightTrigger;

	public GameObject hitParticle;
	public GameObject deadYoyo;

	public float jumpDistanceScale;
	public float bonusFrames;
	public Vector2 vel;
	public Vector2 dir;
	public Vector2 prevVel;
	public float gravity;

	public float jumpSpd;
	public float jumpRange;
	public float spd;
	public float accel;
	public float maxSpeed;
	public float maxDistance;
	public float defaultMaxDistance;
	public float catchRange;
	public float returnSpeed;

	public bool comingBack;
	public bool beingHeld;
	public bool chillin;
	public bool grounded;
	public bool sticking;

	public GameObject grapplePoint;


	int counter = 0;
	public int bonusCounter = 0;


	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody2D> ();
		player = GameObject.Find ("Player");
		playerController = player.GetComponent<PlayerMovement> ();
		beingHeld = true;
		defaultMaxDistance = maxDistance;


	}

	void FixedUpdate() {
		
		
		float dis = Vector2.Distance (player.transform.position, transform.position);

		if (!beingHeld && !grounded){
			vel.y -= (gravity * Time.fixedDeltaTime);
		}

		if (sticking) {
			counter ++;

			if (counter > 10) {
				ReturnYoyo ();
				sticking = false;
				counter = 0;
			}


		}
		Vector2 pos = transform.position;

		dis = Vector2.Distance (player.transform.position, pos);


		if (dis > maxDistance) {
			pos = (Vector2)player.transform.position + ((pos - (Vector2)player.transform.position).normalized * maxDistance);
			vel += (pos - (Vector2)transform.position) * 4f;
		}

		if (comingBack && dis > .2f) {
			ReturnYoyo ();

		} 
        
		if ((comingBack && dis < catchRange && vel != Vector2.zero && !beingHeld)) {
//			Debug.Log ("recalled");
			vel = Vector2.zero;
			comingBack = false;
			beingHeld = true;
		}
			
		if (playerController.grappling) {

			vel = Vector2.zero;
			transform.position = transform.position;
			
		}

		if (playerController.bonusFrames) {
			if (bonusCounter < bonusFrames) {

				vel += playerController.dir1.normalized;
				bonusCounter++;

			} else {
				playerController.bonusFrames = false;
				bonusCounter = 0;
			}


		}

			
		if (beingHeld) {
//			Debug.Log("held");
            vel = Vector2.zero;
			transform.position = playerController.desiredPos + (playerController.vel * Time.fixedDeltaTime); //(Vector2)player.transform.position + playerController.desiredPos;

		} 
		 else {
			
            rb.MovePosition(pos + (vel * Time.fixedDeltaTime));
			prevVel = vel;
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
		

		if (coll.gameObject.tag == "Enemy") {
			
			if (playerController.takenObj == null && !coll.gameObject.GetComponent<EnemyController> ().dead) {
				EnemyController enemy = coll.gameObject.GetComponent<EnemyController> ();
				playerController.takenObj = coll.gameObject;
				SoundController.me.PlaySound (Master.me.enemy1, 1f);
				enemy.pulled = true;
			}

		}

		if (coll.gameObject.tag == "Stage") {
			Instantiate (hitParticle, transform.position, Quaternion.identity);

			if (!beingHeld && !playerController.grappling) {
				SoundController.me.PlaySound (Master.me.yoyoHit2, 1f);
			}

			StageCollision ();
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

	void OnCollisionStay2D(Collision2D coll) {

		StopGoingThisWay (vel - (Vector2)transform.position);

		if (comingBack) {
			//comingBack = false;
		}

	}


	public void StageCollision() {
		Vector2 jumpDir;
        
		//if ((player.transform.position - transform.position).magnitude < jumpRange && transform.position.y < player.transform.position.y && !beingHeld) {
		  if (!beingHeld && !playerController.grappling) {
			jumpDir = (player.transform.position - this.transform.position).normalized;
			//float jumpMag = (1 / (player.transform.position - transform.position).magnitude) * jumpDistanceScale;
			jumpDir.x += playerController.vel.normalized.x;
		    playerController.vel = jumpDir * jumpSpd;
			//vel = playerController.vel;
			//vel = jumpDir * (jumpSpd * 3f);
			sticking = true;
			//ReturnYoyo();
			//beingHeld = true;
		    }
			
			
	}

	public void ReturnYoyo() {

		Vector2 playerDir = (player.transform.position - this.transform.position).normalized;
		vel = playerDir * returnSpeed;
		comingBack = true;

	}

	public void DeactivateYoyo() {
		vel = Vector2.zero;
		comingBack = false;
		transform.position = player.transform.position;
		gameObject.SetActive (false);
		player.GetComponent<PlayerMovement> ().yoyoing = false;
	}

	public void CutYoyo() {

		Instantiate (deadYoyo, transform.position, Quaternion.identity);
		beingHeld = true;
		transform.position = player.transform.position;


	}


	public void SetVelo(Vector2 direction) {
		vel = direction;
	}

	void StopGoingThisWay(Vector2 a) {
		vel -= (a.normalized * Vector2.Dot(vel, a.normalized));
	}
}
