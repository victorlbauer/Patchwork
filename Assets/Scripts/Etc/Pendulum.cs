using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour {
	public float angle = 40.0f;
	public float speed = .7f;
	private float direction;
	private bool goingRight;

	private Quaternion qStart;
	private Quaternion qEnd;

	void Start () {
		qStart = Quaternion.AngleAxis (angle, Vector3.forward);
		qEnd = Quaternion.AngleAxis (-angle, Vector3.forward);
	}

	void Update () {
		direction = transform.rotation.z;
		transform.rotation = Quaternion.Lerp (qStart, qEnd, (Mathf.Sin (Time.time * speed) + 1.0f) / 2.0f);

		goingRight = (transform.rotation.z > direction) ? true : false;
	}

	void OnTriggerStay2D(Collider2D col) {
		if (col.gameObject.tag == "Player") {
			if (goingRight) {
				col.GetComponent<PlayerMovement> ().velocity.x += speed * Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad);
			} else {
				col.GetComponent<PlayerMovement> ().velocity.x -= speed * Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad);
			}
		}
	}
}