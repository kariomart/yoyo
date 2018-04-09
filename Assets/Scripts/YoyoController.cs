using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class YoyoController : MonoBehaviour {

	Rigidbody2D rb;
	SpriteRenderer spr;
	GameObject player;
	public AnimationController playerAnim;
	public GameObject throwPt;
	PlayerMovement playerController;
	public static YoyoController me;
	AudioSource spinningAudio;

	public string rightTrigger;

	public GameObject hitParticle;
	public GameObject deadYoyo;

	public float jumpDistanceScale;
	public float bonusFrames;
	public float jumpFrames;
	public Vector2 vel;
	public Vector2 dir;
	public Vector2 prevVel;
	public float dis;
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
	public float jumpReturnCount;

	public bool comingBack;
	public bool beingHeld;
	public bool grounded;
	public bool sticking;

	public GameObject grapplePoint;


	int counter = 0;
	public int bonusCounter = 0;


	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody2D> ();
		spr = GetComponentInChildren<SpriteRenderer>();
		spinningAudio = GetComponentInChildren<AudioSource>();
		player = GameObject.Find ("Player");
		playerController = player.GetComponent<PlayerMovement> ();
		beingHeld = true;
		defaultMaxDistance = maxDistance;

	}

	void FixedUpdate() {
		
		//Debug.Log(playerController.vel);
		jumpFrames ++;
		dis = Vector2.Distance (player.transform.position, transform.position);

		if (!beingHeld && !grounded){
			vel.y -= (gravity * Time.fixedDeltaTime);
		}

		if (sticking) {
			counter ++;

			if (counter > jumpReturnCount) {
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

		if (dis > maxDistance * 1.5f) {
			beingHeld = true;
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
			

		if (playerController.bonusFrames) {
			if (bonusCounter < bonusFrames) {

				vel += playerController.dir1.normalized;
				bonusCounter++;

			} else {
				playerController.bonusFrames = false;
				bonusCounter = 0;
			}


		}

		if (playerController.grappling) {

			vel = Vector2.zero;
			transform.position = transform.position;
			
		}
			
		if (beingHeld) {
			//Debug.Log("held");
            vel = Vector2.zero;
			transform.position = playerController.desiredPos + (playerController.vel * Time.fixedDeltaTime);
			spr.enabled = false;
			grounded = false;
			spinningAudio.Stop();
			 //(Vector2)player.transform.position + playerController.desiredPos;

		} 
		 else {
			
			spr.enabled = true;
            rb.MovePosition(pos + (vel * Time.fixedDeltaTime));
			prevVel = vel;
        }
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

			if (!beingHeld && !playerController.grappling && vel.y != 0) {
				SoundController.me.PlaySound (Master.me.yoyoHit2, .25f);
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

		grounded = true;

		//StopGoingThisWay (vel - (Vector2)transform.position);
		StopGoingThisWay(this.transform.position - coll.transform.position);

		if (transform.position.y < player.transform.position.y && dis > jumpRange) {
			//grounded = false;
		}

		if (comingBack) {
			//comingBack = false;
		}

	}


	public void StageCollision() {
		Vector2 jumpDir;
        if (spinningAudio.volume > 0.001) {
			spinningAudio.volume *= .5f;
		}

		if ((player.transform.position - transform.position).magnitude < jumpRange && transform.position.y < player.transform.position.y && !beingHeld) {
			jumpFrames = 0;
			//Debug.Log("jumped triggered, vel= " + playerController.vel);
		  //if (!beingHeld && !playerController.grappling) {
			jumpDir = (player.transform.position - this.transform.position).normalized;
			//float jumpMag = (1 / (player.transform.position - transform.position).magnitude) * jumpDistanceScale;
			jumpDir.x += playerController.vel.normalized.x;
		    playerController.vel = jumpDir * jumpSpd;
			playerController.grounded = false;
			playerAnim.triggerJumpAnim();
			//Debug.Log("jumpDir= " + jumpDir + "\nvel after= " + playerController.vel);
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
		spinningAudio.Play();
		spinningAudio.volume = 0.068f;
	}

	void StopGoingThisWay(Vector2 a) {
		vel -= (a.normalized * Vector2.Dot(vel, a.normalized));
	}
}
