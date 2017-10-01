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

	[Header("Shooting Properties")]
	public Rigidbody projectile;
	public float minShootDistance = 20;
	public float shootFrequency = 1;
	public float projectileSpeed = 10;

	private NavMeshAgent agent;
	public float distance;
	private float shootTimer;

	private GameManager gameManager;
	private Transform target;
	private ObjectPool objectPool;

	private IEnumerator particleCoroutine;
	private Rigidbody thisRigidbody;
	private Collider thisCollider;
	private MeshRenderer thisRenderer;

	private void Awake() {
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager>();
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
		//Check if player is in sight, otherwise move closer to player
		bool playerInSight = false;
		distance = Vector3.Distance (transform.position, target.position);

		if (distance < minShootDistance) {
			RaycastHit hit;
			int layerMask = ~(1 << LayerMask.NameToLayer("Enemy"));
			if (Physics.Raycast (barrelEnd.position, barrelEnd.transform.forward, out hit, 100.0f, layerMask)) {
				if (hit.collider.gameObject.tag.Equals ("Player")) {
					playerInSight = true;
				}
			}

			if (shootTimer < Time.time && playerInSight) {
				GameObject newProjectile = objectPool.GetPooledObjects();
				newProjectile.SetActive (true);
				newProjectile.GetComponent<SimpleMissile> ().Init (barrelEnd.position, barrelEnd.rotation, barrelEnd.transform.forward * projectileSpeed, ForceMode.Force);
				shootTimer = Time.time + shootFrequency;
			}
		}
			
		if (distance < minMoveAwayDistance) {
			if (playerInSight) {
				//Move in the opposite direction of the player
				Vector3 toPlayer = target.position - transform.position;
				Vector3 moveBackPosition = toPlayer.normalized * -minMoveAwayDistance;
				agent.destination = moveBackPosition;
				agent.Resume ();
			} else {
				agent.destination = target.position;
				agent.Resume ();
			}
		} else if (distance >= minMoveAwayDistance && distance < minStopDistance) {
			if (playerInSight) {
				agent.Stop ();
			} else {
				agent.destination = target.position;
				agent.Resume ();
			}
		} else if (distance >= minStopDistance) {
			agent.destination = target.position;
			agent.Resume ();
		}

		Vector3 enemyToTarget = target.position - transform.position;
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
}
