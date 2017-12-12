using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxManager : MonoBehaviour {
	public PolygonCollider2D current_hitbox;
	public PolygonCollider2D [] hitboxes;
	public int current_attack;
	public bool attacking;
	private int i;

	void Start () {
		current_hitbox = null;
		hitboxes = GetComponentsInChildren<PolygonCollider2D> ();
		current_attack = 0;
		attacking = false;
		i = 0;

		foreach (PolygonCollider2D poly in hitboxes) {
			poly.enabled = false;
		}

	}

	void Update() {
		if (attacking) {
			i = 0;
			foreach (PolygonCollider2D poly in hitboxes) {
				i++;
				if (current_attack == i) {
					current_hitbox = hitboxes [i - 1];
					poly.enabled = true;
				} else {
					poly.enabled = false;
				}
			}
		} else {
			foreach (PolygonCollider2D poly in hitboxes) {
				current_hitbox = null;
				poly.enabled = false;
			}
		}
	}
}