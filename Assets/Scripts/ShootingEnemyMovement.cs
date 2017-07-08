using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootingEnemyMovement : MonoBehaviour {

	public Rigidbody projectile;
	public float shootDelay = 5;
	public float projectileSpeed = 500;

	private Transform target;
	private NavMeshAgent agent;
	private float timeout;

	private void Awake() {
		target = GameObject.Find ("Player").GetComponent<Transform> ();
	}

	private void Start() {
		agent = GetComponent<NavMeshAgent> ();
	}

	private void Update() {
		RaycastHit hit;
		Debug.DrawRay (transform.position, transform.forward * 100, Color.black);

		transform.LookAt (target);

		if (Physics.Raycast (transform.position, transform.forward, out hit, 100.0f)) {
			//Check if enemy has player in clear sight
			if (hit.collider.gameObject.tag.Equals ("Player")) {
				if (timeout < Time.time) {
					Rigidbody newProjectile = Instantiate (projectile, transform.position, transform.rotation) as Rigidbody;
					newProjectile.AddForce (newProjectile.transform.forward * projectileSpeed, ForceMode.Force);
					timeout = Time.time + shootDelay;
				}
			} else {
				agent.destination = target.position;
			}
		} else {
			agent.destination = target.position;
		}
	}
}
