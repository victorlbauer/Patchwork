using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class SkeletonWalk : MonoBehaviour {
	public float moveSpeed = 1.0f;
	public float gravity = -9.8f;
	public LayerMask groundLayer;

	private float smooth = 1f;
	private Vector3 offset;
	private Vector3 targetAngles;
	private Vector3 velocity;
	private Controller2D controller;

	void Start () {
		controller = GetComponent<Controller2D> ();
		offset = new Vector3 (0.1f, 0.0f, 0.0f);
	}
		
	public void Walk() {
		if (controller.collisions.below) {
			velocity.y = 0;
		}

		if (!IsGrounded()) {
			Turn ();
		}

		velocity.x = moveSpeed * Time.deltaTime;
		velocity.y += gravity * Time.deltaTime;

		controller.Move (velocity, Vector2.zero);
	}

	private void Turn() {
		targetAngles = transform.eulerAngles + 180f * Vector3.up;
		transform.eulerAngles = Vector3.Lerp (transform.eulerAngles, targetAngles, smooth * Time.time);
	}

	private bool IsGrounded() {
		RaycastHit2D hit1 = Physics2D.Raycast (transform.position + offset, Vector2.down, 2f, groundLayer);
		RaycastHit2D hit2 = Physics2D.Raycast (transform.position - offset, Vector2.down, 2f, groundLayer);

		if (hit1.collider != null && hit2.collider != null) {
			return true;
		}
		return false;
	}
}