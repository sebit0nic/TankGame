using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Level {

	void OnLevelStart ();
	void NotifyEnemyDestroyed();
    void OnLevelEnd ();
}
