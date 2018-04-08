using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour {

	public Transform player;
	public float speed;
	public float startPos;
	public float endPos;
	public float maxHeight;
	public float minHeight;

	// Use this for initialization
	void Start () {

		player = Master.me.player.transform;
		speed = Random.Range(.5f, 2f) / 100;
		transform.position = new Vector2(Random.Range(player.position.x, startPos), player.position.y + Random.Range(minHeight, maxHeight));
		
	}
	
	// Update is called once per frame
	void Update () {

		transform.position = new Vector2(transform.position.x - speed, transform.position.y);

		if (transform.position.x < endPos) {
			transform.position = new Vector2(startPos, player.position.y + Random.Range(minHeight, maxHeight));
		}

		
	}
}
