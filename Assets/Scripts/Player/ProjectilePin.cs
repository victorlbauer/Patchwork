using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePin : MonoBehaviour {
	private float lifetime = 2.0f;
	private float speed = 20.0f;
	private float attack = 10.0f;
	public bool collided;

	private float timer;

	private RaycastHit hit;

	void Start() {
		collided = false;
	}

	void Update() {
		if (timer >= lifetime) {
			Destroy (gameObject);
		}

		if (!collided) {
			transform.Translate (Vector2.right * speed * Time.deltaTime);
			timer += Time.deltaTime;
		}
	}
		
	void OnTriggerEnter2D(Collider2D col) {
		if (!collided) {
			if (col.gameObject.tag == "Obstacle" || col.gameObject.tag == "Pinned") {
				collided = true;

				if (Physics.Raycast (transform.position, transform.forward, out hit)) {
					transform.position = hit.point;
				}

				transform.parent = col.transform;
			}
		}

		if (col.gameObject.tag == "Enemy") {
			Destroy (gameObject);
			col.GetComponent<MonsterStatus> ().Damage (attack);
		}
	}
}