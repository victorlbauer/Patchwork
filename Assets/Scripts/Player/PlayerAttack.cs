using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
	private int equippedWeapon;
	private float timer;
	private bool defending;
	private bool descendingAttack;
	private bool animating;
	private bool rocketing;

	public bool hooking;
	public Vector3 hookPos;
	private float hookSpeed = 10.0f;

	public GameObject pin;
	public GameObject needle;
	private const int totalPins = 3;
	private int pinIndex = 0;
	private GameObject[] pinsList;

	private GameObject hook;

	private float dashSpeed = 30.0f;

	private Animator anim;
	private PlayerMovement movement;
	private HitboxManager hitboxManager;

	void Start() {
		equippedWeapon = 0;
		defending = false;
		descendingAttack = false;
		animating = false;
		hooking = false;
		rocketing = false;

		pinsList = new GameObject[totalPins];

		anim = GetComponent<Animator> ();
		movement = GetComponent<PlayerMovement> ();
		hitboxManager = GetComponentInChildren<HitboxManager>();
	}

	void Update () {
		if (hooking) {
			movement.state.movable = false;
			transform.Translate ((hookPos - transform.position) * hookSpeed * Time.deltaTime);
			if ((hookPos.x - transform.position.x) < 0.1 && (hookPos.y - transform.position.y) < 0.1) {
				hooking = false;
				movement.state.hooking = false;
			} else {
				return;
			}
		}

		FlipHitboxes();
		EquipWeapon();

		if (!movement.state.climbing) {
			RocketAttack ();
			PlayerInput ();
		}
	}

	void PlayerInput() {
		CheckHitbox ();
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("charged_attack")) {
			return;
		} else {
			movement.state.movable = true;
		}
			
		if (!movement.state.jumping) {
			descendingAttack = false;
			if (animating) {
				CurrentWeapon();
			  	animating = false;
			}
		}

		if (Input.GetKey(KeyCode.LeftShift) && !movement.state.jumping) {
			Defend ();
		}

		if (Input.GetKeyUp(KeyCode.LeftShift) && equippedWeapon != 2) {
			CurrentWeapon ();
			defending = false;
			movement.state.movable = true;
		}

		if (Input.GetKeyDown(KeyCode.LeftShift) && movement.state.jumping) {
			DescendingAttacK ();
		}

		if (!descendingAttack) {
			if (Input.GetMouseButtonDown (0)) {
				Attack ();
			}
			if (Input.GetMouseButtonDown (1)) {
				RightClick ();
			}

			if (Input.GetKeyDown (KeyCode.LeftShift)) {
				LeftShift ();
			}
		}
	}

	void CheckHitbox() {
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("idle_pin_shield") ||
			anim.GetCurrentAnimatorStateInfo (0).IsName ("idle_needle") ||
			anim.GetCurrentAnimatorStateInfo (0).IsName ("idle_scissors") ||
			anim.GetCurrentAnimatorStateInfo (0).IsName ("idle")) {

			hitboxManager.current_attack = 0;
			hitboxManager.attacking = false;
		}
	}

	void FlipHitboxes() {
		var rotation = transform.rotation.eulerAngles;
		if (!movement.state.movingRight) {
			rotation.y = 180;
		} else {
			rotation.y = 0;
		}
			
		var hitboxes = this.gameObject.transform.GetChild (0);
		hitboxes.transform.rotation = Quaternion.Euler (rotation);
	}

	void EquipWeapon() {
		defending = false;
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			if (equippedWeapon == 1) {
				Debug.Log ("Pin and Button already equipped");
			} else {
				equippedWeapon = 1;
				anim.Play ("idle_pin_shield", -1);
			}
		}

		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			if (equippedWeapon == 2) {
				Debug.Log ("Needle already equipped");
			} else {
				equippedWeapon = 2;
				anim.Play ("idle_needle", -1);
			}
		}

		if(Input.GetKeyDown(KeyCode.Alpha3)) {
			if (equippedWeapon == 3) {
				Debug.Log ("Scissors already equipped");
			} else {
				equippedWeapon = 3;
				anim.Play ("idle_scissors", -1);
			}
		}

		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			if (equippedWeapon == 0) {
				Debug.Log ("I still have no weapons...");
			} else {
				equippedWeapon = 0;
				anim.Play ("idle", -1);
			}
		}

		if (Input.GetKeyDown (KeyCode.Alpha5)) {
			equippedWeapon = 4;
			anim.Play ("descending_attack_alt", -1);
		}

		if (Input.GetKeyDown (KeyCode.Alpha6)) {
			equippedWeapon = 5;
			anim.Play ("idle_alt", -1);
		}

		if (Input.GetKeyDown (KeyCode.Alpha7)) {
			equippedWeapon = 6;
			anim.Play ("idle_alt2", -1);
		}
	}

	void CurrentWeapon() {
		if (equippedWeapon == 1) {
			anim.Play ("idle_pin_shield", -1);
		}
		if (equippedWeapon == 2) {
			anim.Play ("idle_needle", -1);
		}
		if (equippedWeapon == 3) {
			anim.Play ("idle_scissors", -1);
		}
		if (equippedWeapon == 4) {
			anim.Play ("descending_attack_alt", -1);
		}
		if (equippedWeapon == 5) {
			anim.Play ("idle_alt", -1);
		}
		if (equippedWeapon == 6) {
			anim.Play ("idle_alt2", -1);
		}
	}

	void DescendingAttacK() {
		if ((equippedWeapon == 2 || equippedWeapon == 4) && !animating) {
			descendingAttack = true;
			animating = true;

			//movement.velocity.y = 0;
			movement.velocity.y += movement.gravity / 4;
			if (equippedWeapon == 2) {
				anim.Play ("descending_attack", -1);
				hitboxManager.current_attack = 5;
			} else {
				hitboxManager.current_attack = 7;
			}
			hitboxManager.attacking = true;
		}
	}

	void LeftShift() {
		if (equippedWeapon == 3) {
			if (movement.state.movingRight) {
				movement.velocity.x = dashSpeed;
			} else {
				movement.velocity.x = -dashSpeed;
			}
			anim.Play ("dash_attack", -1);
		}
	}


	void RightClick() {
		if (equippedWeapon == 1 && !defending) {
			ThrowPin ();
		}
		if (equippedWeapon == 2) {
			Hook ();
		}
		if (equippedWeapon == 3) {
			ChargedAttack ();
		}
		if (equippedWeapon == 5) {
			rocketing = true;
		}
	}

	void RocketAttack() {
		if (rocketing) {
			float initSpeed = .1f;
			float midSpeed = 3.0f;
			float finalSpeed = 50.0f;

			anim.Play ("attack_needle_alt");
			timer += Time.deltaTime;

			if (!movement.state.movingRight) {
				initSpeed = -initSpeed;
				midSpeed = -midSpeed;
				finalSpeed = -finalSpeed;
			}

			if (timer <= 1.0) {
				movement.velocity.x += initSpeed;
			} 
			if (timer > 1.0 && timer <= 1.5) {
				movement.velocity.x += midSpeed;
			}
			if(timer > 1.5) {
				movement.velocity.x += finalSpeed;
				rocketing = false;
				timer = 0.0f;
			}
		}
	}

	void ThrowPin() {
		pinIndex = pinIndex % totalPins;
		Destroy (pinsList [pinIndex]);

		var rotation = transform.rotation.eulerAngles;
		if (!movement.state.movingRight) {
			rotation.y = 180;
		} else {
			rotation.y = 0;
		}

		GameObject pinInstance = Instantiate (pin, transform.position, Quaternion.Euler(rotation));

		pinsList [pinIndex] = pinInstance;
		pinIndex++;
		anim.Play ("throw_pin", -1);
	}

	void Hook() {
		Destroy (hook);
		hooking = false;

		hook = Instantiate (needle, transform.position, transform.rotation);

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		Vector2 direction = new Vector2 (ray.direction.x, ray.direction.y);

		float angle = Vector2.Angle (direction, new Vector2 (1, 0));
		if (direction.y < 0) {
			angle = -angle;
		}

		hook.transform.rotation = Quaternion.Euler (0, 0, angle);
		ProjectileHook playerRef = hook.GetComponent<ProjectileHook> ();

		playerRef.player = transform;
	}

	void ChargedAttack() {
		if (!movement.state.jumping) {
			anim.Play ("charged_attack", -1);
			movement.state.movable = false;
			movement.velocity.x = 0;
			hitboxManager.current_attack = 6;
			hitboxManager.attacking = true;
		}
	}

	void Attack() {
		if (!defending) {
			if (equippedWeapon == 0) {
				Debug.Log ("I have nothing to attack with!");
			}
			if (equippedWeapon == 1) {
				anim.Play ("attack_pin_shield", -1);
				hitboxManager.current_attack = 3;
				hitboxManager.attacking = true;
			}
			if (equippedWeapon == 2) {
				anim.Play ("attack_needle", -1);
				hitboxManager.current_attack = 1;
				hitboxManager.attacking = true;
			}
			if (equippedWeapon == 3) {
				anim.Play ("attack_scissors", -1);
				hitboxManager.current_attack = 2;
				hitboxManager.attacking = true;
			}
		} else {
			Debug.Log ("Can't attack while defending...");
		}
	}

	void Defend() {
		if (equippedWeapon == 1) {
			defending = true;
			movement.state.movable = false;
			movement.velocity.x = 0;
			anim.Play ("defending", -1);
			hitboxManager.current_attack = 4;
			hitboxManager.attacking = true;
		}
	}
}