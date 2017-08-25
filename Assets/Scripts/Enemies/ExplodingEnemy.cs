using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExplodingEnemy : MonoBehaviour, Enemy {

	public float minExplosionDistance = 15;
	public float timeToExplode = 1;
	public BombExplosion bombExplosion;
	public ParticleSystem[] explosionParticles;
	public GameObject wheels;
	public GameObject warningRadius;

	private NavMeshAgent agent;
	private MeshRenderer thisMeshRenderer;
	private Rigidbody thisRigidbody;
	private BoxCollider thisBoxcollider;
	private SphereCollider thisSpherecollider;
	private Transform target;
	private float distance;
	private float explosionTimer;
	private bool explosionEngaged, canEngage = true;

	private void Start() {
		agent = GetComponent<NavMeshAgent> ();
		thisMeshRenderer = GetComponent<MeshRenderer> ();
		thisRigidbody = GetComponent<Rigidbody> ();
		thisBoxcollider = GetComponent<BoxCollider> ();
		thisSpherecollider = GetComponent<SphereCollider> ();
		target = GameObject.Find ("Player").transform;
	}

	private void Update() {
		if (!explosionEngaged) {
			agent.destination = target.position;
		}
			
		distance = Vector3.Distance (transform.position, target.position);
		if (explosionEngaged && explosionTimer <= Time.time) {
			bombExplosion.Explode ();
			DisableComponents ();
			transform.rotation = Quaternion.identity;

			for (int i = 0; i < explosionParticles.Length; i++) {
				explosionParticles [i].Play ();
			}

			explosionEngaged = false;
			StartCoroutine (ShowParticles ());
		}

		if (distance <= minExplosionDistance && !explosionEngaged && canEngage) {
			EngageExplosion ();
			canEngage = false;
		}
	}

	public void HitByProjectile() {
		if (canEngage) {
			EngageExplosion ();
			canEngage = false;
		}
	}

	private void EngageExplosion() {
		explosionEngaged = true;
		explosionTimer = Time.time + timeToExplode;
		if (agent.enabled) {
			agent.ResetPath ();
		}
		agent.enabled = false;

		thisRigidbody.isKinematic = false;
		thisRigidbody.useGravity = true;
		thisRigidbody.AddForce (transform.forward * 350);

		thisBoxcollider.enabled = false;
		thisSpherecollider.enabled = true;

		wheels.SetActive (false);
		warningRadius.SetActive (true);
	}

	private void DisableComponents() {
		thisMeshRenderer.enabled = false;
		this.enabled = false;
		warningRadius.SetActive (false);
	}

	private IEnumerator ShowParticles() {
		yield return new WaitForSeconds (explosionParticles [0].main.startLifetime.constant);
		gameObject.SetActive (false);
	}
}
