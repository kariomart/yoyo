using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


	float scaleSpd;



	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody2D>();
		box = GetComponentInChildren<BoxCollider2D>();
		yoyoController = yoyo.GetComponent<YoyoController> ();
		//sprite = GetComponent<SpriteRenderer> ();
		defSprScale = sprite.localScale;
		debugPts = new Vector2[2];
		defaultScale = pivot.transform.localScale;//new Vector2 (sprite.BroadcastMessagetransform.localScale.x, transform.localScale.y); 

	}

	// Update is called once per frame
	void Update () {


		trigger = Input.GetAxis (rightTrigger) == 1;
		right = Input.GetAxis (leftStickH) > 0 || Input.GetKeyDown(KeyCode.RightArrow);
		left = Input.GetAxis (leftStickH) < 0 || Input.GetKeyDown(KeyCode.LeftArrow);

		//right = Input.GetKeyDown(KeyCode.RightArrow);
		//left = Input.GetKeyDown(KeyCode.LeftArrow);
	




		dir = new Vector2 (Input.GetAxis (leftStickH), Input.GetAxis (leftStickV));
		dir.Normalize ();

		dir1 = new Vector2 (Input.GetAxis (rightStickH), Input.GetAxis (rightStickV));
		if (dir1.magnitude > 1) dir1.Normalize ();


		if (Input.GetButtonDown(aButton) || Input.GetKeyDown(KeyCode.Space)) {
			jumpFlag = true;
		}


		if (Input.GetButtonDown (xButton) || Input.GetKeyDown(shootKey)) {

				ShootBullet ();

		}

		if (Input.GetButtonDown (yButton)) {

			Melee ();

		}


		if (Input.GetButtonDown ("start")) {
			//Time.timeScale = 1f;
			//Application.LoadLevel(Application.loadedLevel);
			//Yoyo();

		}
			
			


		if (health <= 0 && !gameOver) {

		}

	}

	private void FixedUpdate() {

		if (dir1 != Vector2.zero && !yoyoing) {

			Yoyo ();

		}

		//Debug.Log (grounded);


//		if (!spinning) {
//			pivot.transform.localScale = new Vector2 (defaultScale.x + (defaultScale.y - pivot.transform.localScale.y), pivot.transform.localScale.y + scaleSpd);
//		}


		SetGrounded();

		if (!grounded /*&& spinning*/) {
			//sprite.eulerAngles = new Vector3(0, 0, sprite.eulerAngles.z + ((spinSpd) * Time.fixedDeltaTime * spinDir));
			//sprite.localScale = new Vector3(defSprScale.x, defSprScale.y * .75f, 1f);
		} else {
			sprite.eulerAngles = Vector3.zero;
			//sprite.localScale = defSprScale;
		}


		if (jumpFlag && grounded) {
			if ((left || right)) {
				//spinning = true;

				//if (left) spinDir = 1;
				//if (right) spinDir = -1;
			}
			vel.y = jumpSpd;
		} else if (jumpFlag && !grounded) {
			safety = false;
		}


		float accel = runAccel;
		float mx = runMaxSpd;

		if (!grounded) {
			vel.y -= gravity * Time.fixedDeltaTime;
			accel = airAccel;
			mx = airMaxSpd;
		}

		if (vel.y > 0 && !Input.GetButton(aButton)) {
			vel.y -= unjumpBonusGrav * Time.fixedDeltaTime;
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
			vel.x = 0;
		}

		jumpFlag = false;
		shotTimer--;

		if (gameOver) {
			vel = Vector2.zero;
		}


		rb.MovePosition ((Vector2)transform.position + vel * Time.fixedDeltaTime);
		//bulletDir = Vector2.zero;


		//reticle.transform.position = new Vector2 (shootPt.transform.position.x + (dir.x * 1f), shootPt.transform.position.y + (dir.y * 1f));



	}

	void OnCollisionEnter2D(Collision2D coll) {


	}

	public void Yoyo() {

		yoyoing = true;
		yoyoController.SetVelo (dir1);
//		Debug.Log (dir1.magnitude);
		yoyoController.maxDistance = 5f * dir1.magnitude;
		yoyoController.maxSpeed = .5f * dir1.magnitude;
		yoyo.SetActive (true);
		//GameObject temp = Instantiate (yoyo, transform.position, Quaternion.identity);
		//temp.GetComponent<YoyoController> ().SetVelo (dir1);


	}


	public void Melee() {

	
		melee.GetComponent<MeleeController> ().meleeTimer = melee.GetComponent<MeleeController> ().meleeLength;

		if (dir == Vector2.zero) {
			melee.transform.position = (Vector2)this.transform.position + new Vector2 (.5f, 0);
		} else{
			melee.transform.position = (Vector2)this.transform.position + dir * .5f;
		}
	
		melee.transform.eulerAngles = new Vector3 (melee.transform.eulerAngles.x, melee.transform.eulerAngles.y, Geo.ToAng (dir)); 
		melee.SetActive (true);

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
		//grounded = Physics2D.OverlapArea(pt1, pt2, LayerMask.GetMask("Pinata")) != null;
		grounded = Physics2D.OverlapArea(pt1, pt2, LayerMask.GetMask("Platform")) != null;
		//		Debug.Log (grounded);

		if (grounded) {
			//spinning = false;
			vel.y = 0;
			safety = true;
			if (!prevGrounded) {
				scaleSpd = -.1f;
			}
		}
	}



}


