﻿using System.Collections;
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
	private bool medal1, medal2, medal3;
	private bool task1, task2, task3;

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
		enemyManager.NotifyEnemyDestroyed ();
		mission.NotifyEnemyDestroyed ();
	}

	public void NotifyPlayerHit(int health) {
		playerManager.NotifyPlayerHit (health);
		scoreManager.NotifyPlayerHit ();
	}

	public void NotifyMissionEnd(int loadedLevel) {
		levels [loadedLevel].OnLevelEnd ();
	}

	public void NotifyMedalStep(int index) {
		switch (index) {
		case 0:
			medal1 = true;
			break;
		case 1:
			medal2 = true;
			break;
		case 2:
			medal3 = true;
			break;
		}
	}

	public void NotifySecondaryTaskSuccessful(int index) {
		switch (index) {
		case 0:
			task1 = true;
			break;
		case 1:
			task2 = true;
			break;
		case 2:
			task3 = true;
			break;
		}
	}

	public void InitEventSuccessfulScreen (EventSuccessfulScreen eventSuccessfulScreen) {
		eventSuccessfulScreen.Init (scoreManager.currentScore, medal1, medal2, medal3, task1, task2, task3);
	}
}
