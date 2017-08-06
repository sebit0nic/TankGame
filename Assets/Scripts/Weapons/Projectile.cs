using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public ParticleSystem[] thisParticleSystem;

	private IEnumerator particleCoroutine;
	private Rigidbody thisRigidbody;
	private Collider thisCollider;
	private GameObject thisRenderer;

	private void Awake() {
		thisRigidbody = GetComponent<Rigidbody> ();
		thisCollider = GetComponent<Collider> ();
		thisRenderer = transform.GetChild (0).gameObject;
	}

	public void OnProjectileDestroy() {
		particleCoroutine = WaitForParticleFinish (thisParticleSystem[0].main.duration);

		thisRenderer.SetActive (false);
		thisRigidbody.isKinematic = true;
		thisCollider.enabled = false;
		for (int i = 0; i < thisParticleSystem.Length; i++) {
			thisParticleSystem [i].Play ();
		}
		StartCoroutine (particleCoroutine);
	}

	private IEnumerator WaitForParticleFinish(float waitTime) {
		yield return new WaitForSeconds (waitTime);
		for (int i = 0; i < thisParticleSystem.Length; i++) {
			thisParticleSystem [i].Clear ();
		}
		thisRenderer.SetActive (true);
		thisRigidbody.isKinematic = true;
		gameObject.SetActive (false);
	}
}
