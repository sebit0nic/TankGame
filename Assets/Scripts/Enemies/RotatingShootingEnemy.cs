using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RotatingShootingEnemy : MonoBehaviour, Enemy {

	public Transform[] barrels;
	public Transform[] barrelEnds;
	public int basePoints = 20;
	public int maxHealth = 50;
	public float minStopDistance = 15;
	public float minMoveAwayDistance = 7;
	public ParticleSystem[] thisParticleSystem;
	public GameObject body;
	public TrailRenderer[] trails;

	[Header("Shooting Properties")]
	public Rigidbody projectile;
	public float minShootDistance = 20;
	public float shootFrequency = 1;
	public float projectileSpeed = 10;
	public float barrelRotationSpeed = 10;
	public float shootDuration = 5;
	public float shootPause = 5;

	private NavMeshAgent agent;
	public float distance;
	private float shootTimer;
	private int currentHealth;

	private Transform target;
	private ObjectPool objectPool;

	private IEnumerator particleCoroutine;
	private Rigidbody thisRigidbody;
	private Collider thisCollider;
	private MeshRenderer thisRenderer;
	private bool rotatingShooting, pausing;

	private enum ActorState {ACTOR_FOLLOW, ACTOR_STOP, ACTOR_AVOID};
	private ActorState currentActorState = ActorState.ACTOR_FOLLOW;

	private void Awake() {
		target = GameObject.Find ("Player").transform;
		objectPool = GetComponent<ObjectPool> ();

		thisRigidbody = GetComponent<Rigidbody> ();
		thisCollider = GetComponent<Collider> ();
		thisRenderer = GetComponent<MeshRenderer> ();
	}

	private void Start() {
		agent = GetComponent<NavMeshAgent> ();
	}

	private void Update() {
		Vector3 rawTargetPosition = new Vector3 (target.position.x, transform.position.y, target.position.z);
		distance = Vector3.Distance (transform.position, rawTargetPosition);

		if (distance < minShootDistance) {
			rotatingShooting = true;
		}
			
		if (distance < minMoveAwayDistance) {
			if (GameManager.GetInstance().GetPlayerState().Equals('A')) {
				currentActorState = ActorState.ACTOR_AVOID;
			} else {
				currentActorState = ActorState.ACTOR_FOLLOW;
			}
		} else if (distance >= minMoveAwayDistance && distance < minStopDistance) {
			if (GameManager.GetInstance().GetPlayerState().Equals('A')) {
				currentActorState = ActorState.ACTOR_STOP;
			} else {
				currentActorState = ActorState.ACTOR_FOLLOW;
			}
		} else if (distance >= minStopDistance) {
			currentActorState = ActorState.ACTOR_FOLLOW;
		}

		EvaluateActorState (rawTargetPosition);

		for (int i = 0; i < barrels.Length; i++) {
			barrels [i].Rotate (0, 0, barrelRotationSpeed * Time.deltaTime);
		}
		if (rotatingShooting) {
			if (Time.time > shootTimer) {
				for (int i = 0; i < barrelEnds.Length; i++) {
					GameObject newProjectile = objectPool.GetPooledObjects();
					newProjectile.SetActive (true);
					newProjectile.GetComponent<SimpleMissile> ().Init (barrelEnds[i].position, barrelEnds[i].rotation, barrelEnds[i].transform.forward * projectileSpeed, ForceMode.Force);
					shootTimer = Time.time + shootFrequency;
				}
			}
		}
	}

	public void HitByProjectile(int damage) {
		currentHealth -= damage;
		if (currentHealth <= 0) {
			GameManager.GetInstance ().NotifyEnemyDestroyed (basePoints);

			particleCoroutine = WaitForParticleFinish (thisParticleSystem[0].main.duration);

			this.enabled = false;
			thisRenderer.enabled = false;
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
			StartCoroutine (particleCoroutine);
		}
	}

	private IEnumerator WaitForParticleFinish(float waitTime) {
		yield return new WaitForSeconds (waitTime);
		for (int i = 0; i < thisParticleSystem.Length; i++) {
			thisParticleSystem [i].Clear ();
		}

		thisRenderer.enabled = true;
		thisRigidbody.isKinematic = false;
		thisCollider.enabled = true;
		this.enabled = true;
		body.SetActive (true);
		for (int i = 0; i < barrels.Length; i++) {
			barrels [i].gameObject.SetActive (false);
		}
		gameObject.SetActive (false);
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
		case ActorState.ACTOR_AVOID:
			transform.rotation = Quaternion.LookRotation (transform.position - rawTargetPosition);
			Vector3 runTo = transform.position + transform.forward * 20f;
			NavMeshHit hit;
			NavMesh.SamplePosition (runTo, out hit, 5, 1 << NavMesh.GetAreaFromName ("Walkable"));
			agent.SetDestination (hit.position);
			agent.Resume ();
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

		shootTimer = Time.time + shootFrequency;
		currentHealth = maxHealth;
	}
}
