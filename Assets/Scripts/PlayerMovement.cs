using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;

public class PlayerMovement : MonoBehaviour {


	Rigidbody2D rb;

	public string rightTrigger;
	public string leftTrigger;
	public string xButton;
	public string yButton;
	public string aButton;
	//public string rightStick;
	public string leftStickH;
	public string leftStickV;
	public string rightStickH;
	public string rightStickV;

	public string shootKey;

	public float health;

	public Vector2 vel;
	Vector2 bulletDir;
	bool jumpFlag;
	bool grounded;

	public bool trigger;
	public bool right;
	public bool left;
	public bool slow;
	public bool speed;
	public bool yoyoing;

	public Transform groundPt1;
	public Transform groundPt2;
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
	public Transform sprite;
	//bool spinning;
	//public float spinSpd;
	//public float spinDir;
	public float knockbackAmount;


	Vector3 defSprScale;
	public GameObject bullet;
	public GameObject yoyo;
	public YoyoController yoyoController;
	public GameObject reticle;
	public bool gameOver;
	public int shotCoolDown;
	int shotTimer;
	bool safety;
	public float bulletSpeed;

	public Vector2 defaultScale;
	public GameObject pivot;
	public GameObject melee;

	public Vector2 dir;
	public Vector2 dir1;
	public Vector2 defaultShootingDirection = new Vector2(1, 0);
	BoxCollider2D box;
	Vector2[] debugPts;

	public GameObject takenObj;
	public bool grappling;
	public bool goingToGrapple;
	public float grappleSpd;

	public Text timer;
	float startTime;

    InputDevice dev;

    bool freshYoyo;
    Vector2 prevRStick;

	//public float drag;

	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody2D>();
		box = GetComponentInChildren<BoxCollider2D>();
		yoyoController = yoyo.GetComponent<YoyoController> ();
		//sprite = GetComponent<SpriteRenderer> ();
		defSprScale = sprite.localScale;
		debugPts = new Vector2[2];
		defaultScale = pivot.transform.localScale;
		startTime = Time.time;

	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("start") || transform.position.y < -10f) {
			Application.LoadLevel(Application.loadedLevel);
		}
        dev = InputManager.ActiveDevice;
        trigger = dev.RightTrigger > .75f;//Input.GetAxis (rightTrigger) == 1;
		right = dev.LeftStick.X > 0;
		left = dev.LeftStick.X < 0;
		//right = Input.GetKeyDown(KeyCode.RightArrow);
		//left = Input.GetKeyDown(KeyCode.LeftArrow);







		dir = new Vector2 (dev.LeftStickX, dev.LeftStickY);
		dir.Normalize ();

		dir1 = new Vector2 (dev.RightStickX, dev.RightStickY);

		if (dir1.magnitude > 1) {
			dir1.Normalize ();
		}
		if (dir1.magnitude < .25f) {
			dir1 = Vector2.zero;
		} else if (/*prevRStick == Vector2.zero ||*/ yoyoController.beingHeld) {
            freshYoyo = true;
        }
		//Debug.Log (dir1);

		if (Input.GetButtonDown(aButton) || Input.GetKeyDown(KeyCode.Space)) {
			//	jumpFlag = true;
		}


		if (dev.Action2.WasPressed){//Input.GetButtonDown (xButton) || Input.GetKeyDown(shootKey)) {

			//ShootBullet ();

		}

		if (dev.Action4.WasPressed) {

			Melee ();

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
		CheckForYoyoReturn ();

		if (dir1 != Vector2.zero && freshYoyo) {
			Yoyo ();
		}
			

		if (grappling && dev.Action1.IsPressed) {
			float dis = Vector2.Distance (yoyo.transform.position, transform.position);


			goingToGrapple = true;

			if (dis < 8f) {
				vel = (dir + (Vector2)(yoyo.transform.position - transform.position).normalized) * grappleSpd;
			}

		} else if (goingToGrapple || (grappling && dev.Action2.WasPressed)) {
			goingToGrapple = false;
			grappling = false;
			yoyoing = true;
		}


		float accel = runAccel;
		float mx = runMaxSpd;

		if (!grounded) {
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



		if (!right && !left) {
			vel.x = Mathf.MoveTowards(vel.x, 0, drag);
		}

		jumpFlag = false;
		shotTimer--;
		if (grounded && vel.y < 0) {
			vel.y = 0;
		}

		rb.MovePosition ((Vector2)transform.position + vel * Time.fixedDeltaTime);
	}

	void updateTimer() {
		
		float t = (float)System.Math.Round (Time.time - startTime, 2);
		timer.text = "TIME: " + t;

	}

	void CheckForYoyoReturn() {

		Vector2 stickDir;
		Vector2 yoyoPlayerDir;

		stickDir = dir1.normalized;
		yoyoPlayerDir = (yoyo.transform.position - this.transform.position).normalized;

		float dotProd = Vector2.Dot (stickDir, yoyoPlayerDir);
//		Debug.Log (dotProd);

		if (dotProd <= -0.8f) {

			yoyoController.comingBack = true;

		} else {
			yoyoController.comingBack = false;

		}




	}



	void OnCollisionEnter2D(Collision2D coll) {

		if (coll.gameObject.name == "end") {

			Time.timeScale = 0;

		}


	}

	public void Yoyo() {

		yoyoController.beingHeld = false;
		yoyoController.SetVelo (dir1 * .5f);
		//yoyoController.maxDistance = 5f * dir1.magnitude;
		//yoyoController.maxSpeed = .5f * dir1.magnitude;
		yoyo.SetActive (true);
        freshYoyo = false;

	}


	public void Melee() {


		melee.GetComponent<MeleeController> ().meleeTimer = melee.GetComponent<MeleeController> ().meleeLength;

//		if (dir == Vector2.zero) {
//			melee.transform.position = (Vector2)this.transform.position + new Vector2 (.5f, 0);
//		} else{
//			melee.transform.position = (Vector2)this.transform.position + dir * .5f;
//		}
//
//		melee.transform.eulerAngles = new Vector3 (melee.transform.eulerAngles.x, melee.transform.eulerAngles.y, Geo.ToAng (dir)); 
		//melee.SetActive (true);

		if (takenObj != null) {

			//Destroy (takenObj);
			takenObj.GetComponent<EnemyController>().enabled = false;
			Rigidbody2D rigid = takenObj.GetComponent<Rigidbody2D>();
			takenObj.GetComponent<SpriteRenderer> ().color = Color.black;
			rigid.isKinematic = false;
			rigid.AddForce (dir * 15f, ForceMode2D.Impulse);

			takenObj = null;

		}

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



	void SetGrounded() {

		Vector2 pt1 = transform.TransformPoint(box.offset + new Vector2(box.size.x / 2, -box.size.y / 2) + new Vector2(-.01f, 0));//(box.size / 2));
		Vector2 pt2 = transform.TransformPoint(box.offset - (box.size / 2) + new Vector2(.01f, 0));

		debugPts[0] = pt1;
		debugPts[1] = pt2;
		bool prevGrounded = grounded;
		grounded = Physics2D.OverlapArea(pt1, pt2, LayerMask.GetMask("Platform")) != null;
		//		Debug.Log (grounded); 

		if (grounded) {
			vel.y = Mathf.Max(0, vel.y);
			safety = true;
		}
	}



}


