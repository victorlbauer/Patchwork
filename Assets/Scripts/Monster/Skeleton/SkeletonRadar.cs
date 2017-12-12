using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonRadar : MonoBehaviour {
	public bool spotted;

	void Start () {
		spotted = false;
	}
	
	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.name == "Player") {
			spotted = true;
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.gameObject.name == "Player") {
			spotted = false;
		}
	}
}