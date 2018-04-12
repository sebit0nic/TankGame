using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HitscanEnemy : MonoBehaviour, Enemy  {

	public GameObject body;
	public GameObject barrel;
	public Transform barrelEnd;
	public LineRenderer targetingLine;
	public LineRenderer shotLine;
	public float minTargetingDistance = 50f;
	public float targetingTime = 2f;
	public float shootDelay = 1f, shootCooldown = 1f;
	public float turningSpeed = 5f;
	public float shotLineTime = 0.1f;
	public int maxHealth = 30;
	public int basePoints = 20;
	public TrailRenderer[] trails;
	public ParticleSystem[] thisParticleSystem;
	public Material normalMaterial, hitMaterial;
	public float hitMaterialWaitTime = 0.5f;

	private Transform target;
	private NavMeshAgent agent;
	private enum ActorState {ACTOR_FOLLOW, ACTOR_TARGETING, ACTOR_SHOOTING}
	private ActorState currentActorState = ActorState.ACTOR_FOLLOW;
	private float distance;
	private bool barrelRotationBlock, canShoot = true;
	private float shootTimer, targetTimer, shootCooldownTimer;
	private int currentHealth;
	private Collider thisCollider;
	private bool targetable;
	private ObjectPool effectPool;
	private IEnumerator particleCoroutine;
	private MeshRenderer barrelRenderer, bodyRenderer;

	private void Awake() {
		target = GameObject.Find ("Player").transform;
		agent = GetComponent<NavMeshAgent> ();

		thisCollider = GetComponent<Collider> ();
		effectPool = GameObject.Find ("Effect Pool").GetComponent<ObjectPool> ();

		barrelRenderer = barrel.GetComponentInChildren<MeshRenderer> ();
		bodyRenderer = body.GetComponent<MeshRenderer> ();
	}

	private void Update() {
		Vector3 rawTargetPosition = new Vector3 (target.position.x, transform.position.y, target.position.z);
		distance = Vector3.Distance (transform.position, rawTargetPosition);

		Vector3 enemyToTarget = rawTargetPosition - transform.position;
		enemyToTarget.y = barrel.transform.position.y;

		if (!barrelRotationBlock) {
			Quaternion barrelRotation = Quaternion.Lerp (barrel.transform.rotation, Quaternion.LookRotation (enemyToTarget), Time.deltaTime * turningSpeed);
			barrelRotation.x = 0;
			barrelRotation.z = 0;
			barrel.transform.rotation = barrelRotation;
		}

		EvaluateActorState (rawTargetPosition);
	}

	public void HitByProjectile(int damage) { 
		currentHealth -= damage;
		StartCoroutine (WaitForHitMaterial ());

		if (currentHealth <= 0) {
			GameManager.GetInstance ().NotifyEnemyDestroyed (basePoints);

			particleCoroutine = WaitForParticleFinish (thisParticleSystem[0].main.duration);

			this.enabled = false;
			thisCollider.enabled = false;
			body.SetActive (false);
			barrel.SetActive (false);
			trails [0].enabled = false;
			trails [1].enabled = false;

			for (int i = 0; i < thisParticleSystem.Length; i++) {
				thisParticleSystem [i].Play ();
			}
			targetable = false;

			//Spawn a score text above the destroyed enemy
			GameObject pointText = effectPool.GetPooledObjectByIndex (0);
			pointText.transform.position = transform.position;
			pointText.GetComponentInChildren<PointsText> ().SetText (basePoints.ToString ());
			pointText.SetActive (true);

			//Spawn a crater model around the enemy
			GameObject gravel = effectPool.GetPooledObjectByIndex (1);
			gravel.transform.position = transform.position;
			gravel.SetActive (true);
			gravel.transform.rotation = Quaternion.identity;
			gravel.transform.Rotate (-270, 0, Random.Range(0, 360f));

			StartCoroutine (particleCoroutine);
		}
	}

	public bool IsTargetable() {
		return targetable;
	}

	private void EvaluateActorState(Vector3 rawTargetPosition) {
		switch (currentActorState) {
		case ActorState.ACTOR_FOLLOW:
			FollowPlayer (rawTargetPosition);
			break;
		case ActorState.ACTOR_TARGETING:
			StartTargeting();
			break;
		case ActorState.ACTOR_SHOOTING:
			StartShooting ();
			break;
		}
	}

	private void FollowPlayer(Vector3 rawTargetPosition) {
		if (distance > minTargetingDistance && canShoot) {
			agent.destination = rawTargetPosition;
			agent.Resume ();
		} else {
			if (canShoot) {
				RaycastHit hit;
				int layerMask = ~((1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Projectile")));
				if (Physics.Raycast (barrelEnd.position, barrelEnd.transform.forward, out hit, 100.0f, layerMask)) {
					if (hit.collider.gameObject.tag.Equals ("Player")) {
						currentActorState = ActorState.ACTOR_TARGETING;
						targetTimer = Time.time + targetingTime;
					}
				}
			}
		}
	}

	private void StartTargeting() {
		targetingLine.enabled = true;

		if (Time.time > targetTimer) {
			currentActorState = ActorState.ACTOR_SHOOTING;
			barrelRotationBlock = true;
			shootTimer = Time.time + shootDelay;
		}
	}

	private void StartShooting() {
		agent.Stop ();
		targetingLine.startColor = Color.red;
		targetingLine.endColor = Color.red;
		if (Time.time > shootTimer) {
			RaycastHit hit;
			int layerMask = ~((1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Projectile")));
			if (Physics.Raycast (barrelEnd.position, barrelEnd.transform.forward, out hit, 100.0f, layerMask)) {
				if (hit.collider.gameObject.tag.Equals ("Player")) {
					hit.collider.gameObject.GetComponent<PlayerHealth> ().DecreaseCurrentHealth (20);
				}
			}

			StartCoroutine (ShowShotLine ());
			StartCoroutine (WaitForShootCooldown ());
			currentActorState = ActorState.ACTOR_FOLLOW;
			targetingLine.startColor = Color.yellow;
			targetingLine.endColor = Color.yellow;
			targetingLine.enabled = false;
		}
	}

	private IEnumerator ShowShotLine() {
		shotLine.enabled = true;
		yield return new WaitForSeconds (shotLineTime);
		shotLine.enabled = false;
	}

	private IEnumerator WaitForShootCooldown() {
		canShoot = false;
		yield return new WaitForSeconds (shootCooldown);
		canShoot = true;
		barrelRotationBlock = false;
	}

	private IEnumerator WaitForParticleFinish(float waitTime) {
		yield return new WaitForSeconds (waitTime);
		for (int i = 0; i < thisParticleSystem.Length; i++) {
			thisParticleSystem [i].Clear ();
		}

		thisCollider.enabled = true;
		this.enabled = true;
		body.SetActive (true);
		barrel.gameObject.SetActive (true);
		gameObject.SetActive (false);
	}

	private IEnumerator WaitForHitMaterial() {
		barrelRenderer.material = hitMaterial;
		bodyRenderer.material = hitMaterial;
		yield return new WaitForSeconds (hitMaterialWaitTime);
		barrelRenderer.material = normalMaterial;
		bodyRenderer.material = normalMaterial;
	}

	private void OnEnable() {
		trails [0].Clear ();
		trails [0].enabled = true;
		trails [1].Clear ();
		trails [1].enabled = true;

		currentHealth = maxHealth;
		barrelRenderer.material = normalMaterial;
		bodyRenderer.material = normalMaterial;
		targetable = true;
	}
}
