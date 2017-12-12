using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedHitbox : MonoBehaviour {
	public float attack = 50.0f;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Enemy") {
			col.GetComponent<MonsterStatus> ().Damage (attack);
		}
	}
}