using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour {

	public EnemyManager enemyManager;
	public ScoreManager scoreManager;
	public TimeManager timeManager;
	public PlayerManager playerManager;
	public GameObject levelList;
	public Mission mission;

	private bool initDone = false;
	private Level[] levels;

	public void Init(GameManager gameManager, int loadedLevel) {
		mission.Init (gameManager, loadedLevel, playerManager);

		levels = levelList.GetComponentsInChildren<Level> (true);

		levels [loadedLevel].OnLevelStart ();
		initDone = true;
	}

	private void Update() {
		if (initDone) {
			mission.Update ();
		}
	}

	public void NotifyEnemyDestroyed(int points) {
		scoreManager.NotifyEnemyDestroyed (points);
		mission.NotifyEnemyDestroyed ();
	}

	public void NotifyPlayerHit(int health) {
		playerManager.NotifyPlayerHit (health);
		scoreManager.NotifyPlayerHit ();
	}

	public void NotifyMissionEnd(int loadedLevel) {
		levels [loadedLevel].OnLevelEnd ();
	}
}
