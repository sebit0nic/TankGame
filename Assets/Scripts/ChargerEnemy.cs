using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChargerEnemy : MonoBehaviour {

	public Transform target;

	private NavMeshAgent agent;

	private void Start() {
		agent = GetComponent<NavMeshAgent> ();
	}

	private void Update() {
		agent.destination = target.position;
	}
}
