using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingYoyos : MonoBehaviour {

	public Transform startPos;
	public LineRenderer rope;

	// Use this for initialization
	void Start () {

		startPos = transform.GetChild (0).transform;
		rope = GetComponent<LineRenderer> ();
		
	}
	
	// Update is called once per frame
	void Update () {

		rope.sortingOrder = -1;
		rope.SetPosition (0, startPos.position);
		rope.SetPosition (1, transform.position);
		
	}
}
