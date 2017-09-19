using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventSuccessfulScreen : MonoBehaviour {

	public Text scoreText;
	public Image medal1Img, medal2Img, medal3Img;
	public Text task1Text, task2Text, task3Text;

	private int score;
	private bool medal1, medal2, medal3;
	private bool task1, task2, task3;

	public void Init(int score, bool medal1, bool medal2, bool medal3, bool task1, bool task2, bool task3) {
		this.score = score;
		this.medal1 = medal1;
		this.medal2 = medal2;
		this.medal3 = medal3;
		this.task1 = task1;
		this.task2 = task2;
		this.task3 = task3;

		scoreText.text = score.ToString();

		if (medal1) {
			medal1Img.color = Color.yellow;
		}
		if (medal2) {
			medal2Img.color = Color.yellow;
		}
		if (medal3) {
			medal3Img.color = Color.yellow;
		}

		if (task1) {
			task1Text.color = Color.green;
		}
		if (task2) {
			task2Text.color = Color.green;
		}
		if (task3) {
			task3Text.color = Color.green;
		}
	}
}
