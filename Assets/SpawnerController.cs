using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour {

	public GameObject enemy;
	public Vector3 defaultPos;

	// Use this for initialization
	void Start () {

		enemy = transform.GetChild(0).gameObject;
		defaultPos = enemy.transform.position;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
