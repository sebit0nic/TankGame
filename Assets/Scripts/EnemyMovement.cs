using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {

	private Transform target;
	private NavMeshAgent agent;

	private void Awake() {
		target = GameObject.Find ("Player").GetComponent<Transform> ();
	}

	private void Start() {
		agent = GetComponent<NavMeshAgent> ();
	}

	private void Update() {
		agent.destination = target.position;
	}
}
