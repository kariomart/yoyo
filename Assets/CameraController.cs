using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player;
	public PlayerMovement playerController;
	public float smoothSpeed;
	public Vector2 offset;
	public float defSize;

	public Camera cam;


	// Use this for initialization
	void Start () {

		player = GameObject.Find ("Player");
		playerController = player.GetComponent<PlayerMovement> ();
		cam = GetComponent<Camera> ();
		cam.orthographicSize = defSize;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {

		Vector3 desiredPos = (Vector2)player.transform.position + offset;
		Vector3 smoothedPos = Vector2.Lerp (transform.position, desiredPos, smoothSpeed);
		transform.position = new Vector3 (smoothedPos.x, smoothedPos.y, -10f);

		if (playerController.vel.y > 0) {
			//cam.orthographicSize = defSize + (playerController.vel.y * .5f);
		}

	}
}
