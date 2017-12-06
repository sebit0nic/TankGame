using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class PrimaryTask : MonoBehaviour {

	[Tooltip("How many enemies are needed to be destroyed to complete this task? -1...modifier disabled")]
	public int enemyCount;
	[Tooltip("Is there a time constraint to complete this task? -1...modifier disabled")]
	public float timeLimit;
	public bool timedSurvival;

	private float startTime, endTime;
	private GameManager gameManager;
	private bool taskDone;

	private void Start() {
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();

		if (timeLimit != -1) {
			endTime = Time.time + timeLimit;
		}
		startTime = Time.time;
	}

	public void Update() {
		if (!taskDone) {
			if (timeLimit != -1) {
				if (timedSurvival) {
					if (Time.time > endTime) {
						//gameManager.NotifyMissionSuccessful ();
						taskDone = true;
					}
				} else {
					if (Time.time > endTime) {
						//gameManager.NotifyMissionFailed ();
						taskDone = true;
					}
				}
				gameManager.UpdateTimer (endTime - Time.time);
			} else {
				gameManager.UpdateTimer (Time.time - startTime);
			}
		}
	}

	public void NotifyEnemyDestroyed() {
		if (enemyCount != -1) {
			enemyCount--;
			if (enemyCount == 0) {
				//gameManager.NotifyMissionSuccessful ();
			}
		}
	}
}
