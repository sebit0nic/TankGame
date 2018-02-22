using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RotatingShootingEnemy : MonoBehaviour, Enemy {

	public GameObject[] barrels;
	public GameObject body;
	public Transform[] barrelEnds;
	public int basePoints = 20;
	public int maxHealth = 50;
	public float minStopDistance = 15;
	public float minMoveAwayDistance = 7;
	public ParticleSystem[] thisParticleSystem;
	public TrailRenderer[] trails;
	public Material normalMaterial, hitMaterial;
	public float hitMaterialWaitTime = 0.5f;

	[Header("Shooting Properties")]
	public Rigidbody projectile;
	public float minShootDistance = 20;
	public int shootCount = 5;
	public float shootInterval = 1;
	public float projectileSpeed = 10;
	public float barrelRotationSpeed = 10;
	public float shootPause = 5;

	private NavMeshAgent agent;
	public float distance;
	private int currentShootCount;
	private float currentShootInterval;
	private int currentHealth;

	private Transform target;
	private ObjectPool objectPool;

	private IEnumerator particleCoroutine;
	private Rigidbody thisRigidbody;
	private Collider thisCollider;
	private MeshRenderer[] barrelRenderer;
	private MeshRenderer bodyRenderer;
	private bool rotatingShooting, pausing, targetable = true;

	private enum ActorState {ACTOR_FOLLOW, ACTOR_STOP};
	private ActorState currentActorState = ActorState.ACTOR_FOLLOW;

	private void Awake() {
		target = GameObject.Find ("Player").transform;
		objectPool = GetComponent<ObjectPool> ();

		thisRigidbody = GetComponent<Rigidbody> ();
		thisCollider = GetComponent<Collider> ();

		barrelRenderer = new MeshRenderer[barrels.Length];
		for (int i = 0; i < barrels.Length; i++) {
			barrelRenderer [i] = barrels [i].GetComponent<MeshRenderer> ();
		}
		bodyRenderer = body.GetComponent<MeshRenderer> ();
	}

	private void Start() {
		agent = GetComponent<NavMeshAgent> ();
	}

	private void Update() {
		Vector3 rawTargetPosition = new Vector3 (target.position.x, transform.position.y, target.position.z);
		distance = Vector3.Distance (transform.position, rawTargetPosition);

		if ((distance < minShootDistance || rotatingShooting) && !pausing) {
			rotatingShooting = true;
			currentActorState = ActorState.ACTOR_STOP;
		} else {
			if (distance > minStopDistance) {
				currentActorState = ActorState.ACTOR_FOLLOW;
			} else {
				currentActorState = ActorState.ACTOR_STOP;
			}
		}

		EvaluateActorState (rawTargetPosition);

		if (rotatingShooting) {
			for (int i = 0; i < barrels.Length; i++) {
				barrels [i].transform.Rotate (0, 0, barrelRotationSpeed * Time.deltaTime);
			}

			if (currentShootCount < shootCount) {
				if (Time.time > currentShootInterval) {
					for (int i = 0; i < barrelEnds.Length; i++) {
						GameObject newProjectile = objectPool.GetPooledObjects ();
						newProjectile.SetActive (true);
						newProjectile.GetComponent<NormalMissileCollision> ().Init (barrelEnds [i].position, barrelEnds [i].rotation, barrelEnds [i].transform.forward * projectileSpeed, ForceMode.Force);
					}
					currentShootInterval = Time.time + shootInterval;
					currentShootCount++;
				}
			} else {
				currentShootCount = 0;
				pausing = true;
				rotatingShooting = false;
				StartCoroutine (WaitForShootPause ());
			}
		}
	}

	public void HitByProjectile(int damage) {
		currentHealth -= damage;
		StartCoroutine (WaitForHitMaterial ());

		if (currentHealth <= 0) {
			GameManager.GetInstance ().NotifyEnemyDestroyed (basePoints);

			particleCoroutine = WaitForParticleFinish (thisParticleSystem[0].main.duration);

			this.enabled = false;
			thisRigidbody.isKinematic = true;
			thisCollider.enabled = false;
			body.SetActive (false);
			for (int i = 0; i < barrels.Length; i++) {
				barrels [i].gameObject.SetActive (false);
			}
			if (trails.Length > 0) {
				trails [0].enabled = false;
				trails [1].enabled = false;
			}

			for (int i = 0; i < thisParticleSystem.Length; i++) {
				thisParticleSystem [i].Play ();
			}
			targetable = false;
			StartCoroutine (particleCoroutine);
		}
	}

	public bool IsTargetable() {
		return targetable;
	}

	private IEnumerator WaitForParticleFinish(float waitTime) {
		yield return new WaitForSeconds (waitTime);
		for (int i = 0; i < thisParticleSystem.Length; i++) {
			thisParticleSystem [i].Clear ();
		}

		thisRigidbody.isKinematic = false;
		thisCollider.enabled = true;
		this.enabled = true;
		body.SetActive (true);
		for (int i = 0; i < barrels.Length; i++) {
			barrels [i].gameObject.SetActive (false);
		}
		gameObject.SetActive (false);
	}

	private IEnumerator WaitForShootPause() {
		yield return new WaitForSeconds (shootPause);
		pausing = false;
	}

	private IEnumerator WaitForHitMaterial() {
		for (int i = 0; i < barrelRenderer.Length; i++) {
			barrelRenderer [i].material = hitMaterial;
		}
		bodyRenderer.material = hitMaterial;
		yield return new WaitForSeconds (hitMaterialWaitTime);
		for (int i = 0; i < barrelRenderer.Length; i++) {
			barrelRenderer [i].material = normalMaterial;
		}
		bodyRenderer.material = normalMaterial;
	}

	private void EvaluateActorState(Vector3 rawTargetPosition) {
		switch (currentActorState) {
		case ActorState.ACTOR_FOLLOW:
			agent.destination = rawTargetPosition;
			agent.Resume ();
			break;
		case ActorState.ACTOR_STOP:
			agent.Stop ();
			break;
		}
	}

	private void OnEnable() {
		if (trails.Length > 0) {
			trails [0].Clear ();
			trails [0].enabled = true;
			trails [1].Clear ();
			trails [1].enabled = true;
		}
		for (int i = 0; i < barrels.Length; i++) {
			barrels [i].gameObject.SetActive (true);
		}

		currentShootCount = 0;
		pausing = false;
		rotatingShooting = false;
		currentHealth = maxHealth;

		for (int i = 0; i < barrelRenderer.Length; i++) {
			barrelRenderer [i].material = normalMaterial;
		}
		bodyRenderer.material = normalMaterial;
		targetable = true;
	}
}
