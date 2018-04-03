using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class YoyoController : MonoBehaviour {

	Rigidbody2D rb;
	SpriteRenderer spr;
	GameObject player;
	public GameObject throwPt;
	PlayerMovement playerController;
	public LineRenderer yoyoString;   // TODO - move line renderer here

	public string rightTrigger;

	public GameObject hitParticle;
	public GameObject deadYoyo;

	public float jumpDistanceScale;
	public float bonusFrames;
	public Vector2 vel;
	public Vector2 dir;
	public Vector2 prevVel;
	public float gravity;

	public float jumpRange;
	public float spd;
	public float accel;
	public float maxDistance;
	public float defaultMaxDistance;
	public float catchRange;
	public float returnSpeed;
	public float maxSpeed;
	public float jumpSpeed;

	public bool comingBack;
	public bool chillin;
	public bool grounded;
	public bool sticking;

	public enum YoyoState {held, thrown, grappling};
	public YoyoState state;

	int counter = 0;
	public int bonusCounter = 0;

	// Use this for initialization
	void Start () {
		yoyoString = GetComponentInChildren<LineRenderer> ();
		rb = GetComponent<Rigidbody2D> ();
		spr = GetComponent<SpriteRenderer>();
		player = GameObject.Find ("Player");
		playerController = player.GetComponent<PlayerMovement> ();
		state = YoyoState.held;
		defaultMaxDistance = maxDistance;
	}

	void FixedUpdate() {
		
		checkForGrapple();
		float dis = Vector2.Distance (player.transform.position, transform.position);

		if (state == YoyoState.thrown && !grounded){
			vel.y -= (gravity * Time.fixedDeltaTime);
		}

		if (sticking) {
			counter ++;

			if (counter > 10) {
				returnYoyo ();
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
			returnYoyo ();

		} 
        
		if ((comingBack && dis < catchRange && vel != Vector2.zero && state == YoyoState.thrown)) {
//			Debug.Log ("recalled");
			vel = Vector2.zero;
			comingBack = false;
			state = YoyoState.held; 
		}
			
		if (state == YoyoState.grappling) {
			vel = Vector2.zero;
			transform.position = transform.position;
		}

		if (playerController.bonusFrames) {
			if (bonusCounter < bonusFrames) {

				vel += playerController.rightStickDir.normalized;
				bonusCounter++;

			} else {
				playerController.bonusFrames = false;
				bonusCounter = 0;
			}

		}
		
		if (state == YoyoState.grappling) {
			playerController.swing();
		}

			
		if (state == YoyoState.held) {
//			Debug.Log("held");
            vel = Vector2.zero;
			transform.position = playerController.desiredPos + (playerController.velocity() * Time.fixedDeltaTime);
			spr.enabled = false; //(Vector2)player.transform.position + playerController.desiredPos;

		} 
		 else {
			
			spr.enabled = true;
            rb.MovePosition(pos + (vel * Time.fixedDeltaTime));
			prevVel = vel;
        }

    }

	public void holdYoyo() {
		state = YoyoState.held;
	}

	public void drawYoyoString(Transform starting) {
		if (enabled == true) {
			Vector2 p1 = starting.position;    // TODO - CAST PROBLEM? 
			Vector2 p2 = transform.position;
			yoyoString.SetPosition (0, p1);
			yoyoString.SetPosition (1, p2);

			Debug.DrawLine (p1, p2, Color.black);
			RaycastHit2D stringCast = Physics2D.Raycast (p1, (p2 - p1), Vector2.Distance (p1, p2), LayerMask.GetMask ("spike"));
			if (stringCast.collider != null && state != YoyoState.held) {
				Master.me.gameOver();
			}
		}
	}

	public void throwYoyo(Vector2 initialVelocity) {
		state = YoyoState.thrown;
		comingBack = false;
		vel = initialVelocity;
		this.gameObject.SetActive (true);
	}

	void checkForGrapple() {
		float dis = Vector2.Distance (player.transform.position, transform.position);

		if (dis >= (maxDistance - .01f) && grounded && transform.position.y > player.transform.position.y) {
			state = YoyoState.grappling;
		} else {
			if (state == YoyoState.grappling) {
				state = YoyoState.thrown;
			}

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

			if (state == YoyoState.thrown) {
				SoundController.me.PlaySound (Master.me.yoyoHit2, 1f);
			}

			StageCollision ();
			grounded = true;
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

		stopGoingThisWay (vel - (Vector2)transform.position);

		if (comingBack) {
			//comingBack = false;
		}

	}


	public void StageCollision() {
    
		if ((player.transform.position - transform.position).magnitude < jumpRange && transform.position.y < player.transform.position.y && state != YoyoState.held) {
		  //if (!beingHeld && !playerController.grappling) {
			playerController.jump();
			sticking = true;
		}
			
			
	}

	public void returnYoyo() {
		Vector2 playerDir = (player.transform.position - this.transform.position).normalized;
		vel = playerDir * returnSpeed;
		comingBack = true;
	}

	public void deactivateYoyo() {
		vel = Vector2.zero;
		comingBack = false;
		transform.position = player.transform.position;
		gameObject.SetActive (false);
		state = YoyoState.held;
	}

	void stopGoingThisWay(Vector2 a) {
		vel -= (a.normalized * Vector2.Dot(vel, a.normalized));
	}

	public bool isGrappling() {
		return state == YoyoState.grappling;
	}
}
