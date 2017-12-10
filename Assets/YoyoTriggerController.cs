using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoyoTriggerController : MonoBehaviour {

	public YoyoController yoyo;
	public LineRenderer yoyoString;
	public GameObject fakeYoyo;

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll) {

		fakeYoyo.SetActive (true);
		Destroy (this.gameObject);
		//yoyo.beingHeld = true;
		//yoyo.gameObject.transform = 
		//yoyo.enabled = true;

	}
}
