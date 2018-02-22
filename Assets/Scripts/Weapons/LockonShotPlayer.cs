using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockonShotPlayer : MonoBehaviour, PlayerWeapon {

	public float minProjectileSpeed = 400f, maxProjectileSpeed = 600f;
	public float lockonDelay = 0.75f;
	public int maxTargetCount = 13;
	public RectTransform[] targetPoints;

	private ObjectPool projectilePool;
	private GameObject[] targets;
	private bool targetingMode = true, canTarget = true;
	public int targetCount;
	private float projectileSpeed;

	public void Init() {
		projectilePool = GetComponent<ObjectPool> ();
		targets = new GameObject[maxTargetCount];
	}

	public void Shoot(char mouseEvent, Transform barrelEnd) {
		if (mouseEvent.Equals ('D') && targetingMode && targetCount > 0) {
			targetingMode = false;
			for (int i = 0; i < targetCount; i++) {
				Vector3 randomDeviation = new Vector3 (Random.Range (-0.5f, 0.5f), Random.Range (-0.5f, 0.5f), Random.Range (-0.5f, 0.5f));
				projectileSpeed = Random.Range (minProjectileSpeed, maxProjectileSpeed);
				GameObject newProjectile = projectilePool.GetPooledObjects ();
				newProjectile.SetActive (true);
				newProjectile.GetComponent<LockonShotCollision> ().Init (barrelEnd.position + randomDeviation, barrelEnd.rotation, targets[i], projectileSpeed, this);
				targets [i] = null;
				targetPoints [i].gameObject.SetActive (false);
			}
		}
	}

	public void UpdateTargetingLine(Transform barrelEnd, RaycastHit obscuranceHit, RaycastHit floorHit) {
		if (targetingMode) {
			if (canTarget && targetCount < maxTargetCount) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
					if (hit.collider.gameObject.tag.Equals ("Enemy")) {
						bool duplicateTarget = false;
						for (int i = 0; i < targetCount; i++) {
							if (targets [i] == hit.collider.gameObject) {
								duplicateTarget = true;
								break;
							}
						}

						if (!duplicateTarget) {
							targets [targetCount] = hit.collider.gameObject;
							targetPoints [targetCount].gameObject.SetActive (true);
							targetPoints [targetCount].position = Camera.main.WorldToScreenPoint (targets [targetCount].transform.position);

							targetCount++;
							StartCoroutine (WaitForLockonDelay ());
						}
					}
				}
			}

			for (int i = 0; i < targetCount; i++) {
				targetPoints [i].position = Camera.main.WorldToScreenPoint (targets [i].transform.position);
			}
		} else {
			if (targetCount == 0) {
				targetingMode = true;
			}
		}
	}

	public void NotifyLockonShotDestroyed() {
		targetCount--;
	}

	private IEnumerator WaitForLockonDelay() {
		canTarget = false;
		yield return new WaitForSeconds (lockonDelay);
		canTarget = true;
	}
}
