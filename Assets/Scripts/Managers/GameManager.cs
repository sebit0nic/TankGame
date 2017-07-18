using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public MapManager mapManager;
	public MissionManager missionManager;

	public GameObject gameOverPanel;

	private void Awake() {
		LoadLevel (0);
	}

	public void LoadLevel(int index) {
		mapManager.LoadLevel (index);
	}

	public void NotifyEnemyDestroyed(int points) {
		missionManager.NotifyEnemyDestroyed (points);
	}

	public void NotifyPlayerDestroyed() {
		gameOverPanel.SetActive (true);
	}

	public void NotifyRestartScene() {
		SceneManager.LoadScene (0, LoadSceneMode.Single);
	}

	public void NotifyStopGame() {
		Application.Quit ();
	}
}
