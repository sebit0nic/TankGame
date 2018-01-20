using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	
public class EnemySpawner : MonoBehaviour {

	public Vector3 minSpawnPosition, maxSpawnPosition;

	private ObjectPool objectPool;

	private void Awake() {
		objectPool = GetComponent<ObjectPool> ();
	}

	public bool Spawn(SpawnLevel.EnemyTypes enemyType) {
		if (transform.position.x < minSpawnPosition.x || transform.position.x > maxSpawnPosition.x || 
			transform.position.z < minSpawnPosition.z || transform.position.z > maxSpawnPosition.z) {
			Debug.Log (gameObject.name + " skipped because it's outside of the tolerance!");
			return false;
		} else {
			GameObject enemy = null;

			switch (enemyType) {
			case SpawnLevel.EnemyTypes.NORMAL_SHOOTING_ENEMY:
				enemy = objectPool.GetPooledObjectByIndex (0);
				break;
			case SpawnLevel.EnemyTypes.EXPLODING_ENEMY:
				enemy = objectPool.GetPooledObjectByIndex (1);
				break;
			case SpawnLevel.EnemyTypes.ROTATING_SHOOTING_ENEMY:
				enemy = objectPool.GetPooledObjectByIndex (2);
				break;
			}

			enemy.transform.position = transform.position;
			enemy.SetActive (true);
			return true;
		}
	}

	public bool Spawn(int index) {
		if (transform.position.x < minSpawnPosition.x || transform.position.x > maxSpawnPosition.x || 
			transform.position.z < minSpawnPosition.z || transform.position.z > maxSpawnPosition.z) {
			Debug.Log (gameObject.name + " skipped because it's outside of the tolerance!");
			return false;
		} else {
			GameObject enemy = objectPool.GetPooledObjectByIndex (index);
			enemy.transform.position = transform.position;
			enemy.SetActive (true);
			return true;
		}
	}
}
