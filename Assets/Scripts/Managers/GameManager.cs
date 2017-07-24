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
	public Text timerText;

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
		Time.timeScale = 0;
		gameOverPanel.SetActive (true);
	}

	public void NotifyMissionSuccessful() {
		Time.timeScale = 0;
		missionSuccessfulPanel.SetActive (true);
	}

	public void NotifyMissionFailed() {
		Time.timeScale = 0;
		gameOverPanel.SetActive (true);
	}

	public void NotifyRestartScene() {
		Time.timeScale = 1;
		SceneManager.LoadScene (0, LoadSceneMode.Single);
		missionManager.Init (this, loadedLevel);
	}

	public void NotifyStopGame() {
		Application.Quit ();
	}
}
