using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public Text scoreText;
	public Combometer combometer;

	private int currentScore = 0;

	public void NotifyEnemyDestroyed(int points) {
		currentScore += (points * combometer.GetCurrentCombo());
		string scoreString = currentScore.ToString ();
		scoreText.text = scoreString.PadLeft (6, '0');

		combometer.ComboUp ();
	}
}
