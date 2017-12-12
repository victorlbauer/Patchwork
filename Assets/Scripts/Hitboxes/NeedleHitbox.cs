using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleHitbox : MonoBehaviour {
	public float attack = 20.0f;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Enemy") {
			col.GetComponent<MonsterStatus> ().Damage (attack);
		}
	}
}