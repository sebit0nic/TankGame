using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseShotPlayer : MonoBehaviour, PlayerWeapon {

	public int damage = 25;
	public float range = 200f;
	public float chargedShotTime = 0.5f, cooldown = 1f;
	public LineRenderer chargedShotLineRenderer;
	public GameObject targetingTriangle;
	public Transform head;
	public ParticleSystem shotParticle;

	private bool stopped = false, chargedShot = false;

	public void Init() {
	}

	public void Shoot(char mouseEvent, Transform barrelEnd) {
		if (mouseEvent.Equals ('D') && !stopped) {
			if (chargedShot) {
				RaycastHit[] hits;
				hits = Physics.RaycastAll (barrelEnd.position, barrelEnd.forward, range);
				for (int i = 0; i < hits.Length; i++) {
					Enemy hitEnemy = hits [i].collider.gameObject.GetComponent<Enemy> ();
					if (hitEnemy != null) {
						hitEnemy.HitByProjectile (damage);
					}
				}

				chargedShotLineRenderer.SetPosition (0, barrelEnd.position);
				chargedShotLineRenderer.SetPosition (1, new Vector3 (barrelEnd.position.x + (head.forward.x * range), barrelEnd.position.y, barrelEnd.position.z + (head.forward.z * range)));
				StartCoroutine (ChargedShotEffect ());
				StartCoroutine (WaitForCooldown ());
			} else {
				StartCoroutine (OpenShotEffect ());
				StartCoroutine (WaitForCooldown ());
			}
		}
	}

	public void UpdateTargetingLine(Transform barrelEnd, RaycastHit obscuranceHit, RaycastHit floorHit) {
	}

	public void SetChargedShot() {
		chargedShot = !chargedShot;
	}

	private IEnumerator ChargedShotEffect() {
		chargedShotLineRenderer.enabled = true;
		yield return new WaitForSeconds (chargedShotTime);
		chargedShotLineRenderer.enabled = false;
	}

	private IEnumerator OpenShotEffect() {
		//shotParticle.Clear ();
		shotParticle.Play ();
		targetingTriangle.GetComponent<MeshCollider> ().enabled = true;
		yield return new WaitForSeconds (shotParticle.main.duration);
		targetingTriangle.GetComponent<MeshCollider> ().enabled = false;
	}

	private IEnumerator WaitForCooldown() {
		stopped = true;
		targetingTriangle.GetComponent<MeshRenderer>().enabled = false;
		yield return new WaitForSeconds (cooldown);
		stopped = false;
		targetingTriangle.GetComponent<MeshRenderer> ().enabled = true;
	}
}
