using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour {
	public GameObject monsterRef;
	private GameObject monster;
	private MonsterStatus ms;

	public int quantity;
	public bool random;

	public float timer;
	public float cooldown;

	void Start () {
		quantity = 1;
		random = false;
		cooldown = 5.0f;
		timer = 0.0f;

		SpawnMonster ();
	}

	void Update () {
		if (ms.isDead) {
			timer += Time.deltaTime;
		}

		if (timer >= cooldown) {
			SpawnMonster ();
			timer = 0.0f;
		}
	}

	void SpawnMonster() {
		monster = Instantiate (monsterRef, transform.position, transform.rotation);
		ms = monster.GetComponent<MonsterStatus> ();
	}
}