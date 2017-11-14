using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour {

	public float meleeTimer;
	public float meleeLength;
	public SpriteRenderer sprite;

	// Use this for initialization
	void Start () {

		sprite = GetComponent<SpriteRenderer> ();
		
	}

	void Awake() {
		
		meleeTimer = meleeLength;


	}
	
	// Update is called once per frame
	void Update () {

		if (meleeTimer > 0) {

			sprite.enabled = true;
			meleeTimer -= Time.deltaTime;


		} else {

			sprite.enabled = false;
			this.gameObject.SetActive (false);
		}
		
	}

	public void OnTriggerEnter2D(Collider2D coll) {


		if (coll.gameObject.tag == "Enemy") {

			Destroy (coll.gameObject);

		}

	}


}
