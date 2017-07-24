using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

[System.Serializable]
public class PrimaryTask {

	public string description;
	[Tooltip("How many enemies are needed to be destroyed to complete this task? -1...modifier disabled")]
	public int enemyCount;
	[Tooltip("Is there a time constraint to complete this task? -1...modifier disabled")]
	public float timeLimit;
	public bool timedSurvival;

	private float endTime;
	private GameManager gameManager;

	public void Update() {
		if (timeLimit != -1) {
			if (timedSurvival) {
				if (Time.time > endTime) {
					gameManager.NotifyMissionSuccessful ();
				}
			} else {
				if (Time.time > endTime) {
					//TODO: check if task has been completed yet (prevent last second failure of task)
					gameManager.NotifyMissionFailed ();
				}
			}

			gameManager.UpdateTimer (endTime - Time.time);
		} else {
			gameManager.UpdateTimer (Time.time);
		}
	}

	public void Init(GameManager gameManager, XmlNode primaryTask) {
		description = primaryTask.ChildNodes [0].InnerText;
		enemyCount = XmlConvert.ToInt32 (primaryTask.ChildNodes [1].InnerText);
		timeLimit = XmlConvert.ToInt32 (primaryTask.ChildNodes [2].InnerText);
		timedSurvival = XmlConvert.ToBoolean (primaryTask.ChildNodes [3].InnerText);

		if (timeLimit != -1) {
			endTime = Time.time + timeLimit;
		}

		this.gameManager = gameManager;
	}

	public void NotifyEnemyDestroyed() {
		if (enemyCount != -1) {
			enemyCount--;
			if (enemyCount == 0) {
				gameManager.NotifyMissionSuccessful ();
			}
		}
	}

	public void NotifyTargetEnemyDestroyed() {
		gameManager.NotifyMissionSuccessful ();
	}

	public void NotifyEscortableReachedGoal() {
		gameManager.NotifyMissionSuccessful ();
	}

	public void NotifyPlayerReachedGoal() {
		gameManager.NotifyMissionSuccessful ();
	}
}
