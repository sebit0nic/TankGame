using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public Text scoreText;
	public float comboDropTime = 2f;
	public Combometer combometer;

	private int currentScore = 0;
	private int currentCombo = 1;
	private float comboDropTimer;

	public void NotifyEnemyDestroyed(int points) {
		currentScore += (points * currentCombo);
		string scoreString = currentScore.ToString ();
		scoreText.text = scoreString.PadLeft (6, '0');

		ComboUp ();
	}

	public void NotifyPlayerHit() {
		currentCombo = 2;
		ComboDown ();
	}

	private void Update() {
		if (comboDropTimer < Time.time) {
			ComboDown ();
		} else {
			combometer.UpdateUI (comboDropTimer, comboDropTime, currentCombo);
		}
	}

	private void ComboUp() {
		if (currentCombo < 9) {
			currentCombo++;
		}
		comboDropTimer = Time.time + comboDropTime;
	}

	private void ComboDown() {
		if (currentCombo != 1) {
			currentCombo--;
			if (currentCombo != 1) {
				comboDropTimer = Time.time + comboDropTime;
			}
		} else {
			combometer.UpdateUI (comboDropTimer, comboDropTime, currentCombo);
		}
	}
}
