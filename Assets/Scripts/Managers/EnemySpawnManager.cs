using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour {

	public EnemyManager enemyManager;
	public int concurrentAllowedEnemies;
	public EnemySpawner[] enemySpawners;

	private int currentConcurrentEnemies;

	private void Start() {
		enemyManager.Init (this);
	}

	public void NotifyEnemySpawned() {
		currentConcurrentEnemies++;

		if (currentConcurrentEnemies >= concurrentAllowedEnemies) {
			for (int i = 0; i < enemySpawners.Length; i++) {
				enemySpawners [i].StopSpawning ();
			}
		}
	}

	public void NotifyEnemyDestroyed() {
		currentConcurrentEnemies--;

		if (currentConcurrentEnemies < concurrentAllowedEnemies) {
			for (int i = 0; i < enemySpawners.Length; i++) {
				enemySpawners [i].ResumeSpawning ();
			}
		}
	}
}
