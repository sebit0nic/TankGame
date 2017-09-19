using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	
public class EnemySpawner : MonoBehaviour {

	public int maxSpawnedEnemies = 5;
	public float firstSpawnedEnemy;
	public float startSpawnRate = 3f;
	public GameObject[] activatedSpawner;

	[System.Serializable]
	public struct SpawnRateManager {
		public int remainingEnemies;
		public float newSpawnRate;
	}
	public SpawnRateManager[] spawnRateManager;

	private ObjectPool objectPool;
	private float spawnTimer;
	private float currentSpawnRate;
	private int currentSpawnRateIndex;
	private bool canSpawn = true;
	public float tempTimeDifference;
	private EnemySpawnManager enemySpawnManager;

	private void Awake() {
		objectPool = GetComponent<ObjectPool> ();
	}

	private void Start() {
		enemySpawnManager = GetComponentInParent<EnemySpawnManager> ();

		currentSpawnRate = startSpawnRate;
		spawnTimer = Time.time + firstSpawnedEnemy;
		currentSpawnRateIndex = 0;
	}

	private void Update() {
		if (canSpawn) {
			if (Time.time > spawnTimer && maxSpawnedEnemies > 0) {
				GameObject enemy = objectPool.GetPooledObjects ();
				enemy.SetActive (true);
				enemy.transform.position = transform.position;

				spawnTimer = Time.time + currentSpawnRate;
				maxSpawnedEnemies--;

				enemySpawnManager.NotifyEnemySpawned ();
			}

			if (maxSpawnedEnemies == 0) {
				for (int i = 0; i < activatedSpawner.Length; i++) {
					activatedSpawner [i].SetActive (true);
				}
				gameObject.SetActive (false);
			}

			if (spawnRateManager.Length != 0 && currentSpawnRateIndex < spawnRateManager.Length) {
				if (maxSpawnedEnemies < spawnRateManager [currentSpawnRateIndex].remainingEnemies) {
					currentSpawnRate = spawnRateManager [currentSpawnRateIndex].newSpawnRate;
					currentSpawnRateIndex++;
				}
			}
		}
	}

	public void StopSpawning() {
		canSpawn = false;
		tempTimeDifference = spawnTimer - Time.time;
	}

	public void ResumeSpawning() {
		canSpawn = true;
		spawnTimer = Time.time + tempTimeDifference;
	}
}
