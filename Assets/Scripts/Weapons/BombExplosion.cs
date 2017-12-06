using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour {

	public float explosionRadius = 10;
	public float expansionSpeed = 2.5f;
	public float fireballLength = 0.5f;
	public bool hurtsPlayer;
	public int damage = 10;

	private SphereCollider thisSphereCollider;
	private MeshRenderer thisMeshRenderer;
	private bool exploding;
	private float initialRadius;

	private void Awake() {
		thisSphereCollider = GetComponent<SphereCollider> ();
		thisMeshRenderer = GetComponent<MeshRenderer> ();
		initialRadius = thisSphereCollider.radius;
	}

	public void Explode() {
		exploding = true;
		thisSphereCollider.enabled = true;
		thisSphereCollider.radius = initialRadius;
		StartCoroutine (ShowFireball());
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
			other.GetComponent<Enemy> ().HitByProjectile (damage);
		}
		if (other.gameObject.tag.Equals ("Player") && hurtsPlayer) {
			other.GetComponent<PlayerHealth> ().DecreaseCurrentHealth (1);
		}
	}

	private IEnumerator ShowFireball() {
		thisMeshRenderer.enabled = true;
		yield return new WaitForSeconds (fireballLength);
		thisMeshRenderer.enabled = false;
	}
}
