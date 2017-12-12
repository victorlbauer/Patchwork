using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof (SkeletonWalk))]
public class SkeletonController : MonoBehaviour {
	private SkeletonWalk skelWalk;
	//private SkeletonRadar skelRadar;
	private SkeletonAttack skelAttack;
	private Animator anim;

	private float walkTimer;
	private float idleTimer;
	private float walkingTime = 5.0f;
	private float idleTime = 3.0f;

	public SkelState states;

	void Start () {
		skelWalk = GetComponent<SkeletonWalk> ();
	//	skelRadar = GetComponentInChildren<SkeletonRadar> ();
		skelAttack = GetComponentInChildren<SkeletonAttack> ();
		anim = GetComponent<Animator> ();

		states.attacking = false;
		states.dead = false;
		states.hit = false;
		states.idle = false;
		states.spotted = false;
		states.walking = true;
	}

	void Update () {
		if (!skelAttack.canAttack) {
			walkTimer += Time.deltaTime;
			if (walkTimer >= walkingTime) {
				idleTimer += Time.deltaTime;
				states.walking = false;
				states.idle = true;
			}
			if (idleTimer >= idleTime) {
				walkTimer = 0;
				idleTimer = 0;
				states.idle = false;
				states.walking = true;
			}
			states.attacking = false;
		} else {
			states.attacking = true;
			states.idle = false;
			states.walking = false;
		}	

		Action ();
	}
		
	void Action() {
		Idle ();
		Walking ();
		Spotted ();
		Attacking ();
	}

	void Idle() {
		if (states.idle) {
			anim.SetBool ("Idle", true);
		} else {
			anim.SetBool ("Idle", false);
		}
	}

	void Walking() {
		if (states.walking) {
			skelWalk.Walk ();
			anim.SetBool ("Walking", true);
		} else {
			anim.SetBool ("Walking", false);
		}
	}

	void Spotted() {
		if (states.spotted) {
			anim.SetBool ("Spotted", true);
		} else {
			anim.SetBool ("Spotted", false);
		}
	}

	void Attacking() {
		if (states.attacking) {
			anim.SetBool ("Attacking", true);
		} else {
			anim.SetBool ("Attacking", false);
		}
	}

	void ResetState() {
		states.attacking = false;
		states.dead = false;
		states.hit = false;
		states.idle = false;
		states.spotted = false;
		states.walking = false;
	}

	public struct SkelState {
		public bool attacking;
		public bool dead;
		public bool hit;
		public bool idle;
		public bool spotted;
		public bool walking;

	}

}