using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMissilePlayer : MonoBehaviour, PlayerWeapon {

	public float projectileSpeed = 500f;
	public float weaponCooldown = 1f;

	private ObjectPool projectilePool;

	public void Init() {
		projectilePool = GetComponent<ObjectPool> ();
	}

	public void Shoot(Transform barrelEnd) {
		GameObject newProjectile = projectilePool.GetPooledObjects ();
		newProjectile.SetActive (true);
		newProjectile.GetComponent<NormalMissileCollision> ().Init (barrelEnd.position, barrelEnd.rotation, barrelEnd.transform.forward * projectileSpeed, ForceMode.Force);
	}

	public void UpdateTargetingLine(Transform barrelEnd, LineRenderer thisLineRenderer, RaycastHit obscuranceHit, RaycastHit floorHit) {
		thisLineRenderer.SetPosition (0, new Vector3 (barrelEnd.position.x, barrelEnd.position.y, barrelEnd.position.z));
		thisLineRenderer.SetPosition (1, new Vector3 (obscuranceHit.point.x, barrelEnd.position.y, obscuranceHit.point.z));
	}

	public float GetWeaponCooldown() {
		return weaponCooldown;
	}
}
