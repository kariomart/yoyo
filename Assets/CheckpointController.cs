using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour {

	public SpriteRenderer sprite;
	public GameObject checkpoint;

	// Use this for initialization
	void Start () {

		sprite = GetComponent<SpriteRenderer> ();
		checkpoint = Master.me.startPos;

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll) {

		if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "yoyo") {

			sprite.color = Color.red;
			checkpoint.transform.position = this.transform.position;

		}

	}
}
