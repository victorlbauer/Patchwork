using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinHitbox : MonoBehaviour {
	public float attack = 10.0f;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Enemy") {
			col.GetComponent<MonsterStatus> ().Damage (attack);
		}
	}
}