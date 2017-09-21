using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : MonoBehaviour, Level {

	public PlayerHealth playerHealth;
	//TODO: Add a "Treelist" to watch how many trees have been destroyed for secondary task 2
	public ScoreManager scoreManager;
	public MissionManager missionManager;

	public int medalStep1, medalStep2, medalStep3;

	private PrimaryTask primaryTask;
	private bool ninerCombo = false;
	private bool medal1Got, medal2Got, medal3Got;

	private void Start() {
		primaryTask = GetComponent<PrimaryTask> ();
	}

	private void FixedUpdate() {
		if (scoreManager.currentCombo >= 9) {
			ninerCombo = true;
		}

		if (scoreManager.currentScore >= medalStep1 && !medal1Got) {
			medal1Got = true;
			missionManager.NotifyMedalStep (0);
		}
		if (scoreManager.currentScore >= medalStep2 && !medal2Got) {
			medal2Got = true;
			missionManager.NotifyMedalStep (1);
		}
		if (scoreManager.currentScore >= medalStep3 && !medal3Got) {
			medal3Got = true;
			missionManager.NotifyMedalStep (2);
		}
	}

	public void OnLevelStart () {
	}

	public void NotifyEnemyDestroyed() {
		primaryTask.NotifyEnemyDestroyed ();
	}

	public void OnLevelEnd () {
		if (playerHealth.currentHealth == 3) {
			missionManager.NotifySecondaryTaskSuccessful (0);
		}

		//TODO: Implement secondary task 2

		if (ninerCombo) {
			missionManager.NotifySecondaryTaskSuccessful (2);
		}
	}
}
