using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Direction {left, right};

public class YoyoPlayer : MonoBehaviour {

	public Direction facing = Direction.right;
	float health = 1;
	public bool grounded = false;
	public Vector2 vel;
	public Vector2 throwDirection;

	void Start() {
		//vel = new Vector2(1, 1);
		//throwDirection = new Vector2(0, 0);
	}

	// Player Control
	void flipFace() {
		facing = (facing == Direction.left) ? Direction.right : Direction.left;
	}

	public void face(Direction f) {
		facing = f;
	}

	bool alive() {
		return health > 0;
	}

	public void stop() {
		this.vel = Vector2.zero;
	}

	public void ground() {
		vel.y = Mathf.Max(0, vel.y);
	}
}
