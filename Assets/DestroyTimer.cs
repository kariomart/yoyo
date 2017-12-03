using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour {

	public float timer;

	// Use this for initialization
	void Start () {

		Destroy (this.gameObject, timer);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
