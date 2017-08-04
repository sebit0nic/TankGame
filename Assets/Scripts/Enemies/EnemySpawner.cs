using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public int maxSpawnedEnemies = 5;
	public float spawnRate = 3f;
	public bool hasPlayerTrigger;
	public GameObject[] activatedSpawner;

	private ObjectPool objectPool;
	private float spawnTimer;

	private void Awake() {
		objectPool = GetComponent<ObjectPool> ();
	}

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

		if (maxSpawnedEnemies == 0) {
			for (int i = 0; i < activatedSpawner.Length; i++) {
				activatedSpawner [i].SetActive (true);
			}
			gameObject.SetActive (false);
		}
	}
}
