using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;


public class PlayerMovement : MonoBehaviour {

	YoyoPlayer player;

	public Rigidbody2D rb;   // ??? 
	public BoxCollider2D box;
	Transform startPos;

	public bool bonusFrames;

	public GameObject reticle;
	public GameObject pivot;
	public GameObject melee;

	public GameObject arm;
	public SpriteRenderer armSprite;

	public GameObject yoyo;
	public YoyoController yoyoController;

    InputDevice dev;

	public GameObject takenObj;  // -> YoYo? Or somewhere else

	// debugging tools
	Vector2[] debugPts;
	
	public Transform shootPt;

	public Direction stickDirection;

	public bool right;
	public bool left;

	public float gravity;
	public float unjumpBonusGrav;

	public Vector2 leftStickDir;
	public Vector2 untouchedLDir;
	public Vector2 rightStickDir;

	int jmpTimer;
    public float runMaxSpd;
    public float airMaxSpd;
	public float runAccel;
	public float airAccel;
	public float drag;
	public float swingSpeedScale;
	public float throwSpeed;
	public float jumpSpeed;

	public bool gameOver;			// move to Game model 

    public bool freshYoyo;
    Vector2 prevRStick;
    int timeSinceThrow;
    float throwSpd;
    public Vector2 desiredPos;


	// 

	bool beingHeld() {			// To be refactord later. 
		return yoyoController.state == YoyoController.YoyoState.held;
	}

	public void reset() {
		player.stop();
		goToCheckPoint();
	}

	public void goToCheckPoint() {
		this.transform.position = startPos.position;
		yoyoController.holdYoyo();
		takenObj = null;    // TODO - Refactor
		//yoyo.transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
	}


	// Unity Behavior implementation 
	void Start () {
		player = GetComponent<YoyoPlayer>();
		startPos = GameObject.Find("StartPos").transform;
		rb = GetComponent<Rigidbody2D>();
		box = GetComponentInChildren<BoxCollider2D>();
		yoyoController = yoyo.GetComponent<YoyoController> ();
		armSprite = arm.GetComponent<SpriteRenderer>();
		debugPts = new Vector2[2];

	}

	// Update is called once per frame
	void Update () {
		yoyoController.drawYoyoString (shootPt);
		checkForYoyoReturn ();

		if (Input.GetKeyDown(KeyCode.R)) {
			 Application.LoadLevel (0);   /// TODO - FIX WARNING
		}

		if (Input.GetButtonDown("start")) {
			Master.me.gameOver();
		}

        dev = InputManager.ActiveDevice;

		right = dev.LeftStick.X > 0;
		left = dev.LeftStick.X < 0;

		if (dev.Action2.WasPressed) {
			Master.me.gameOver();
			// goToCheckPoint ();
			// yoyoController.cutYoyo();
		}


		leftStickDir = new Vector2 (dev.LeftStickX, dev.LeftStickY);
		untouchedLDir = new Vector2 (dev.LeftStickX, dev.LeftStickY);

        if (leftStickDir.x < .25f) {
            leftStickDir = Vector2.zero;
        } else {
            leftStickDir.Normalize();
        }

        rightStickDir = new Vector2 (dev.RightStickX, dev.RightStickY);

		if (rightStickDir.magnitude > 1) {
			rightStickDir.Normalize ();
		}
		if (rightStickDir.magnitude < .25f) {
			rightStickDir = Vector2.zero;
		} else if (prevRStick == Vector2.zero && beingHeld()) {
            freshYoyo = true;
        }

		player.throwDirection = rightStickDir;

		if (dev.Action4.WasPressed && takenObj != null) {

			throwEnemy ();

		}


		if (Input.GetButtonDown ("start")) {
			//Time.timeScale = 1f;
			//Application.LoadLevel(Application.loadedLevel);
			//Yoyo();

		}

        prevRStick = rightStickDir;
	}

	private void FixedUpdate() {
        setGrounded();
		rotateArm();
//		checkForGrapple ();
		float dis = Vector2.Distance (yoyo.transform.position, transform.position);

		if (rightStickDir != Vector2.zero && freshYoyo) {
			throwYoyo();
		}
			
		float accel = runAccel;
		float mx = runMaxSpd;

		if (!player.grounded && !yoyoController.isGrappling()) {
			player.vel.y -= ZERO(gravity * Time.fixedDeltaTime);
			accel = airAccel;
			mx = airMaxSpd;
		}

		if (right) {
			player.vel.x += accel * Time.fixedDeltaTime;
			player.face(Direction.right);
		}
		
		if (left) {
			player.vel.x -= ZERO(accel * Time.fixedDeltaTime);
			player.face(Direction.left);
		}

		if (yoyoController.isGrappling()) {
			stopGoingThisWay (transform.position - yoyo.transform.position);
		}

		//Debug.Log("LEFT: " + left + " RIGHT " + right + " VEL.X " + player.vel.x);
		player.vel.x = ZERO(Mathf.Max(Mathf.Min(player.vel.x, mx), -mx));
		

		if (!right && !left && !yoyoController.isGrappling() && player.grounded) {
			player.vel.x = ZERO(Mathf.MoveTowards(player.vel.x, 0, drag));
		}

		
		// if (player.grounded) {
		// 	player.ground();   /// re-enforce grounding
		// }

		if (yoyoController.isGrappling()) {
			player.vel.x = player.vel.x / 2.0f;
		}



        desiredPos = (Vector2)transform.position + player.vel * Time.fixedDeltaTime;
        rb.MovePosition(desiredPos);
		
        timeSinceThrow++;



	}


    private void OnDrawGizmos() {
        if (Application.isPlaying) {
			Gizmos.color = Color.black;
            Gizmos.DrawCube(debugPts[0], Vector3.one * .01f);
            Gizmos.DrawCube(debugPts[1], Vector3.one * .01f);
        }
    }


	void rotateArm() {
		float aimAngle;


		if (rightStickDir == Vector2.zero) {
			aimAngle = 0;
			arm.transform.eulerAngles = new Vector3(0, 0, 0);	
		} else {
			aimAngle = Geo.ToAng(rightStickDir.normalized) + 90f;
			arm.transform.eulerAngles = new Vector3(0, 0, aimAngle);
		}

		float z = arm.transform.eulerAngles.z;
		//Debug.Log(z + " " + aimAngle);
		if (z <= 360 && z >= 180 || z == 0) {
			armSprite.flipX = false;
		} else {
			armSprite.flipX = true;
		}

		if (player.facing == Direction.left) {
			armSprite.flipX = true;
		}

		Vector3 pos = arm.transform.localPosition;

		if (player.facing == Direction.right) {
			arm.transform.localPosition = new Vector3(-0.168f, pos.y, pos.z);
		} if (player.facing == Direction.left) {
			arm.transform.localPosition = new Vector3(0.154f, pos.y, pos.z);
		}

		if (!beingHeld()) {
			float dirAngle = Geo.ToAng(yoyo.transform.position - shootPt.transform.position);
//			Debug.Log(dirAngle);
			arm.transform.eulerAngles = new Vector3(0, 0, dirAngle + 90f);
		}

	}
	
	bool swang = false;

	public void swing() {
		if (!swang) {
			player.vel += (Vector2)((yoyo.transform.position + ((transform.position - yoyo.transform.position).normalized * yoyoController.maxDistance)) - transform.position) * swingSpeedScale;
			swang = true;
		}
	}

	// Check to see if it's time for yoyo to return, and return it
	void checkForYoyoReturn() {
		if ((!yoyoController.comingBack || beingHeld()) && yoyoController.enabled) {

			if (timeSinceThrow < 5) {
				return;
			}
			Vector2 stickVector;
			Vector2 yoyoPlayerDir;
        
			stickVector = rightStickDir.normalized;
			yoyoPlayerDir = (yoyo.transform.position - transform.position).normalized;

			float dotProd = Vector2.Dot (stickVector, yoyoPlayerDir);
			if (dotProd <= -0.5f) {
				freshYoyo = false;

				if (!yoyoController.isGrappling() && !yoyoController.comingBack && !beingHeld()) {
					SoundController.me.PlaySound (Master.me.yoyoThrow, .5f);
				}

				yoyoController.returnYoyo ();

			} else {
				//yoyoController.comingBack = false;

			}
		}
	}

	public void jump() {
		Vector2 jumpDir;

		jumpDir = (player.transform.position - yoyo.transform.position).normalized;
		jumpDir.x += player.vel.normalized.x;
		player.vel = jumpDir * jumpSpeed;

	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (!player.grounded) {
			SoundController.me.PlaySound (Master.me.foot2, 1f);
		}

		if (coll.gameObject.name == "end") {
			Time.timeScale = 0;
		}
	}

    private void OnCollisionStay2D(Collision2D coll) {
        stopGoingThisWay(desiredPos - (Vector2)transform.position);
	}

    public void throwYoyo() {
		if (beingHeld()) {
			SoundController.me.PlaySound(Master.me.yoyoWhoosh, 0.5f);
			bonusFrames = true;
			timeSinceThrow = 0;
			yoyoController.throwYoyo(player.vel + player.throwDirection * throwSpeed);
			freshYoyo = false;				// TODO - what does this do? 
		}

    }

    public void throwEnemy() {
		EnemyController enemy = takenObj.GetComponent<EnemyController> ();
		enemy.enabled = false;
		enemy.thrownDir = (untouchedLDir.normalized);
		enemy.thrown = true;
		enemy.dead = true;
		enemy.enabled = true;
		takenObj.GetComponent<SpriteRenderer> ().color = Color.black;
		takenObj.GetComponent<BoxCollider2D> ().enabled = true;
		SoundController.me.PlaySound (Master.me.enemy3, 1f);
		//Destroy (takenObj, 5);
		takenObj = null;
	}

    void setGrounded() {
		Vector2 pt1 = (Vector2)transform.TransformPoint(box.offset + new Vector2((box.size.x / 2f), -box.size.y / 2f)) + new Vector2(-.01f, 0);//(box.size / 2));
		Vector2 pt2 = (Vector2)transform.TransformPoint(box.offset - (box.size / 2f)) + new Vector2(.01f, 0);

		debugPts[0] = pt1;
		debugPts[1] = pt2;

		bool prevGrounded = player.grounded;
		player.grounded = Physics2D.OverlapArea(pt1, pt2, LayerMask.GetMask("Platform")) != null;
	}

    public void stopGoingThisWay(Vector2 a) {
        player.vel -= a.normalized * Vector2.Dot(player.vel, a.normalized);
		Debug.Log("STOP GOING " + player.vel);
    }


	// For animation controller 

	public Vector2 velocity () {
		return player.vel;
	}

	public float speedX () {
		return player.vel.x;
	}

	public bool grounded() {
		return player.grounded;
	}

	public Direction facing() {
		return player.facing;
	}

	private float ZERO(float val) {
		return val;
	}


}

