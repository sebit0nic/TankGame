using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerWeapon {

	void Init();
	void Shoot(Transform barrelEnd);
	void UpdateTargetingLine(Transform barrelEnd, LineRenderer thisLineRenderer, RaycastHit obscuranceHit, RaycastHit floorHit);
	float GetWeaponCooldown();
}
