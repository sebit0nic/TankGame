using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombShot : MonoBehaviour {

	public float countdown;

	private Projectile thisProjectile;
	private Rigidbody thisRigidbody;
	private SphereCollider thisCollider;
	private BombExplosion bombExplosion;
	private bool countdownActive;
	private float countdownTimer;

	private void Awake() {
		thisProjectile = GetComponent<Projectile> ();
		thisRigidbody = GetComponent<Rigidbody> ();
		thisCollider = GetComponent<SphereCollider> ();
		bombExplosion = GetComponentInChildren<BombExplosion> ();
	}

	public void Init(Vector3 position, Vector3 velocity) {
		transform.position = position;
		thisRigidbody.isKinematic = false;
		thisRigidbody.velocity = velocity;
		thisCollider.enabled = true;
	}

	private void Update() {
		if (countdownActive && countdownTimer < Time.time) {
			thisProjectile.OnProjectileDestroy ();
			countdownActive = false;
			bombExplosion.Explode ();
		}
	}

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag.Equals ("Enemy") || other.gameObject.tag.Equals ("Environment")) {
			thisRigidbody.velocity /= 15;
			countdownActive = true;
			countdownTimer = Time.time + countdown;
		}
	}
}
