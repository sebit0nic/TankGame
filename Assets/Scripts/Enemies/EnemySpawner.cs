using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	
public class EnemySpawner : MonoBehaviour {

	private ObjectPool objectPool;

	private void Awake() {
		objectPool = GetComponent<ObjectPool> ();
	}

	public void Spawn(SpawnLevel.EnemyTypes enemyType) {
		GameObject enemy = null;

		switch (enemyType) {
		case SpawnLevel.EnemyTypes.NORMAL_SHOOTING_ENEMY:
			enemy = objectPool.GetPooledObjectByIndex (0);
			break;
		case SpawnLevel.EnemyTypes.EXPLODING_ENEMY:
			enemy = objectPool.GetPooledObjectByIndex (1);
			break;
		}

		enemy.transform.position = transform.position;
		enemy.SetActive (true);
	}
}
