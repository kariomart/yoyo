using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;

public class PlayerMovement : MonoBehaviour {

	Rigidbody2D rb;

	/*public string rightTrigger;
	public string leftTrigger;
	public string xButton;
	public string yButton;
	public string aButton;
	//public string rightStick;
	public string leftStickH;
	public string leftStickV;
	public string rightStickH;
	public string rightStickV;

	public string shootKey;*/

	public float health;

	public Vector2 vel;
	Vector2 bulletDir;
	bool jumpFlag;
	public bool grounded;

	public bool trigger;
	public bool right;
	public bool left;
	public bool slow;
	public bool speed;
	public bool yoyoing;
	public bool bonusFrames;

	//public Transform groundPt1;
	//public Transform groundPt2;
	public Transform shootPt;
	public float jumpSpd;
	public float gravity;
	public float unjumpBonusGrav;

	int face;
	int jmpTimer;
	public float runMaxSpd;
	public float airMaxSpd;
	public float runAccel;
	public float airAccel;
	public float drag;
	public float swingSpeedScale;
	public Transform sprite;

	Vector3 defSprScale;
	public GameObject bullet;
	public GameObject yoyo;
	public YoyoController yoyoController;
	public GameObject reticle;
	public bool gameOver;
	public int shotCoolDown;

	public GameObject pivot;
	public GameObject melee;
	public LineRenderer yoyoString;

	public Vector2 dir;
	public Vector2 untouchedLDir;
	public Vector2 dir1;
	public Vector2 defaultShootingDirection = new Vector2(1, 0);
	public BoxCollider2D box;
	Vector2[] debugPts;

	public GameObject takenObj;
	public bool grappling;
	public bool goingToGrapple;
	public float grappleSpd;

	public Text timer;
	float startTime;

    InputDevice dev;

    public bool freshYoyo;
    Vector2 prevRStick;
    int timeSinceThrow;
    public float throwSpd;
    public Vector2 desiredPos;

	public GameObject throwyo;

	//public float drag;
	void Awake () {



	}

	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody2D>();
		box = GetComponentInChildren<BoxCollider2D>();
		yoyoString = GetComponentInChildren<LineRenderer> ();
		yoyoController = yoyo.GetComponent<YoyoController> ();
		defSprScale = sprite.localScale;
		debugPts = new Vector2[2];
		startTime = Time.time;

	}

	// Update is called once per frame
	void Update () {

		DrawYoyoString ();
		CheckForYoyoReturn ();
//		Debug.Log (bonusFrames + " " + yoyoController.bonusCounter);

		if (Input.GetKeyDown(KeyCode.R)) {
			Application.LoadLevel (0);
		}

		if (Input.GetButtonDown("start")) {
			GoToCheckPoint ();
			yoyoController.beingHeld = true;
		}

        dev = InputManager.ActiveDevice;
        trigger = dev.RightTrigger > .75f;
		right = dev.LeftStick.X > 0;
		left = dev.LeftStick.X < 0;

		if (dev.Action2.WasPressed) {

			GoToCheckPoint ();
			yoyoController.CutYoyo ();

		}





		dir = new Vector2 (dev.LeftStickX, dev.LeftStickY);
		untouchedLDir = new Vector2 (dev.LeftStickX, dev.LeftStickY);
        if (dir.x < .25f) {
            dir = Vector2.zero;
        } else {
            dir.Normalize();
        }

        dir1 = new Vector2 (dev.RightStickX, dev.RightStickY);

		if (dir1.magnitude > 1) {
			dir1.Normalize ();
		}
		if (dir1.magnitude < .25f) {
			dir1 = Vector2.zero;
		} else if (prevRStick == Vector2.zero && yoyoController.beingHeld) {
            freshYoyo = true;
        }

		//if (Input.GetButtonDown(aButton) || Input.GetKeyDown(KeyCode.Space)) {
			//	jumpFlag = true;
		//}


		if (dev.Action2.WasPressed){//Input.GetButtonDown (xButton) || Input.GetKeyDown(shootKey)) {

			//ShootBullet ();

		}

		if (dev.Action4.WasPressed && takenObj != null) {

			ThrowEnemy ();

		}

		if (dev.RightTrigger.WasPressed && !yoyoController.beingHeld) {

			yoyoController.ReturnYoyo ();

		}


		if (Input.GetButtonDown ("start")) {
			//Time.timeScale = 1f;
			//Application.LoadLevel(Application.loadedLevel);
			//Yoyo();

		}





		if (health <= 0 && !gameOver) {

		}
        prevRStick = dir1;
	}

	private void FixedUpdate() {
        SetGrounded();
        updateTimer();
		CheckForGrapple ();
		float dis = Vector2.Distance (yoyo.transform.position, transform.position);

		if (dir1 != Vector2.zero && freshYoyo) {
			Yoyo ();
		}
			


			
//
//		if (grappling && dev.Action1.IsPressed) {
//			goingToGrapple = true;
//		}
//
//		if (goingToGrapple) {
//			//vel = (dir + (Vector2)(yoyo.transform.position - transform.position).normalized) * grappleSpd;
//			if (yoyoController.maxDistance > .5f) {
//				yoyoController.maxDistance -= .5f;
//			}
//		}
//
//	    if (goingToGrapple &&(grappling && dev.Action1.WasReleased)) {
//			
//			goingToGrapple = false;
//			grappling = false;
//			yoyoing = true;
//			yoyoController.ReturnYoyo ();
//			yoyoController.maxDistance = yoyoController.defaultMaxDistance;
//
//		}

//		if (!grappling) {
//			yoyoController.maxDistance = yoyoController.defaultMaxDistance;
//
//		}
	
		float accel = runAccel;
		float mx = runMaxSpd;

		if (!grounded || grappling) {
			vel.y -= gravity * Time.fixedDeltaTime;
			accel = airAccel;
			mx = airMaxSpd;
		}

		if (right && !trigger) {
			vel.x += accel * Time.fixedDeltaTime;
			face = 1;

		}
		if (left && !trigger) {
			vel.x -= accel * Time.fixedDeltaTime;
			face = -1;
		}

		vel.x = Mathf.Max(Mathf.Min(vel.x, mx), -mx);



		if (!right && !left && !grappling && grounded) {
			vel.x = Mathf.MoveTowards(vel.x, 0, drag);
		}

		jumpFlag = false;

		if (grounded && vel.y < 0) {
			//vel.y = 0;
		}


        desiredPos = (Vector2)transform.position + vel * Time.fixedDeltaTime;



        rb.MovePosition(desiredPos);
        timeSinceThrow++;
	}

	void updateTimer() {
		
		float t = (float)System.Math.Round (Time.time - startTime, 2);
		timer.text = "TIME: " + t;

	}

	void DrawYoyoString() {

		if (yoyoController.enabled == true) {
			Vector2 p1 = transform.position;
			Vector2 p2 = yoyo.transform.position;
			yoyoString.SetPosition (0, p1);
			yoyoString.SetPosition (1, p2);

			//int layer = LayerMask.NameToLayer ("Player");

			Debug.DrawLine (p1, p2, Color.black);
			RaycastHit2D stringCast = Physics2D.Raycast (p1, (p2 - p1), Vector2.Distance (p1, p2), LayerMask.GetMask ("spike"));


			if (stringCast.collider != null && !yoyoController.beingHeld) {

				GoToCheckPoint ();
				yoyoController.CutYoyo ();
			}
		}

	}
		

	void CheckForGrapple() {
		
		float dis = Vector2.Distance (transform.position, yoyo.transform.position);
		Vector2 pos = transform.position;

		if (dis >= (yoyoController.maxDistance - .01f)&& yoyoController.grounded && yoyo.transform.position.y > transform.position.y) {
			//Vector2 perpVect = Geo.PerpVectL (yoyo.transform.position, transform.position);
			StopGoingThisWay (transform.position - yoyo.transform.position);
			grappling = true;
			vel += (Vector2)((yoyo.transform.position + ((transform.position - yoyo.transform.position).normalized * yoyoController.maxDistance)) - transform.position) * swingSpeedScale;

		} else {
			grappling = false;
		}
			




	}

	void CheckForYoyoReturn() {
		if ((!yoyoController.comingBack || yoyoController.beingHeld) && yoyoController.enabled) {

			if (timeSinceThrow < 5) {
				return;
			}
			Vector2 stickDir;
			Vector2 yoyoPlayerDir;
        
			stickDir = dir1.normalized;
			yoyoPlayerDir = (yoyo.transform.position - transform.position).normalized;

			float dotProd = Vector2.Dot (stickDir, yoyoPlayerDir);
//		Debug.Log (dotProd);

			if (dotProd <= -0.5f) {
				freshYoyo = false;

				if (!grappling && !yoyoController.comingBack && !yoyoController.beingHeld) {
					SoundController.me.PlaySound (Master.me.yoyoThrow, .5f);
				}

				yoyoController.ReturnYoyo ();

			} else {
				//yoyoController.comingBack = false;

			}
		}
	}



	void OnCollisionEnter2D(Collision2D coll) {

		if (grounded) {
			Vector2 normal = coll.contacts [0].normal;
			//transform.localEulerAngles = new Vector3 (0, 0, Geo.ToAng (normal) - 90);

		}
		if (!grounded) {
			SoundController.me.PlaySound (Master.me.foot2, 1f);

		}

		//transform.localRotation 

		if (coll.gameObject.name == "end") {
			Time.timeScale = 0;
		}

		if (yoyoController.beingHeld) {
			yoyo.transform.position = transform.position;//(Vector2)player.transform.position + playerController.desiredPos;
		}

	}

    private void OnCollisionStay2D(Collision2D coll) {
        StopGoingThisWay(desiredPos - (Vector2)transform.position);

		if (yoyoController.beingHeld) {
			yoyo.transform.position = transform.position;
		}

//		if (vel.y != 0) {
//			StopGoingThisWay (-coll.contacts [0].normal);
//		}
    }

    public void Yoyo() {
		if (yoyoController.beingHeld) {
			bonusFrames = true;
			SoundController.me.PlaySound (Master.me.yoyoWhoosh, .5f);
			timeSinceThrow = 0;
			yoyoController.beingHeld = false;
			yoyoController.comingBack = false;
			yoyoController.SetVelo ((dir1 * throwSpd) + vel);
			//yoyoController.maxDistance = 5f * dir1.magnitude;
			//yoyoController.maxSpeed = .5f * dir1.magnitude;
			yoyo.SetActive (true);
			freshYoyo = false;
		}

    }


    public void ThrowEnemy() {
		

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

	public void GoToCheckPoint() {

		SoundController.me.PlaySound (Master.me.restart, 1f);
		vel = Vector2.zero;
		yoyoController.beingHeld = true;
		transform.position = GameObject.Find("StartPos").transform.position;
		Master.me.RespawnEnemies ();
		takenObj = null;
		//yoyo.transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);

	}


	public void ShootBullet() {

		GameObject tempBullet = Instantiate(bullet, new Vector3(shootPt.position.x + dir.x, shootPt.transform.position.y + dir.y), Quaternion.identity);
		BulletController bulletController = tempBullet.GetComponent<BulletController> ();
		if (dir == Vector2.zero) {

			bulletController.vel = new Vector2 (1, 0);

		} else {

			bulletController.vel = dir;
		}
	}


    private void OnDrawGizmos() {
        if (Application.isPlaying) {
			Gizmos.color = Color.black;
            Gizmos.DrawCube(debugPts[0], Vector3.one * .01f);
            Gizmos.DrawCube(debugPts[1], Vector3.one * .01f);
        }
    }


    void SetGrounded() {

		Vector2 pt1 = (Vector2)transform.TransformPoint(box.offset + new Vector2((box.size.x / 2f), -box.size.y / 2f)) + new Vector2(-.01f, 0);//(box.size / 2));
		Vector2 pt2 = (Vector2)transform.TransformPoint(box.offset - (box.size / 2f)) + new Vector2(.01f, 0);

		debugPts[0] = pt1;
		debugPts[1] = pt2;

		bool prevGrounded = grounded;
		grounded = Physics2D.OverlapArea(pt1, pt2, LayerMask.GetMask("Platform")) != null;
		//		Debug.Log (grounded); 

		if (grounded) {
			vel.y = Mathf.Max(0, vel.y);
			//safety = true;
		}
	}

    void StopGoingThisWay(Vector2 a) {
        vel -= (a.normalized * Vector2.Dot(vel, a.normalized));
    }


}


