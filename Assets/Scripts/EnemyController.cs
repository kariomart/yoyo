using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public bool pulled;
	public bool taken;
	public Transform yoyo;
	public GameObject player;
	public GameObject bullet;
	public PlayerMovement playerController;
	BoxCollider2D collider;
	Rigidbody2D rigid;
	public float bulletSpd;
	public float shootingDistance;

	public BoxCollider2D shield;
	// Use this for initialization
	void Start () {
			
		//yoyo = GameObject.Find ("yoyo").transform;
		player = GameObject.Find ("Player");
		playerController = player.GetComponent<PlayerMovement> ();
		collider = GetComponent<BoxCollider2D> ();
		rigid = GetComponent<Rigidbody2D> ();

	}
	
	// Update is called once per frame
	void Update () {
		int rand = Random.Range (0, 150);

		//Debug.Log (Vector2.Distance (transform.position, player.transform.position));
		if (rand == 1 && !taken && Vector2.Distance(transform.position, player.transform.position) < shootingDistance) {

			Shoot ();

		}

		
	}

	void FixedUpdate() {
		float dis = Vector2.Distance (player.transform.position, transform.position);

		if (pulled && dis > 1) {

			transform.position = yoyo.position;
		} 

		else if (pulled && dis < 1){
			
			pulled = false;
			taken = true;
		}

		if (taken) {

			transform.position = (Vector2)player.transform.position + playerController.dir.normalized;
			playerController.takenObj = this.gameObject;
			collider.enabled = false;
			rigid.isKinematic = true;
			shield.enabled = true;

		}


	}

	void Shoot() {

		GameObject tempBullet = Instantiate (bullet, transform.position, Quaternion.identity);
		BulletController bulletController = tempBullet.GetComponent<BulletController> ();

		bulletController.vel = (player.transform.position - transform.position).normalized * bulletSpd;


	}

	void OnCollisionEnter2D(Collision2D coll) {

		//Debug.Log (coll.gameObject.name + " " + coll.gameObject.tag);

		if (coll.gameObject.tag != "Stage" && coll.gameObject.name != "Player") {

			//Destroy (this.gameObject);

		}
			


	}

	public void Pulled() {




	}
}
