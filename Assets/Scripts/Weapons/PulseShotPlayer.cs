using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseShotPlayer : MonoBehaviour, PlayerWeapon {

	public int damage = 25;
	public float range = 200f;
	public float shotTime = 0.5f, cooldown = 1f;
	public LineRenderer chargedShotLineRenderer;
	public GameObject targetingTriangle;
	public Transform head;

	private bool stopped = false, chargedShot = false;

	public void Init() {
	}

	public void Shoot(char mouseEvent, Transform barrelEnd) {
		if (mouseEvent.Equals ('D') && !stopped) {
			if (chargedShot) {
				RaycastHit[] hits;
				hits = Physics.RaycastAll (barrelEnd.position, barrelEnd.forward, range);
				for (int i = 0; i < hits.Length; i++) {
					Enemy hitEnemy = hits[i].collider.gameObject.GetComponent<Enemy>();
					if (hitEnemy != null) {
						hitEnemy.HitByProjectile (damage);
					}
				}

				chargedShotLineRenderer.SetPosition(0, barrelEnd.position);
				chargedShotLineRenderer.SetPosition(1, new Vector3 (barrelEnd.position.x + (head.forward.x * range), barrelEnd.position.y, barrelEnd.position.z + (head.forward.z * range)));
				StartCoroutine (ShotEffect ());
				StartCoroutine (WaitForCooldown ());
			}
		}
	}

	public void UpdateTargetingLine(Transform barrelEnd, RaycastHit obscuranceHit, RaycastHit floorHit) {
	}

	public void SetChargedShot() {
		chargedShot = !chargedShot;
	}

	private IEnumerator ShotEffect() {
		chargedShotLineRenderer.enabled = true;
		yield return new WaitForSeconds (shotTime);
		chargedShotLineRenderer.enabled = false;
	}

	private IEnumerator WaitForCooldown() {
		stopped = true;
		targetingTriangle.SetActive (false);
		yield return new WaitForSeconds (cooldown);
		stopped = false;
		targetingTriangle.SetActive (true);
	}
}
