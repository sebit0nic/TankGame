using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour {

	public TaskManager taskManager;
	public EnemyManager enemyManager;
	public ScoreManager scoreManager;
	public TimeManager timeManager;
	public PlayerManager playerManager;

	public void NotifyEnemyDestroyed(int points) {
		scoreManager.NotifyEnemyDestroyed (points);
	}
}
