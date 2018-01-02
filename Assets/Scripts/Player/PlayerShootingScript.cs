using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShootingScript : MonoBehaviour {

	public Transform barrelEnd;

	public enum WeaponType {NORMAL_MISSILE, BOMB_SHOT, PULSE_SHOT};
	public WeaponType currentWeaponType;
	public GameObject[] playerWeapons;

	private PlayerWeapon currentPlayerWeapon;
	private bool canShoot = true;

	private void Awake() {
		switch (currentWeaponType) {
		case WeaponType.NORMAL_MISSILE:
			playerWeapons [0].SetActive (true);
			currentPlayerWeapon = playerWeapons [0].GetComponent<PlayerWeapon> ();
			break;
		case WeaponType.BOMB_SHOT:
			playerWeapons [1].SetActive (true);
			currentPlayerWeapon = playerWeapons [1].GetComponent<PlayerWeapon> ();
			break;
		case WeaponType.PULSE_SHOT:
			playerWeapons [2].SetActive (true);
			currentPlayerWeapon = playerWeapons [2].GetComponent<PlayerWeapon> ();
			break;
		}
			
		currentPlayerWeapon.Init ();
	}

	private void Update() {
		if (this.enabled && canShoot) {
			if (Input.GetKeyDown (KeyCode.Mouse0)) {
				currentPlayerWeapon.Shoot ('D', barrelEnd);
			}

			if (Input.GetKey (KeyCode.Mouse0)) {
				currentPlayerWeapon.Shoot ('H', barrelEnd);
			}

			if (Input.GetKeyUp (KeyCode.Mouse0)) {
				currentPlayerWeapon.Shoot ('U', barrelEnd);
			}
		}
	}

	public void UpdateTargetingLine(RaycastHit floorHit, RaycastHit obscuranceHit) {
		if (this.enabled) {
			currentPlayerWeapon.UpdateTargetingLine (barrelEnd, obscuranceHit, floorHit);
		}
	}

	public void SetCanShoot (bool newValue) {
		canShoot = newValue;
		if (canShoot) {
			//thisLineRenderer.enabled = true;
		} else {
			//thisLineRenderer.enabled = false;
		}
	}
}
