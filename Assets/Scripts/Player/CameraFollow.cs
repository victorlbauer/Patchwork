using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	public Transform target;
	private Vector3 offset = new Vector3 (0, 0, -10);

	void LateUpdate () {
		transform.position = target.position + offset;
	}
}