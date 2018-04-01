using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public enum YoyoState {held, thrown, returning, stopped};

public class NewYoyoController : MonoBehaviour {

	public AudioClip heldSound;
	public Transform player;
	InputDevice dev;

	DistanceJoint2D joint;
	Rigidbody2D rb;
	LineRenderer yoyoString;
	
	public string stick;

	public float throwSpd;
	public float stringLength;
	public Vector2 stickDir;
	public Vector2 throwDir;
	public float SLOP = 0.01f;

	public YoyoState state = YoyoState.held;

	// Use this for initialization
	void Start () {
		
		joint = GetComponent<DistanceJoint2D>();
		rb = GetComponent<Rigidbody2D>();
		yoyoString = GetComponent<LineRenderer>();
		rb.velocity = Vector2.zero;

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.LoadLevel(Application.loadedLevel);
		}

		dev = InputManager.ActiveDevice;
		if (stick == "left") {
			stickDir = new Vector2 (dev.LeftStickX, dev.LeftStickY);
		}
		else if (stick == "right") {
			stickDir = new Vector2 (dev.RightStickX, dev.RightStickY);
		}
		DrawString();

//		Debug.Log(stickDir.magnitude);
		
	}

	bool nearPlayer() {
		return joint.distance < 0.0 + SLOP;
		// return (transform.position - joint.connectedBody.transform.position).magnitude < 0.0 + SLOP;
	}

	bool abovePlayer() {
		return transform.position.y > player.position.y;
	}

	bool maxLength() {
		return joint.distance >= stringLength;
	}

	bool touchedJoystick() {
		return stickDir.magnitude >= .1f;
	}

	void FixedUpdate() {
		switch (state) {
			case YoyoState.held: 
				rb.velocity = Vector2.zero;
				transform.position = player.position;
				joint.enabled = false;
				if (stickDir.magnitude > .4f) {
					joint.enabled = true;
					throwDir = stickDir.normalized;
					state = YoyoState.thrown; 
					rb.velocity = throwDir;
				}
				return;
			case YoyoState.thrown: 
				joint.distance += throwSpd;
				if (maxLength()) state = YoyoState.returning;
				if (stickDir.magnitude > .4f) {
					throwDir = stickDir.normalized;
					state = YoyoState.thrown; 
					rb.velocity = throwDir;
				}
			break;

			case YoyoState.returning: 
				joint.distance -= throwSpd / 2.0f;
				if (nearPlayer() ) {
					state = abovePlayer() ? YoyoState.thrown : YoyoState.held;
					SoundController.me.PlaySound(heldSound, 1f);
				}
			break;

			case YoyoState.stopped:
				rb.velocity = Vector2.zero; 
				rb.isKinematic = true;
				if (touchedJoystick()) {
					state = YoyoState.returning;
					rb.isKinematic = false;
				}
				break;
			}
	}

	void DrawString() {

			Vector2 p1 = joint.connectedBody.transform.position;
			Vector2 p2 = transform.position;
			yoyoString.SetPosition (0, p1);
			yoyoString.SetPosition (1, p2);

	}

	void OnCollisionEnter2D(Collision2D coll) {
		// if player above yoyo, do nothing
		if (player.position.y > transform.position.y) return;

		// if yoyo not above platform, do nothing
		Bounds b = coll.gameObject.GetComponent<BoxCollider2D>().bounds;
		if (b.center.y + b.size.y / 2 > transform.position.y) return;

		// TODO - if yoyo above platform and player below platform

		// all else stop yoyo from moving
		state = YoyoState.stopped;

	}
}
