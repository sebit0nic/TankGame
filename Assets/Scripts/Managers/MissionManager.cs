using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour {

	public EnemyManager enemyManager;
	public ScoreManager scoreManager;
	public PlayerManager playerManager;

	public void NotifyEnemyDestroyed(int points, int loadedLevel) {
		scoreManager.NotifyEnemyDestroyed (points);
		enemyManager.NotifyEnemyDestroyed ();
	}

	public void NotifyPlayerHealthUpdate(float health, bool damaged) {
		playerManager.NotifyPlayerHealthUpdate (health);
		scoreManager.NotifyPlayerHealthUpdate (damaged);
	}

	public float GetCurrentHeatFactor() {
		return scoreManager.GetCurrentHeatFactor ();
	}

	public bool CanUseSpecialAbility() {
		return scoreManager.CanUseSpecialAbility ();
	}

	public void NotifyUseSpecialAbility() {
		scoreManager.NotifyUseSpecialAbility ();
	}
}
