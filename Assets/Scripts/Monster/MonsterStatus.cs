using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus : MonoBehaviour {
	public float HP;
	public float Attack;
	public bool isDead;

	void Start() {
		isDead = false;
	}

	public void Damage(float damage) {
		HP -= damage;
		if (HP <= 0.0) {
			isDead = true;
			Destroy (gameObject);
		}
	}
}