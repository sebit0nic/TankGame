using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventSuccessfulScreen : MonoBehaviour {

	public Text scoreText;
	public Animator medal1Anim, medal2Anim, medal3Anim;
	public Animator task1Anim, task2Anim, task3Anim;

	private Animator thisAnimator;
	private int score;
	private bool medal1, medal2, medal3;
	private bool task1, task2, task3;
	private bool onIntro, onMedals;
	private float tempFloatScore, tempScore;

	private void Start() {
		thisAnimator = GetComponent<Animator> ();
	}

	public void Init(int score, bool medal1, bool medal2, bool medal3, bool task1, bool task2, bool task3) {
		this.score = score;
		this.medal1 = medal1;
		this.medal2 = medal2;
		this.medal3 = medal3;
		this.task1 = task1;
		this.task2 = task2;
		this.task3 = task3;
	}

	private void Update() {
		if (onIntro) {
			if (tempScore < score) {
				tempFloatScore += Time.unscaledDeltaTime * score;
				tempScore = Mathf.RoundToInt (tempFloatScore);
				scoreText.text = tempScore.ToString ();
			} else {
				onIntro = false;
				scoreText.text = score.ToString ();
				StartCoroutine (NextAnimationStep (0.5f));
			}
		}
	}

	private IEnumerator NextAnimationStep(float time) {
		yield return new WaitForSecondsRealtime (time);
		thisAnimator.SetTrigger ("OnMedalView");
	}

	private IEnumerator OnMedalView(float time) {
		if (medal1) {
			medal1Anim.SetTrigger ("OnTurn");
		}
		if (medal2) {
			medal2Anim.SetTrigger ("OnTurn");
		}
		if (medal3) {
			medal3Anim.SetTrigger ("OnTurn");
		}

		yield return new WaitForSecondsRealtime (time);
		thisAnimator.SetTrigger ("OnSecondaryTaskView");
	}

	private IEnumerator OnTaskView(float time) {
		if (task1) {
			task1Anim.SetTrigger ("OnStrike");
		}
		if (task2) {
			task2Anim.SetTrigger ("OnStrike");
		}
		if (task3) {
			task3Anim.SetTrigger ("OnStrike");
		}

		yield return new WaitForSecondsRealtime (time);
		thisAnimator.SetTrigger ("OnButtonView");
	}

	public void OnIntro() {
		onIntro = true;
	}

	public void OnMedals() {
		StartCoroutine (OnMedalView (1.5f));
	}

	public void OnTasks() {
		StartCoroutine (OnTaskView (1.5f));
	}
}
