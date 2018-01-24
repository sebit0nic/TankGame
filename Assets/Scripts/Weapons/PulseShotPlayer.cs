using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseShotPlayer : MonoBehaviour, PlayerWeapon {

	public int damage = 25;
	public float range = 200f;
	public float minTargetOpen = 0.5f, maxTargetOpen = 100f;
	public float minTargetOpenChargedShot = 1f;
	public float shotTime = 0.5f;
	public float targetOpenCloseSpeed = 50f;
	public float expansionOffset = 20f, expansionSpeed = 1f;
	public float waitCloseTimeout = 1f, waitOpenTimeout = 1f;
	public LineRenderer thisLineRenderer, chargedShotLineRenderer;
	public Transform head;
	public Color normalShotColor, chargedShotColor;

	private bool expanding = false, stopped = false, chargedShot = false;

	public void Init() {
	}

	public void Shoot(char mouseEvent, Transform barrelEnd) {
		if (mouseEvent.Equals ('D')) {
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
			} else {
				
			}
		}
	}

	public void UpdateTargetingLine(Transform barrelEnd, RaycastHit obscuranceHit, RaycastHit floorHit) {
		Debug.DrawRay(barrelEnd.position, barrelEnd.forward * range, Color.green);
		thisLineRenderer.SetPosition (0, new Vector3 (barrelEnd.position.x, barrelEnd.position.y, barrelEnd.position.z));
		thisLineRenderer.SetPosition (1, new Vector3 (barrelEnd.position.x + (head.forward.x * expansionOffset), 10f, barrelEnd.position.z + (head.forward.z * expansionOffset)));
		if (!stopped) {
			if (expanding) {
				thisLineRenderer.endWidth += targetOpenCloseSpeed * Time.deltaTime;
				expansionOffset -= expansionSpeed * Time.deltaTime;
				if (thisLineRenderer.endWidth >= minTargetOpenChargedShot) {
					chargedShot = false;
				}
				if (thisLineRenderer.endWidth >= maxTargetOpen) {
					StartCoroutine (WaitForOpenCloseTimeout (false, waitOpenTimeout));
				}
			} else {
				thisLineRenderer.endWidth -= targetOpenCloseSpeed * Time.deltaTime;
				expansionOffset += expansionSpeed * Time.deltaTime;
				if (thisLineRenderer.endWidth < minTargetOpenChargedShot) {
					chargedShot = true;
				}
				if (thisLineRenderer.endWidth <= minTargetOpen) {
					StartCoroutine (WaitForOpenCloseTimeout (true, waitCloseTimeout));
				}
			}
			expansionOffset = Mathf.Clamp (expansionOffset, 20f, 300f);
		}

		if (chargedShot) {
			thisLineRenderer.startColor = chargedShotColor;
		} else {
			thisLineRenderer.startColor = normalShotColor;
		}
	}

	private IEnumerator WaitForOpenCloseTimeout(bool expanding, float openCloseTimeout) {
		stopped = true;
		yield return new WaitForSeconds (openCloseTimeout);
		this.expanding = expanding;
		stopped = false;
	}

	private IEnumerator ShotEffect() {
		chargedShotLineRenderer.enabled = true;
		yield return new WaitForSeconds (shotTime);
		chargedShotLineRenderer.enabled = false;
	}
}
