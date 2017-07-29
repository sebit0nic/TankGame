using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public GameObject projectileExplosion;

	private GameObject instantiatedExplosion;
	private ParticleSystem instantiatedParticleSystem;
	private IEnumerator particleCoroutine;
	private Rigidbody thisRigidbody;
	private Collider thisCollider;
	private Transform dynamicObjects;
	private GameObject thisRenderer;

	private void Awake() {
		dynamicObjects = GameObject.Find ("Dynamic Objects").GetComponent<Transform>();
		instantiatedExplosion = Instantiate (projectileExplosion, transform.position, projectileExplosion.transform.rotation) as GameObject;
		instantiatedExplosion.transform.parent = dynamicObjects;
		instantiatedParticleSystem = instantiatedExplosion.GetComponent<ParticleSystem> ();
		instantiatedExplosion.SetActive (false);
		thisRigidbody = GetComponent<Rigidbody> ();
		thisCollider = GetComponent<Collider> ();
		thisRenderer = transform.GetChild (0).gameObject;
	}

	public void OnProjectileDestroy() {
		particleCoroutine = WaitForParticleFinish (instantiatedParticleSystem.main.duration);

		thisRenderer.SetActive (false);
		thisRigidbody.isKinematic = true;
		thisCollider.enabled = false;
		instantiatedExplosion.transform.position = transform.position;
		instantiatedExplosion.SetActive (true);
		instantiatedParticleSystem.Play ();
		StartCoroutine (particleCoroutine);
	}

	private IEnumerator WaitForParticleFinish(float waitTime) {
		yield return new WaitForSeconds (waitTime);
		instantiatedExplosion.SetActive (false);
		instantiatedParticleSystem.Clear ();
		thisRenderer.SetActive (true);
		thisRigidbody.isKinematic = true;
		gameObject.SetActive (false);
	}
}
