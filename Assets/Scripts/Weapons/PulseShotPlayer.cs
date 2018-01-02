using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseShotPlayer : MonoBehaviour, PlayerWeapon {

	public float minTargetOpen = 0.5f, maxTargetOpen = 100f;
	public float targetOpenCloseSpeed = 50f;
	public LineRenderer thisLineRenderer;

	private bool expanding = false;

	public void Init() {
		
	}

	public void Shoot(char mouseEvent, Transform barrelEnd) {
		
	}

	public void UpdateTargetingLine(Transform barrelEnd, RaycastHit obscuranceHit, RaycastHit floorHit) {
		thisLineRenderer.SetPosition (0, new Vector3 (barrelEnd.position.x, barrelEnd.position.y, barrelEnd.position.z));
		thisLineRenderer.SetPosition (1, new Vector3 (floorHit.point.x, barrelEnd.position.y, floorHit.point.z));
		if (expanding) {
			thisLineRenderer.endWidth += targetOpenCloseSpeed * Time.deltaTime;
			if (thisLineRenderer.endWidth >= maxTargetOpen) {
				expanding = false;
			}
		} else {
			thisLineRenderer.endWidth -= targetOpenCloseSpeed * Time.deltaTime;
			if (thisLineRenderer.endWidth <= minTargetOpen) {
				expanding = true;
			}
		}
	}
}
