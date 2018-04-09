using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour {

	public SpriteRenderer sprite;
	public GameObject checkpoint;
	public GameObject fx;
	public bool checkpointed;

	// Use this for initialization
	void Start () {

		sprite = GetComponent<SpriteRenderer> ();
		checkpoint = Master.me.startPos;

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll) {

		if ((coll.gameObject.tag == "Player" || coll.gameObject.tag == "yoyo") && !checkpointed) {

			//sprite.color = Color.red;
			checkpoint.transform.position = this.transform.position;
			Instantiate (fx, transform.position, Quaternion.identity);
			SoundController.me.PlaySound (Master.me.checkpoint, 1f);
			checkpointed = true;

		}

	}
}
