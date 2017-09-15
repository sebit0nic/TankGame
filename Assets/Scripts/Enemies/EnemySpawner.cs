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

	private void Awake() {
		objectPool = GetComponent<ObjectPool> ();
	}

	private void Start() {
		currentSpawnRate = startSpawnRate;
		spawnTimer = Time.time + firstSpawnedEnemy;
		currentSpawnRateIndex = 0;
	}

	private void Update() {
		if (Time.time > spawnTimer && maxSpawnedEnemies > 0) {
			GameObject enemy = objectPool.GetPooledObjects ();
			enemy.SetActive (true);
			enemy.transform.position = transform.position;

			spawnTimer = Time.time + currentSpawnRate;
			maxSpawnedEnemies--;
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
