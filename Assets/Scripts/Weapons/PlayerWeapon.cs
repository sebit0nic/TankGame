using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerWeapon {

	void Init();
	//mouseEvent = D: mouseDown, H: mouseHold, U: mouseUp
	void Shoot(char mouseEvent, Transform barrelEnd);
	void UpdateTargetingLine(Transform barrelEnd, RaycastHit obscuranceHit, RaycastHit floorHit);
}
