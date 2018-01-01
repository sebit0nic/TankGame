using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombShotPlayer : MonoBehaviour, PlayerWeapon {

	public float projectileSpeed = 500f;
	public float weaponCooldown = 1f;
	public ParticleSystem thisParticleSystem;
	public Animator targetingAnimator;

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
				thisParticleSystem.Play ();
			} else {
				charging = true;
				thisParticleSystem.Stop ();
				thisParticleSystem.Clear ();
				thisParticleSystem.Play ();
				targetingAnimator.SetTrigger ("OnExtend");
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
			targetingAnimator.SetTrigger ("OnIdle");
			thisParticleSystem.Clear ();
			thisParticleSystem.Stop ();
		}
	}

	public void UpdateTargetingLine(Transform barrelEnd, RaycastHit obscuranceHit, RaycastHit floorHit) {
		if (charging) {
			shootForce += Time.deltaTime * shootRamp;
			shootForce = Mathf.Clamp (shootForce, 10f, maxShootForce);
		}
	}

	public float GetWeaponCooldown() {
		return weaponCooldown;
	}
}
