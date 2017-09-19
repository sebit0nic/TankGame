using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

	private EnemySpawnManager enemySpawnManager;

	public void Init(EnemySpawnManager enemySpawnManager) {
		this.enemySpawnManager = enemySpawnManager;
	}

	public void NotifyEnemyDestroyed() {
		enemySpawnManager.NotifyEnemyDestroyed ();
	}
}
