using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public MapManager mapManager;
	public MissionManager missionManager;

	public int loadedLevel = 0;
	public GameObject gameOverPanel, missionSuccessfulPanel;
	public Animator ingameMenuAnimator, ingameCameraAnimator;
	public Text timerText;

	private bool missionSuccessful;

	private void Awake() {
		LoadLevel (loadedLevel);
		missionManager.Init (this, loadedLevel);
	}

	public void LoadLevel(int index) {
		mapManager.LoadLevel (index);
	}

	public void UpdateTimer(float timer) {
		timerText.text = Mathf.RoundToInt (timer).ToString();
	}

	public void NotifyEnemyDestroyed(int points) {
		missionManager.NotifyEnemyDestroyed (points);
	}

	public void NotifyPlayerHit(int health) {
		missionManager.NotifyPlayerHit (health);
	}

	public void NotifyPlayerDestroyed() {
		Time.timeScale = 0.2f;
		Time.fixedDeltaTime = 0.2f * 0.02f;
		ingameMenuAnimator.SetTrigger ("OnOutro");
		ingameCameraAnimator.SetTrigger ("OnOutro");
	}

	public void NotifyMissionSuccessful() {
		Time.timeScale = 0.2f;
		Time.fixedDeltaTime = 0.2f * 0.02f;
		ingameMenuAnimator.SetTrigger ("OnOutro");
		ingameCameraAnimator.SetTrigger ("OnOutro");
		missionSuccessful = true;
		missionManager.NotifyMissionEnd (loadedLevel);
	}

	public void NotifySecondaryTaskOneSuccessful() {
	}

	public void NotifySecondaryTaskTwoSuccessful() {
	}

	public void NotifySecondaryTaskThreeSuccessful() {
	}

	public void NotifyMissionFailed() {
		Time.timeScale = 0.2f;
		Time.fixedDeltaTime = 0.2f * 0.02f;
		ingameMenuAnimator.SetTrigger ("OnOutro");
		ingameCameraAnimator.SetTrigger ("OnOutro");
	}

	public void NotifyCameraOutroAnimationFinished() {
		Time.timeScale = 0f;
		if (missionSuccessful) {
			missionSuccessfulPanel.SetActive (true);
		} else {
			gameOverPanel.SetActive (true);
		}
	}

	public void NotifyRestartScene() {
		Time.timeScale = 1;
		Time.fixedDeltaTime = 0.02f;
		SceneManager.LoadScene (0, LoadSceneMode.Single);
		missionManager.Init (this, loadedLevel);
	}

	public void NotifyStopGame() {
		if (!Application.isEditor) {
			System.Diagnostics.Process.GetCurrentProcess ().Kill ();
		}
	}
}
