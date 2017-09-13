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

	private float startTime, endTime;
	private GameManager gameManager;
	private bool taskDone;

	public void Update() {
		if (!taskDone) {
			if (timeLimit != -1) {
				if (timedSurvival) {
					if (Time.time > endTime) {
						gameManager.NotifyMissionSuccessful ();
						taskDone = true;
					}
				} else {
					if (Time.time > endTime) {
						gameManager.NotifyMissionFailed ();
						taskDone = true;
					}
				}
				gameManager.UpdateTimer (endTime - Time.time);
			} else {
				gameManager.UpdateTimer (Time.time - startTime);
			}
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
		startTime = Time.time;
		taskDone = false;
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
