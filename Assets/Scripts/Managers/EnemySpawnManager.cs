using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour {

	public EnemyManager enemyManager;
	public SpawnLevel[] spawnLevels;
	public EnemySpawner[] spawners;

	private int currentSpawnLevel = 0;
	private int currentConcurrentEnemies;
	private int concurrentAllowedEnemies;
	private float spawnTimer;
	private bool canSpawn = true;

	private void Start() {
		enemyManager.Init (this);
		EvaluateSpawnLevel ();
	}

	private void Update() {
		if (currentSpawnLevel + 1 < spawnLevels.Length) {
			if (Time.time > spawnLevels [currentSpawnLevel + 1].unlockTime) {
				currentSpawnLevel++;
				EvaluateSpawnLevel ();
			}
		}

		if (Time.time > spawnTimer && canSpawn) {
			int randomEnemy = spawnLevels [currentSpawnLevel].GetRandomEnemyType ();
			Spawn (spawnLevels[currentSpawnLevel].enemyTypes[randomEnemy]);
			spawnTimer = Time.time + Random.Range (spawnLevels [currentSpawnLevel].minSpawnRate, spawnLevels [currentSpawnLevel].maxSpawnRate);
		}
	}

	private void Spawn(SpawnLevel.EnemyTypes enemyType) {
		currentConcurrentEnemies++;
		int randomSpawner = Random.Range (0, spawners.Length);
		spawners [randomSpawner].Spawn (enemyType);

		if (currentConcurrentEnemies >= concurrentAllowedEnemies) {
			canSpawn = false;
		}
	}

	private void EvaluateSpawnLevel() {
		concurrentAllowedEnemies = Random.Range (spawnLevels [currentSpawnLevel].minConcurrentEnemies, spawnLevels [currentSpawnLevel].maxConcurrentEnemies);
		spawnTimer = Time.time + Random.Range (spawnLevels [currentSpawnLevel].minSpawnRate, spawnLevels [currentSpawnLevel].maxSpawnRate);
	}

	public void NotifyEnemyDestroyed() {
		currentConcurrentEnemies--;
		if (currentConcurrentEnemies < concurrentAllowedEnemies) {
			canSpawn = true;
		}
	}
}
