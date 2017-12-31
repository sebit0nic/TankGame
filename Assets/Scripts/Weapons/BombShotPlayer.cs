using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombShotPlayer : MonoBehaviour, PlayerWeapon {

	public float projectileSpeed = 500f;
	public float weaponCooldown = 1f;

	private ObjectPool projectilePool;
	private Vector3 balisticVelocity;
	private float shootForce = 10f, maxShootForce = 6000f, shootRamp = 6000f;
	private bool charging = false, bombOut = false;
	private GameObject newProjectile;

	public void Init() {
		projectilePool = GetComponent<ObjectPool> ();
	}

	public void Shoot(char mouseEvent, Transform barrelEnd) {
		if (mouseEvent.Equals ('D')) {
			if (bombOut) {
				newProjectile.GetComponent<BombShot> ().Explode ();
				bombOut = false;
			} else {
				charging = true;
			}
		}

		if (mouseEvent.Equals ('U') && charging) {
			charging = false;
			newProjectile = projectilePool.GetPooledObjects ();
			newProjectile.SetActive (true);
			newProjectile.transform.rotation = barrelEnd.rotation;
			newProjectile.GetComponent<BombShot> ().Init (barrelEnd, shootForce);
			shootForce = 10f;
			bombOut = true;
		}
	}

	public void UpdateTargetingLine(Transform barrelEnd, LineRenderer thisLineRenderer, RaycastHit obscuranceHit, RaycastHit floorHit) {
		if (charging) {
			shootForce += Time.deltaTime * shootRamp;
			shootForce = Mathf.Clamp (shootForce, 10f, maxShootForce); 
			thisLineRenderer.SetPosition (0, new Vector3 (barrelEnd.position.x, barrelEnd.position.y, barrelEnd.position.z));
			thisLineRenderer.SetPosition (1, new Vector3 (floorHit.point.x, barrelEnd.position.y, floorHit.point.z));

		} else {
			thisLineRenderer.SetPosition (0, new Vector3 (barrelEnd.position.x, barrelEnd.position.y, barrelEnd.position.z));
			thisLineRenderer.SetPosition (1, new Vector3 (floorHit.point.x, barrelEnd.position.y, floorHit.point.z));
			thisLineRenderer.startColor = new Color (1, 1, 1, 0.35f);
		}
	}

	public float GetWeaponCooldown() {
		return weaponCooldown;
	}
}
