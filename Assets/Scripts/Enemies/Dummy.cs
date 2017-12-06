using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour, Enemy {

	public bool moving;
	public float speed = 2.0f;
	public float minX, maxX;

	private DummyManager dummyManager;
	private bool dirRight = true;

	private void Awake() {
		dummyManager = transform.parent.parent.GetComponent<DummyManager> ();
	}

	private void Update() {
		if (moving) {
			if (dirRight) {
				transform.Translate (Vector2.right * speed * Time.deltaTime);
			} else {
				transform.Translate (-Vector2.right * speed * Time.deltaTime);
			}

			if (transform.localPosition.x >= maxX) {
				dirRight = false;
			}
			if (transform.localPosition.x <= minX) {
				dirRight = true;
			}
		}
	}

	public void HitByProjectile(int damage) {
		//TODO: play destroyed animation/particle effect
		dummyManager.NotifyDummyDestroyed();
		GameManager.GetInstance ().NotifyEnemyDestroyed (0);
		gameObject.SetActive (false);
	}
}
