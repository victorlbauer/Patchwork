using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.name == "Player") {
			col.GetComponent<PlayerMovement> ().state.spring = true;
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.gameObject.name == "Player") {
			col.GetComponent<PlayerMovement> ().state.spring = false;
		}
	}
}