using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ProjectileShootingEnemy : MonoBehaviour, Enemy {

	public Transform barrel;
	public Transform barrelEnd;
	public GameObject actorExplosion;
	public int basePoints = 20;
	public float minStopDistance = 15;
	public float minMoveAwayDistance = 7;

	[Header("Shooting Properties")]
	public Rigidbody projectile;
	public float minShootDistance = 20;
	public float shootFrequency = 1;
	public float projectileSpeed = 10;

	private NavMeshAgent agent;
	private float distance;
	private float shootTimer;

	private GameManager gameManager;
	private Transform target;
	private ObjectPool objectPool;

	private void Awake() {
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager>();
		target = GameObject.Find ("Player").transform;
		objectPool = GetComponent<ObjectPool> ();
	}

	private void Start() {
		agent = GetComponent<NavMeshAgent> ();
	}

	private void Update() {
		//Check if player is in sight, otherwise move closer to player
		bool playerInSight = false;
		RaycastHit hit;
		int layerMask = ~(1 << LayerMask.NameToLayer("Enemy"));
		if (Physics.Raycast (barrelEnd.position, barrelEnd.transform.forward, out hit, 200.0f, layerMask)) {
			if (hit.collider.gameObject.tag.Equals ("Player")) {
				playerInSight = true;
			}
		}

		distance = Vector3.Distance (transform.position, target.position);
		if (!playerInSight) {
			agent.ResetPath ();
			agent.destination = target.position;
		} else if (distance < minMoveAwayDistance) {
			agent.ResetPath ();
			//Move in the opposite direction of the player
			agent.destination = (target.position - transform.position) * -10;
		} else if (distance >= minMoveAwayDistance && distance < minStopDistance) {
			agent.Stop ();
		} else if (distance >= minStopDistance) {
			agent.ResetPath ();
			agent.destination = target.position;
		}

		Vector3 enemyToTarget = target.position - transform.position;
		enemyToTarget.y = barrel.position.y;

		Quaternion barrelRotation = Quaternion.LookRotation (enemyToTarget);
		barrelRotation.x = 0;
		barrelRotation.z = 0;
		barrel.rotation = barrelRotation;

		if (distance < minShootDistance && shootTimer < Time.time && playerInSight) {
			GameObject newProjectile = objectPool.GetPooledObjects();
			newProjectile.SetActive (true);
			newProjectile.GetComponent<SimpleMissile> ().Init (barrelEnd.position, barrelEnd.rotation, barrelEnd.transform.forward * projectileSpeed, ForceMode.Force);
			shootTimer = Time.time + shootFrequency;
		}
	}

	public void HitByProjectile() {
		Instantiate (actorExplosion, transform.position, Quaternion.identity);
		gameManager.NotifyEnemyDestroyed (basePoints);
		gameObject.SetActive (false);
	}
}
