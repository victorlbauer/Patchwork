using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttack : MonoBehaviour {
	public bool canAttack = false;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Player") {
			canAttack = true;
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.gameObject.tag == "Player") {
			canAttack = false;
		}
	}

}