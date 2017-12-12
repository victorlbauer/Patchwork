using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescendingAltHitbox : MonoBehaviour {
	public float attack = 20.0f;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Enemy") {
			col.GetComponent<MonsterStatus> ().Damage (attack);

			GameObject player = GameObject.Find ("Player");
			player.GetComponent<PlayerMovement> ().velocity.y = 50.0f;
		}
	}
}