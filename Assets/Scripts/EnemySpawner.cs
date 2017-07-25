using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public ObjectPool objectPool;
	public int maxSpawnedEnemies = 5;
	public float spawnRate = 3f;
	public bool hasPlayerTrigger;

	private float spawnTimer;

	private void Start() {
		if (!hasPlayerTrigger) {
			spawnTimer = Time.time + spawnRate;
		}
	}

	private void Update() {
		if (Time.time > spawnTimer && maxSpawnedEnemies > 0) {
			GameObject enemy = objectPool.GetPooledObjects ();
			enemy.SetActive (true);
			enemy.transform.position = transform.position;

			spawnTimer = Time.time + spawnRate;
			maxSpawnedEnemies--;
		}
	}
}
