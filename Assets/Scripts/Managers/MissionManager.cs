using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour {

	public EnemyManager enemyManager;
	public ScoreManager scoreManager;
	public TimeManager timeManager;
	public PlayerManager playerManager;
	public GameObject levelList;

	private Level[] levels;
	private bool medal1, medal2, medal3;
	private bool task1, task2, task3;

	public void Init(GameManager gameManager, int loadedLevel) {
		levels = levelList.GetComponentsInChildren<Level> (true);
		levels [loadedLevel].OnLevelStart ();
	}

	/*
	 * Notify message syntax:
	 * 		1. char = for GameManager parsing
	 * 		2. char = which method should be called
	 * 			0 = Enemy destroyed
	 * 			1 = Player Hit
	 * 			2 = Mission End
	 * 			3 = Medal Step
	 * 			4 = Secondary Task Successful
	 * 		
	 * 		other characters = method specific
	 */
	public void Notify (string message) {
		char secondChar = message.ToCharArray () [1];
		switch (secondChar) {
		case '0':
			break;
		}
	}

	public void NotifyEnemyDestroyed(int points, int loadedLevel) {
		scoreManager.NotifyEnemyDestroyed (points);
		enemyManager.NotifyEnemyDestroyed ();
		levels [loadedLevel].NotifyEnemyDestroyed ();
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
