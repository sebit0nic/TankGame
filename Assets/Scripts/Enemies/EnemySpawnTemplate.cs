using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnTemplate {
	
	//0 = Normal Missile Enemy, 1 = Exploding Enemy, 2 = Rotating Enemy
	public int[] spawnedEnemyType;
	public int[] affectedSpawners;
	public int[] spawnedAmount;
}
