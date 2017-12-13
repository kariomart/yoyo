using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public bool pulled;
	public bool taken;
	public bool thrown;
	public bool dead;
	public Vector2 thrownDir;

	public Transform yoyo;
	public GameObject player;
	public GameObject bullet;
	public PlayerMovement playerController;
	BoxCollider2D collider;
	Rigidbody2D rigid;
	SpriteRenderer sprite;
	public float bulletSpd;
	public float shootingDistance;
	public int shootFreq;

	public BoxCollider2D shield;
	public GameObject enemyDead;
	// Use this for initialization
	void Start () {
			
		yoyo = Master.me.yoyo.transform;
		player = Master.me.player;
		playerController = Master.me.playerController;
		collider = GetComponent<BoxCollider2D> ();
		rigid = GetComponent<Rigidbody2D> ();
		sprite = GetComponent<SpriteRenderer> ();
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

		} 

		else if (pulled && dis < 1){
			
			pulled = false;
			taken = true;
			collider.enabled = false;
		}

		if (taken) {

			transform.position = (Vector2)player.transform.position + playerController.untouchedLDir.normalized;
			//playerController.takenObj = this.gameObject;
			shield.enabled = true;

		}

		if (thrown) {
			
			pulled = false;
			taken = false;
			thrownDir.y -= 0.02f;

			rigid.MovePosition ((Vector2)transform.position + thrownDir * .5f);

		}




	}

	void Shoot() {

		if (!pulled || taken || thrown) {
			GameObject tempBullet = Instantiate (bullet, transform.position, Quaternion.identity);
			BulletController bulletController = tempBullet.GetComponent<BulletController> ();

			bulletController.vel = (player.transform.position - transform.position).normalized * bulletSpd;
		}


	}

	void OnCollisionEnter2D(Collision2D coll) {

		//Debug.Log (coll.gameObject.name + " " + coll.gameObject.tag);

		if (coll.gameObject.tag != "Stage" && coll.gameObject.name != "Player") {

			//Destroy (this.gameObject);

		}

		if (coll.gameObject.tag == "Enemy") {

			Instantiate (enemyDead, transform.position, Quaternion.identity);
			Destroy (this.gameObject);
			Destroy(coll.gameObject);


		}

		if (coll.gameObject.tag == "yoyo" && dead) {
			
			thrownDir = Master.me.yoyoController.vel * .05f;


		}
			


	}

	public void Pulled() {




	}
}
