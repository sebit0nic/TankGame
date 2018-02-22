using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LasershotPlayer : MonoBehaviour, PlayerWeapon {

	public float range;
	public int damage = 1;
	public float damageDelay = 0.1f;
	public LineRenderer laserLineRenderer;
	public GameObject laserParticles, laserEndParticles;

	private bool onDamageDelay = false;

	public void Init() {
	}

	public void Shoot(char mouseEvent, Transform barrelEnd) {
		if (mouseEvent.Equals ('H')) {
			laserLineRenderer.enabled = true;
			laserLineRenderer.SetPosition (0, barrelEnd.position);

			RaycastHit hit;
			int layerMask = ~(1 << LayerMask.NameToLayer("Projectile"));
			if (Physics.Raycast (barrelEnd.position, barrelEnd.transform.forward, out hit, range, layerMask)) {
				laserLineRenderer.SetPosition (1, new Vector3 (hit.point.x, barrelEnd.position.y, hit.point.z));
				laserEndParticles.transform.position = new Vector3 (hit.point.x, barrelEnd.position.y, hit.point.z);
				laserParticles.transform.localScale = new Vector3 (1, 1, Vector3.Distance (barrelEnd.position, hit.point) / range);

				laserParticles.SetActive (true);
				laserEndParticles.SetActive (true);

				Enemy hitEnemy = hit.collider.GetComponent<Enemy> ();
				if (hitEnemy != null) {
					if (!onDamageDelay) {
						hitEnemy.HitByProjectile (damage);
						StartCoroutine (WaitForDamageDelay ());
					}
				}
			} else {
				laserParticles.transform.localScale = Vector3.one;
				laserParticles.SetActive (true);

				laserEndParticles.SetActive (false);
				laserLineRenderer.SetPosition (1, new Vector3 (barrelEnd.position.x + (barrelEnd.forward.x * range), barrelEnd.position.y, barrelEnd.position.z + (barrelEnd.forward.z * range)));
			}
		}
		if (mouseEvent.Equals ('U')) {
			laserParticles.SetActive (false);
			laserEndParticles.SetActive (false);
			laserLineRenderer.enabled = false;
		}
	}

	public void UpdateTargetingLine(Transform barrelEnd, RaycastHit obscuranceHit, RaycastHit floorHit) {
	}

	private IEnumerator WaitForDamageDelay() {
		onDamageDelay = true;
		yield return new WaitForSeconds (damageDelay);
		onDamageDelay = false;
	}
}
