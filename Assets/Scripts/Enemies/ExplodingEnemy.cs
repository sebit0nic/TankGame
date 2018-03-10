using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExplodingEnemy : MonoBehaviour, Enemy {
	
	public float maxHealth = 20;
	public float baseSpeed = 15;
	public int basePoints = 20;
	public float minExplosionDistance = 15;
	public float timeToExplode = 1;
	public BombExplosion bombExplosion;
	public ParticleSystem[] explosionParticles;
	public ParticleSystem fuseParticles;
	public GameObject wheels;
	public GameObject warningRadius;
	public MeshRenderer thisMeshRenderer;
	public Animator bodyAnimator;
	public Material normalMaterial, hitMaterial;
	public float hitMaterialWaitTime = 0.1f;

	private GameManager gameManager;
	private NavMeshAgent agent;
	private Rigidbody thisRigidbody;
	private BoxCollider thisBoxcollider;
	private SphereCollider thisSpherecollider;
	private Transform target;
	private float distance;
	private float explosionTimer;
	private bool explosionEngaged, canEngage = true, exploded, targetable = true;
	private bool firstInitiation = true;
	private float currentSpeed, currentHealth;

	private void Start() {
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager>();

		agent = GetComponent<NavMeshAgent> ();
		thisRigidbody = GetComponent<Rigidbody> ();
		thisBoxcollider = GetComponent<BoxCollider> ();
		thisSpherecollider = GetComponent<SphereCollider> ();
		target = GameObject.Find ("Player").transform;

		firstInitiation = false;
	}

	private void Update() {
		if (!exploded) {
			if (!explosionEngaged) {
				agent.destination = target.position;
			}

			distance = Vector3.Distance (transform.position, target.position);
			if (explosionEngaged && explosionTimer <= Time.time) {
				bombExplosion.Explode ();
				targetable = false;
				DisableComponents ();
				transform.rotation = Quaternion.identity;

				for (int i = 0; i < explosionParticles.Length; i++) {
					explosionParticles [i].Play ();
				}

				explosionEngaged = false;
				thisSpherecollider.enabled = false;
				thisRigidbody.isKinematic = true;
				thisRigidbody.useGravity = false;
				fuseParticles.Stop ();
				exploded = true;
				gameManager.NotifyEnemyDestroyed (basePoints);
				StartCoroutine (ShowParticles ());
			}

			if (distance <= minExplosionDistance && !explosionEngaged && canEngage) {
				EngageExplosion ();
				canEngage = false;
			}
		}
	}

	public void HitByProjectile(int damage) {
		currentHealth -= damage;
		currentHealth = Mathf.Clamp (currentHealth, 0, 1000);
		StartCoroutine (WaitForHitMaterial ());

		if (canEngage && currentHealth <= 0) {
			basePoints *= 2;
			EngageExplosion ();
			canEngage = false;
		}
		currentSpeed = ((1f - (currentHealth / maxHealth)) + 1f) * baseSpeed;
		agent.speed = currentSpeed;	
	}

	public bool IsTargetable() {
		return targetable;
	}

	private void EngageExplosion() {
		explosionEngaged = true;
		bodyAnimator.SetBool ("Engaged", true);
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
		warningRadius.SetActive (false);
	}

	private IEnumerator WaitForHitMaterial() {
		thisMeshRenderer.material = hitMaterial;
		yield return new WaitForSeconds (hitMaterialWaitTime);
		thisMeshRenderer.material = normalMaterial;
	}

	private IEnumerator ShowParticles() {
		yield return new WaitForSeconds (explosionParticles [0].main.startLifetime.constant);
		gameObject.SetActive (false);
	}

	private void OnEnable() {
		currentHealth = maxHealth;
		currentSpeed = baseSpeed;

		if (!firstInitiation) {
			canEngage = true;
			agent.enabled = true;

			thisBoxcollider.enabled = true;
			wheels.SetActive (true);

			thisMeshRenderer.enabled = true;
			this.enabled = true;
			warningRadius.SetActive (false);
			fuseParticles.Play ();
			basePoints = 10;

			exploded = false;
			targetable = true;
			bodyAnimator.SetBool ("Engaged", false);
		}
	}
}
