using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombShotPlayer : MonoBehaviour, PlayerWeapon {

	public float projectileSpeed = 500f;
	public float weaponCooldown = 1f;

	private ObjectPool projectilePool;
	private Vector3 balisticVelocity;

	public void Init() {
		projectilePool = GetComponent<ObjectPool> ();
	}

	public void Shoot(Transform barrelEnd) {
		GameObject newProjectile = projectilePool.GetPooledObjects ();
		newProjectile.SetActive (true);
		newProjectile.GetComponent<BombShot> ().Init (barrelEnd.position, balisticVelocity);
	}

	public void UpdateTargetingLine(Transform barrelEnd, LineRenderer thisLineRenderer, RaycastHit obscuranceHit, RaycastHit floorHit) {
		CalculateBallisticVelocity (floorHit, barrelEnd, thisLineRenderer);
	}

	private void CalculateBallisticVelocity(RaycastHit floorHit, Transform barrelEnd, LineRenderer thisLineRenderer) {
		Vector3 dir = floorHit.point - barrelEnd.position;
		dir.y = 0;
		float dist = dir.magnitude;
		float a = 30 * Mathf.Deg2Rad;
		dir.y = dist * Mathf.Tan (a);
		float vel = Mathf.Sqrt (dist * Physics.gravity.magnitude / Mathf.Sin (2 * a));
		balisticVelocity = vel * dir.normalized;
		UpdateTrajectory (barrelEnd.position, balisticVelocity, Physics.gravity, thisLineRenderer);
	}

	private void UpdateTrajectory (Vector3 initialPosition, Vector3 initialVelocity, Vector3 gravity, LineRenderer thisLineRenderer) {
		int numSteps = 100; // for example
		float timeDelta = 1.0f / initialVelocity.magnitude; // for example

		thisLineRenderer.numPositions = numSteps;

		Vector3 position = initialPosition;
		Vector3 velocity = initialVelocity;
		for (int i = 0; i < numSteps; ++i) {
			thisLineRenderer.SetPosition(i, position);

			position += velocity * timeDelta + 0.5f * gravity * timeDelta * timeDelta;
			velocity += gravity * timeDelta;
		}
	}

	public float GetWeaponCooldown() {
		return weaponCooldown;
	}
}
