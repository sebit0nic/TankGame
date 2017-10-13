using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ProjectileShootingEnemy : MonoBehaviour, Enemy {

	public Transform barrel;
	public Transform barrelEnd;
	public int basePoints = 20;
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

	private NavMeshAgent agent;
	public float distance;
	private float shootTimer;
	private float randomShootDeviation;

	private GameManager gameManager;
	private Transform target;
	private ObjectPool objectPool;

	private IEnumerator particleCoroutine;
	private Rigidbody thisRigidbody;
	private Collider thisCollider;
	private MeshRenderer thisRenderer;

	private enum ActorState {ACTOR_FOLLOW, ACTOR_STOP, ACTOR_AVOID};
	private ActorState currentActorState = ActorState.ACTOR_FOLLOW;

	private void Awake() {
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager>();
		target = GameObject.Find ("Player").transform;
		objectPool = GetComponent<ObjectPool> ();

		thisRigidbody = GetComponent<Rigidbody> ();
		thisCollider = GetComponent<Collider> ();
		thisRenderer = GetComponent<MeshRenderer> ();

		//Give the shootTimer some sort of randomness so that each enemy doesn't fire at
		//the exact same time
		randomShootDeviation = Random.Range (1f, 2f);
	}

	private void Start() {
		agent = GetComponent<NavMeshAgent> ();
	}

	private void Update() {
		//TODO: make this Update() more performant

		//Check if player is in sight, otherwise move closer to player
		bool playerInSight = false;
		Vector3 rawTargetPosition = new Vector3 (target.position.x, transform.position.y, target.position.z);
		distance = Vector3.Distance (transform.position, rawTargetPosition);

		if (distance < minShootDistance) {
			RaycastHit hit;
			int layerMask = ~((1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Projectile")));
			if (Physics.Raycast (barrelEnd.position, barrelEnd.transform.forward, out hit, 100.0f, layerMask)) {
				if (hit.collider.gameObject.tag.Equals ("Player")) {
					playerInSight = true;
				}
			}

			if (shootTimer < Time.time && playerInSight) {
				GameObject newProjectile = objectPool.GetPooledObjects();
				newProjectile.SetActive (true);
				newProjectile.GetComponent<SimpleMissile> ().Init (barrelEnd.position, barrelEnd.rotation, barrelEnd.transform.forward * projectileSpeed, ForceMode.Force);
				shootTimer = Time.time + shootFrequency + randomShootDeviation;
			}
		}
			
		if (distance < minMoveAwayDistance) {
			if (playerInSight) {
				//Move in the opposite direction of the player
				currentActorState = ActorState.ACTOR_AVOID;
			} else {
				//Player uses ability, so don't follow
				if (gameManager.GetPlayerState ().Equals ('A')) {
					currentActorState = ActorState.ACTOR_AVOID;
				} else {
					currentActorState = ActorState.ACTOR_FOLLOW;
				}
			}
		} else if (distance >= minMoveAwayDistance && distance < minStopDistance) {
			if (playerInSight) {
				currentActorState = ActorState.ACTOR_STOP;
			} else {
				if (gameManager.GetPlayerState ().Equals ('A')) {
					currentActorState = ActorState.ACTOR_STOP;
				} else {
					currentActorState = ActorState.ACTOR_FOLLOW;
				}
			}
		} else if (distance >= minStopDistance) {
			currentActorState = ActorState.ACTOR_FOLLOW;
		}

		EvaluateActorState (rawTargetPosition);

		Vector3 enemyToTarget = rawTargetPosition - transform.position;
		enemyToTarget.y = barrel.position.y;

		Quaternion barrelRotation = Quaternion.LookRotation (enemyToTarget);
		barrelRotation.x = 0;
		barrelRotation.z = 0;
		barrel.rotation = barrelRotation;
	}

	public void HitByProjectile() {
		gameManager.NotifyEnemyDestroyed (basePoints);

		particleCoroutine = WaitForParticleFinish (thisParticleSystem[0].main.duration);

		this.enabled = false;
		thisRenderer.enabled = false;
		thisRigidbody.isKinematic = true;
		thisCollider.enabled = false;
		body.SetActive (false);
		barrel.gameObject.SetActive (false);
		trails [0].enabled = false;
		trails [1].enabled = false;

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

		thisRenderer.enabled = true;
		thisRigidbody.isKinematic = false;
		thisCollider.enabled = true;
		this.enabled = true;
		body.SetActive (true);
		barrel.gameObject.SetActive (true);
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
		trails [0].Clear ();
		trails [0].enabled = true;
		trails [1].Clear ();
		trails [1].enabled = true;
	}
}
