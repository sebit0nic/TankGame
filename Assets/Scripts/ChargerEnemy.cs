using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChargerEnemy : MonoBehaviour, Enemy {

	public Transform target;
	public GameObject actorExplosion;
	public int basePoints = 10;

	private NavMeshAgent agent;
	private GameManager gameManager;

	private void Awake() {
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager>();
	}

	private void Start() {
		agent = GetComponent<NavMeshAgent> ();
	}

	private void Update() {
		agent.destination = target.position;
	}

	public void HitByProjectile() {
		Instantiate (actorExplosion, transform.position, Quaternion.identity);
		gameManager.NotifyEnemyDestroyed (basePoints);
		Destroy (gameObject);
	}
}
