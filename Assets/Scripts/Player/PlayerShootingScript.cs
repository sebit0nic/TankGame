using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShootingScript : MonoBehaviour {

	public Transform barrelEnd;
	public float projectileSpeed = 500f;
	public float weaponCooldown = 1f;
	public Rigidbody simpleMissileTemplate;
	public enum WeaponType {simpleMissile};
	public WeaponType currentWeaponType;
	public Image cooldown;

	private LineRenderer thisLineRenderer;
	private float shootTimer;
	private float fillAmountTimer = 1f;

	private void Awake() {
		thisLineRenderer = GetComponent<LineRenderer> ();
	}

	private void Update() {
		float fillAmount = weaponCooldown - fillAmountTimer;
		if (fillAmount >= 1) {
			cooldown.fillAmount = 1;
		} else {
			cooldown.fillAmount = fillAmount;
		}
		fillAmountTimer += Time.deltaTime;
	}

	public void Shoot() {
		if (shootTimer < Time.time) {
			switch (currentWeaponType) {
			case WeaponType.simpleMissile:
				Rigidbody newProjectile = Instantiate (simpleMissileTemplate, barrelEnd.position, barrelEnd.rotation) as Rigidbody;
				newProjectile.AddForce (newProjectile.transform.forward * projectileSpeed, ForceMode.Force);
				break;
			}
			shootTimer = Time.time + weaponCooldown;
			fillAmountTimer = 0.01f;
		}
	}

	public void UpdateTargetingLine(RaycastHit floorHit) {
		thisLineRenderer.SetPosition (0, new Vector3 (barrelEnd.position.x, barrelEnd.position.y, barrelEnd.position.z));
		thisLineRenderer.SetPosition (1, new Vector3 (floorHit.point.x, barrelEnd.position.y, floorHit.point.z));
	}
}
