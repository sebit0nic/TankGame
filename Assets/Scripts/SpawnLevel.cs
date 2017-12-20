using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnLevel {

	public float unlockTime;
	public int minConcurrentEnemies, maxConcurrentEnemies;
	public float minSpawnRate, maxSpawnRate;
	public enum EnemyTypes {NORMAL_SHOOTING_ENEMY, EXPLODING_ENEMY, ROTATING_SHOOTING_ENEMY}
	public EnemyTypes[] enemyTypes;
	public int[] enemyOccurencePossibility;

	public int GetRandomEnemyType() {
		int randomNum = Random.Range (0, 100);
		for (int i = 0; i < enemyOccurencePossibility.Length; i++) {
			if (randomNum <= enemyOccurencePossibility [i]) {
				return i;
			}
		}

		return 0;
	}
}
