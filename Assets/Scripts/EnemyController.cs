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
	public int shootFreq;

	public BoxCollider2D shield;
	// Use this for initialization
	void Start () {
			
		yoyo = Master.me.yoyo.transform;
		player = Master.me.player;
		playerController = Master.me.playerController;
		collider = GetComponent<BoxCollider2D> ();
		rigid = GetComponent<Rigidbody2D> ();

	}
	
	// Update is called once per frame
	void Update () {
		int rand = Random.Range (0, shootFreq);

		//Debug.Log (Vector2.Distance (transform.position, player.transform.position));
		if (rand == 1 && !taken && Vector2.Distance(transform.position, player.transform.position) < shootingDistance && !taken) {

			Shoot ();

		}

		
	}

	void FixedUpdate() {
		
		float dis = Vector2.Distance (player.transform.position, transform.position);

		if (pulled && dis > 1) {

			transform.position = yoyo.position;
			collider.enabled = false;
			rigid.isKinematic = true;
			rigid.gravityScale = 0f;

		} 

		else if (pulled && dis < 1){
			
			pulled = false;
			taken = true;
			}

		if (taken) {

			transform.position = (Vector2)player.transform.position + playerController.dir.normalized;
			playerController.takenObj = this.gameObject;
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

		if (coll.gameObject.tag == "Enemy") {

			Destroy (this.gameObject);
			Destroy(coll.gameObject);

		}
			


	}

	public void Pulled() {




	}
}
