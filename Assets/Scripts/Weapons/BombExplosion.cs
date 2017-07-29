using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour {

	public float explosionRadius = 10;
	public float expansionSpeed = 2.5f;

	private SphereCollider thisSphereCollider;
	private bool exploding;
	private float initialRadius;

	private void Awake() {
		thisSphereCollider = GetComponent<SphereCollider> ();
		initialRadius = thisSphereCollider.radius;
	}

	public void Explode() {
		exploding = true;
		thisSphereCollider.enabled = true;
		thisSphereCollider.radius = initialRadius;
	}

	private void Update() {
		if (exploding) {
			thisSphereCollider.radius += explosionRadius * (Time.deltaTime * expansionSpeed);
			if (thisSphereCollider.radius > explosionRadius) {
				exploding = false;
				thisSphereCollider.radius = explosionRadius;
				thisSphereCollider.enabled = false;
			}
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag.Equals ("Enemy")) {
			other.GetComponent<Enemy> ().HitByProjectile ();
		}
	}
}
