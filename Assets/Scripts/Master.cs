using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Master : MonoBehaviour {

	public enum Sound {enemy1, enemy2, yoyoWhoosh};
	public static Master me;
	public GameObject player;
	public GameObject yoyo;
	public GameObject startPos;
	public GameObject enemy;

	public PlayerMovement playerController;
	public YoyoController yoyoController;

	//public List<AudioClip> sounds = new List<AudioClip> ();
	public AudioClip[] sounds;

	public GameObject[] spawners;

	public AudioClip enemy1;
	public AudioClip enemy2;
	public AudioClip enemy3;
	public AudioClip foot1;
	public AudioClip foot2;
	public AudioClip yoyoHit1; 
	public AudioClip yoyoHit2;
	public AudioClip yoyoThrow;
	public AudioClip yoyoWhoosh;
	public AudioClip glass;
	public AudioClip checkpoint;
	public AudioClip restart;
	public AudioClip fireworks;


	// Use this for initialization
	void Awake () {

		if (me == null) {
			me = this;
		} else {
			Destroy (this);
		}

		//LoadAssets ();
		GetEnemies();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LoadAssets() {

// //		Object[] boop = Resources.LoadAll("Audio", typeof(AudioClip));
// //		audio = new AudioClip [boop.Length];
// //
// //		//audio = Resources.LoadAll ("Audio", typeof(AudioClip));
// //		//audio = (AudioClip[])Resources.LoadAll("Audio", typeof(AudioClip));
// //
// //		for (int i = 0; i < audio.; i++) {
// //
// //			audio [i] = (AudioClip)boop [i];
// //
// //		}
// //
// //	}

// 		Object[] boop = Resources.LoadAll ("Audio", typeof(AudioClip));
// 		Debug.Log (boop.Length);
// 		sounds = new AudioClip[boop.Length];

// 		for (int i = 0; i < boop.Length; i++) {
// 			sounds [i] = (AudioClip)boop [i];
// 		}

		//SoundController.me.PlaySound ("enemy 1", 1);


	}

	void GetEnemies() {

		spawners = GameObject.FindGameObjectsWithTag ("Spawner");

	}

	public void gameOver() {
		SoundController.me.PlaySound (restart, 1f);
		playerController.reset();
		Master.me.respawnEnemies ();

		Instantiate (yoyoController.deadYoyo, transform.position, Quaternion.identity);
		yoyo.transform.position = player.transform.position;

	}

	public void respawnEnemies() {

		foreach (GameObject spawner in spawners) {

			if (spawner.transform.childCount > 0) {
				Destroy (spawner.transform.GetChild (0).gameObject);
			}
			GameObject temp = Instantiate (enemy, spawner.transform.position, Quaternion.identity); 
			temp.transform.parent = spawner.transform;

		}


	}


}
