using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerController : MonoBehaviour {

	public static PlayerController me;

	public DistanceJoint2D yoyoJoint1;
	public Rigidbody2D yoyo1;
	public DistanceJoint2D yoyoJoint2;
	public Rigidbody2D yoyo2;

	InputDevice dev;
	Vector2 lStickDir;
	Vector2 rStickDir;

	public float throwSpd;
	public float distanceIncrease;

	// Use this for initialization
	void Start () {

		me = this;

	}
	
	// Update is called once per frame
	void Update () {

		dev = InputManager.ActiveDevice;
		lStickDir = new Vector2 (dev.LeftStickX, dev.LeftStickY);
		rStickDir = new Vector2 (dev.RightStickX, dev.RightStickY);

	}

	void FixedUpdate() {

		NormalizeYoyoDir();
		Yoyo();

		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.LoadLevel (0);
		}
		
	}

	void Yoyo() {

//		Debug.Log(rStickDir * throwSpd);
		//yoyo1.MovePosition((Vector2)yoyo1.gameObject.transform.position + rStickDir * throwSpd);
		yoyo1.velocity = lStickDir;
		if ((yoyo1.gameObject.transform.position - transform.position).magnitude - yoyoJoint1.distance > -1f) {
			yoyoJoint1.distance += distanceIncrease;
		}
		//yoyo2.MovePosition((Vector2)yoyo2.gameObject.transform.position + lStickDir * throwSpd);
	}

	void NormalizeYoyoDir() {

		if (rStickDir.magnitude > 1) {
			rStickDir.Normalize ();
		}
		if (rStickDir.magnitude < .25f) {
		}
			rStickDir = Vector2.zero;

		if (lStickDir.magnitude > 1) {
			lStickDir.Normalize ();
		}
		if (lStickDir.magnitude < .25f) {
			lStickDir = Vector2.zero;
		}
	}
}
