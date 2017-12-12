using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHook : MonoBehaviour {
	private bool returning;
	private bool collided;
	public bool isHooked;


	float speed = 30.0f;
	private float lifeTime = 0.3f;
	private float timer;

	public Transform player;

	void Start () {
		timer = 0.0f;
		returning = false;
		isHooked = false;
		collided = false;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if (isHooked) {
			return;
		}

		if (timer <= lifeTime && !collided) {
			transform.Translate (Vector2.right * speed * Time.deltaTime);
		} else {
			returning = true;
			Vector2 direction = new Vector2 (player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).normalized;
			float angle = Vector2.Angle (direction, Vector2.left);

			if (direction.y > 0) {
				angle = -angle;
			}

			transform.rotation = Quaternion.Euler (0, 0, angle);
			transform.Translate (Vector2.left * speed * Time.deltaTime);
		}
	}

	void SetPlayer(Transform target) {
		player = target;
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "HookPoint") {
			isHooked = true;
			GameObject player = GameObject.Find ("Player");
			PlayerAttack pa = player.GetComponent<PlayerAttack> ();
			PlayerMovement pm = player.GetComponent<PlayerMovement> ();
			pa.hooking = true;
			pm.state.hooking = true;
			pa.hookPos = col.gameObject.transform.position;
		}

		if (col.gameObject.tag == "Obstacle") {
			collided = true;
		}

		if (returning) {
			if (col.gameObject.tag == "Player") {
				Destroy (gameObject);
			}
		}
	}
}