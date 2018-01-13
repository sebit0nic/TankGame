using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseShotPlayer : MonoBehaviour, PlayerWeapon {

	public float minTargetOpen = 0.5f, maxTargetOpen = 100f;
	public float targetOpenCloseSpeed = 50f;
	public float expansionOffset = 20f, expansionSpeed = 1f;
	public float waitCloseTimeout = 1f, waitOpenTimeout = 1f;
	public LineRenderer thisLineRenderer;
	public Transform head;

	private bool expanding = false, stopped = false;

	public void Init() {
	}

	public void Shoot(char mouseEvent, Transform barrelEnd) {
		
	}

	public void UpdateTargetingLine(Transform barrelEnd, RaycastHit obscuranceHit, RaycastHit floorHit) {
		thisLineRenderer.SetPosition (0, new Vector3 (barrelEnd.position.x, barrelEnd.position.y, barrelEnd.position.z));
		thisLineRenderer.SetPosition (1, new Vector3 (barrelEnd.position.x + (head.forward.x * expansionOffset), 10f, barrelEnd.position.z + (head.forward.z * expansionOffset)));
		if (!stopped) {
			if (expanding) {
				thisLineRenderer.endWidth += targetOpenCloseSpeed * Time.deltaTime;
				expansionOffset -= expansionSpeed * Time.deltaTime;
				if (thisLineRenderer.endWidth >= maxTargetOpen) {
					StartCoroutine (WaitForOpenCloseTimeout (false, waitOpenTimeout));
				}
			} else {
				thisLineRenderer.endWidth -= targetOpenCloseSpeed * Time.deltaTime;
				expansionOffset += expansionSpeed * Time.deltaTime;
				if (thisLineRenderer.endWidth <= minTargetOpen) {
					StartCoroutine (WaitForOpenCloseTimeout (true, waitCloseTimeout));
				}
			}
			expansionOffset = Mathf.Clamp (expansionOffset, 20f, 300f);
		}
	}

	private IEnumerator WaitForOpenCloseTimeout(bool expanding, float openCloseTimeout) {
		stopped = true;
		yield return new WaitForSeconds (openCloseTimeout);
		this.expanding = expanding;
		stopped = false;
	}
}
