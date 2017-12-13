using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour {

	public static Master me;
	public GameObject player;
	public GameObject yoyo;
	public GameObject startPos;

	public PlayerMovement playerController;
	public YoyoController yoyoController;

	//public List<AudioClip> sounds = new List<AudioClip> ();
	public AudioClip[] sounds;

	public AudioClip enemy1;
	public AudioClip enemy2;
	public AudioClip enemy3;
	public AudioClip foot1;
	public AudioClip foot2;
	public AudioClip yoyoHit1; 
	public AudioClip yoyoHit2;
	public AudioClip yoyoThrow;
	public AudioClip yoyoWhoosh;

	// Use this for initialization
	void Awake () {

		if (me == null) {
			me = this;
		} else {
			Destroy (this);
		}

		//LoadAssets ();

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LoadAssets() {

//		Object[] boop = Resources.LoadAll("Audio", typeof(AudioClip));
//		audio = new AudioClip [boop.Length];
//
//		//audio = Resources.LoadAll ("Audio", typeof(AudioClip));
//		//audio = (AudioClip[])Resources.LoadAll("Audio", typeof(AudioClip));
//
//		for (int i = 0; i < audio.; i++) {
//
//			audio [i] = (AudioClip)boop [i];
//
//		}
//
//	}

		Object[] boop = Resources.LoadAll ("Audio", typeof(AudioClip));
		Debug.Log (boop.Length);
		sounds = new AudioClip[boop.Length];

		for (int i = 0; i < boop.Length; i++) {
			sounds [i] = (AudioClip)boop [i];
		}

		//SoundController.me.PlaySound ("enemy 1", 1);


	}


}
