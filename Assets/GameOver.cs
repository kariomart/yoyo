using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {

	public GameObject firework;
	public bool over;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D coll) {

		if (coll.gameObject.tag == "Player" && !over) {

			Fireworks ();
			over = true;	

		}


	}


	void Fireworks() {

		SoundController.me.PlaySound (Master.me.fireworks, 1f);
		for (int i = 0; i < Random.Range (5, 10); i++) {

			Instantiate(firework, new Vector3(transform.position.x + Random.Range(-3, 3), transform.position.y + Random.Range(-3, 3), transform.position.z), Quaternion.identity);


		}


	}
}
