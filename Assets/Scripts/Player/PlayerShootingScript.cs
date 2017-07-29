using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShootingScript : MonoBehaviour {

	public Transform barrelEnd;
	public float projectileSpeed = 500f;
	public float weaponCooldown = 1f;
	public enum WeaponType {simpleMissile, bombShot};
	public WeaponType currentWeaponType;
	public GameObject[] projectileTemplates;
	public Image cooldown;

	private LineRenderer thisLineRenderer;
	private float shootTimer;
	private Vector3 balisticVelocity;
	private ObjectPool projectilePool;

	private void Awake() {
		thisLineRenderer = GetComponent<LineRenderer> ();
		projectilePool = GetComponent<ObjectPool> ();

		switch (currentWeaponType) {
		case WeaponType.simpleMissile:
			thisLineRenderer.numPositions = 2;
			projectilePool.Init (projectileTemplates [0]);
			break;
		case WeaponType.bombShot:
			thisLineRenderer.numPositions = 20;
			projectilePool.Init (projectileTemplates [1]);
			break;
		}
	}

	private void Update() {
		float fillAmount = (shootTimer - Time.time) / weaponCooldown;
		cooldown.fillAmount = fillAmount;
	}

	public void Shoot() {
		if (shootTimer < Time.time) {
			GameObject newProjectile = projectilePool.GetPooledObjects ();
			newProjectile.SetActive (true);
			switch (currentWeaponType) {
			case WeaponType.simpleMissile:
				newProjectile.GetComponent<SimpleMissile> ().Init (barrelEnd.position, barrelEnd.rotation, barrelEnd.transform.forward * projectileSpeed, ForceMode.Force);
				break;
			case WeaponType.bombShot:
				newProjectile.GetComponent<BombShot> ().Init (barrelEnd.position, balisticVelocity);
				break;
			}
			shootTimer = Time.time + weaponCooldown;
		}
	}

	public void UpdateTargetingLine(RaycastHit floorHit) {
		if (shootTimer < Time.time) {
			switch (currentWeaponType) {
			case WeaponType.simpleMissile:
				thisLineRenderer.SetPosition (0, new Vector3 (barrelEnd.position.x, barrelEnd.position.y, barrelEnd.position.z));
				thisLineRenderer.SetPosition (1, new Vector3 (floorHit.point.x, barrelEnd.position.y, floorHit.point.z));
				break;
			case WeaponType.bombShot:
				CalculateBallisticVelocity (floorHit);
				break;
			}
		} else {
			switch (currentWeaponType) {
			case WeaponType.simpleMissile:
				thisLineRenderer.SetPosition (0, Vector3.zero);
				thisLineRenderer.SetPosition (1, Vector3.zero);
				break;
			case WeaponType.bombShot:
				for (int i = 0; i < thisLineRenderer.numPositions; i++) {
					thisLineRenderer.SetPosition (i, Vector3.zero);
				}
				break;
			}
		}
	}

	private void CalculateBallisticVelocity(RaycastHit floorHit) {
		Vector3 dir = floorHit.point - barrelEnd.position;
		dir.y = 0;
		float dist = dir.magnitude;
		float a = 30 * Mathf.Deg2Rad;
		dir.y = dist * Mathf.Tan (a);
		float vel = Mathf.Sqrt (dist * Physics.gravity.magnitude / Mathf.Sin (2 * a));
		balisticVelocity = vel * dir.normalized;
		UpdateTrajectory (barrelEnd.position, balisticVelocity, Physics.gravity);
	}

	private void UpdateTrajectory (Vector3 initialPosition, Vector3 initialVelocity, Vector3 gravity) {
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
}
