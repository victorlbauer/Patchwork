using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWalk : MonoBehaviour {
	public float walkingSpeed = 5.0f;

	private float timeWalking;
	private float totalTimeWalking = 2.0f;

	public bool movingRight;

	void Start () {
		movingRight = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		var rotation = transform.eulerAngles;
		timeWalking += Time.deltaTime;

		if (timeWalking >= totalTimeWalking) {
			timeWalking = 0;
			if (movingRight) {
				movingRight = false;
				rotation.y = 180;
			} else {
				rotation.y = 0;
				movingRight = true;
			}
			transform.rotation = Quaternion.Euler (rotation);
		}

		Walk ();
	}

	void Walk () {
		transform.Translate (Vector2.right * walkingSpeed * Time.deltaTime);
	}
}