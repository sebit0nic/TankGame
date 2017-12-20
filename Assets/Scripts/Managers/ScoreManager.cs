using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public Image heatMeter;
	public Text scoreText;
	public float heatDropFactor = 5;
	public int currentScore = 0;
	public float heatDropPauseTime = 1;

	private float currentHeatFactor = 1;
	private bool canDrop = true;

	private void Update() {
		heatMeter.fillAmount = currentHeatFactor / 100;
		if (canDrop) {
			currentHeatFactor -= heatDropFactor * Time.deltaTime;
		}

		currentHeatFactor = Mathf.Clamp (currentHeatFactor, 1, 100);
	}

	public void NotifyEnemyDestroyed(int points) {
		currentScore += points * (int)currentHeatFactor;
		scoreText.text = currentScore.ToString ();
		currentHeatFactor += points * 2;
		StartCoroutine (WaitForDropPause ());
	}

	public void NotifyPlayerHealthUpdate(bool damaged) {
		if (damaged) {
			currentHeatFactor = currentHeatFactor / 3;
		}
	}

	private IEnumerator WaitForDropPause() {
		canDrop = false;
		yield return new WaitForSeconds (heatDropPauseTime);
		canDrop = true;
	}

	public float GetCurrentHeatFactor() {
		return currentHeatFactor;
	}
}
