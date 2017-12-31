using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombShot : MonoBehaviour {

	private Projectile thisProjectile;
	private Rigidbody thisRigidbody;
	private SphereCollider thisCollider;
	private BombExplosion bombExplosion;

	private void Awake() {
		thisProjectile = GetComponent<Projectile> ();
		thisRigidbody = GetComponent<Rigidbody> ();
		thisCollider = GetComponent<SphereCollider> ();
		bombExplosion = GetComponentInChildren<BombExplosion> ();
	}

	public void Init(Transform position, float shootForce) {
		this.transform.position = position.position;
		thisRigidbody.isKinematic = false;
		thisRigidbody.AddForce (position.forward * shootForce);
		thisCollider.enabled = true;
	}

	public void Explode() {
		thisProjectile.OnProjectileDestroy ();
		bombExplosion.Explode ();
	}
}
