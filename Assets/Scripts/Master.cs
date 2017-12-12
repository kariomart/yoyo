using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour {

	public static Master me;
	public GameObject player;
	public PlayerMovement playerController;
	public GameObject yoyo;
	public YoyoController yoyoController;

	// Use this for initialization
	void Awake () {

		if (me == null) {
			me = this;
		} else {
			Destroy (this);
		}

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
