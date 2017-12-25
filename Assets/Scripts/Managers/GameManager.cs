using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager INSTANCE;

	public MapManager mapManager;
	public MissionManager missionManager;

	public int loadedLevel = 0;
	public GameObject gameOverPanel, missionSuccessfulPanel;
	public Animator ingameCameraAnimator;

	private char playerState = 'N';

	private void Awake() {
		INSTANCE = this;
	}

	public static GameManager GetInstance() {
		if (INSTANCE == null) {
			INSTANCE = new GameManager ();
		}

		return INSTANCE;
	}

	public void NotifyEnemyDestroyed(int points) {
		missionManager.NotifyEnemyDestroyed (points, loadedLevel);
	}

	public void NotifyPlayerHealthUpdate(float health, bool damaged) {
		missionManager.NotifyPlayerHealthUpdate (health, damaged);
	}

	public void NotifyPlayerDestroyed() {
		Time.timeScale = 0.2f;
		Time.fixedDeltaTime = 0.2f * 0.02f;
		ingameCameraAnimator.SetTrigger ("OnOutro");
	}

	public void NotifyPlayerStateChanged(char newState) {
		playerState = newState;
	}

	public char GetPlayerState() {
		return playerState;
	}

	public float GetCurrentHeatFactor() {
		return missionManager.GetCurrentHeatFactor ();
	}

	public bool CanUseSpecialAbility() {
		return missionManager.CanUseSpecialAbility ();
	}

	public void NotifyUseSpecialAbility() {
		missionManager.NotifyUseSpecialAbility ();
	}

	public void NotifyCameraOutroAnimationFinished() {
		Time.timeScale = 1f;
		Time.fixedDeltaTime = 0.02f;
		gameOverPanel.SetActive (true);
	}

	public void NotifyRestartScene() {
		Time.timeScale = 1;
		Time.fixedDeltaTime = 0.02f;
		SceneManager.LoadScene (0, LoadSceneMode.Single);
	}

	public void NotifyStopGame() {
		if (!Application.isEditor) {
			System.Diagnostics.Process.GetCurrentProcess ().Kill ();
		}
	}
}
