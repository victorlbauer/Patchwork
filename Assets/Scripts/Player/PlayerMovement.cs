using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class PlayerMovement : MonoBehaviour {
	public float maxJumpHeight = 3.0f;
	public float minJumpHeight = 1.0f;
	public float timeToJumpApex = 0.3f;
	public float moveSpeed = 6.0f;
	public float climbSpeed = 3.0f;
	public float springSpeed = 50.0f;

	private float accelerationTimeAirborne = 0.2f;
	private float accelerationTimeGrounded = 0.1f;

	public float gravity;
	private float maxJumpVelocity;
	private float minJumpVelocity;
	public float velocityXSmoothing;
	public Vector3 velocity;

	private SpriteRenderer sr;
	private Controller2D controller;

	public PlayerState state;
	private Vector2 input;

	void Start () {
		controller = GetComponent<Controller2D> ();
		sr = GetComponent<SpriteRenderer> ();

		state.movingRight = true;
		state.movable = true;
		state.climbing = false;

		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
	}
	
	void Update () {
		//Velocidade vertical zero se detectar colisão abaixo
		//Mudar o collision above para caso de pulo
		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

		if (controller.collisions.below) {
			state.jumping = false;
		}

		if (state.movable) {
			input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		}

		// Pulo alto
		if (Input.GetKeyDown (KeyCode.Space) && controller.collisions.below) {
			velocity.y = maxJumpVelocity;
		}
		// Pulo curto
		if (Input.GetKeyUp (KeyCode.Space)) {
			if (velocity.y > minJumpVelocity) {
				velocity.y = minJumpVelocity;
			}
		}
		// Pulando
		if (velocity.y != 0 && !state.climbing) {
			state.jumping = true;
		}

		if (state.spring && state.jumping) {
			velocity.y = springSpeed;
		}

		// Direção do movimento
		float targetVelocityX = input.x * moveSpeed;
		if (state.movable) {
			velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		}

		if (state.hooking) {
			velocity.x = 0;
			velocity.y = 0;
		} else {
			velocity.y += gravity * Time.deltaTime;
		}
		

		if (velocity.x < 0) {
			sr.flipX = true;
			state.movingRight = false;
		} else if (velocity.x > 0) {
			sr.flipX = false;
			state.movingRight = true;
		}

		if (state.climbing) {
			velocity.x = velocity.x/1.1f;
			Climb (input);
			controller.Move (velocity * Time.deltaTime, input);
		} else {
			controller.Move (velocity * Time.deltaTime, input);
		}

	}

	void Climb(Vector2 input) {
		if (input.y > 0.0) {
			velocity.y = climbSpeed;
		} else if (input.y < 0.0) {
			velocity.y = -climbSpeed;
		} else {
			velocity.y = 0.0f;
		}
	}

	public struct PlayerState {
		public bool movable;
		public bool movingRight;
		public bool jumping;
		public bool climbing;
		public bool spring;
		public bool hooking;
	}

	// Mudar para oncollisonenter / controller.move
	void OnTriggerStay2D(Collider2D col) {
		if (col.gameObject.tag == "Climb") {
			Vector2 input = new Vector2 (0, Input.GetAxisRaw ("Vertical"));
			if (input.y > 0) {
				state.climbing = true;
			} else if (input.y < 0) {
				if (controller.collisions.below) {
					state.climbing = false;
				} else {
					state.climbing = true;
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.gameObject.tag == "Climb") {
			state.climbing = false;
		}
	}
}