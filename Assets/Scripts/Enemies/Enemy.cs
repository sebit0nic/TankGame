﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Enemy {

	void HitByProjectile(int damage);
	bool IsTargetable();
}
