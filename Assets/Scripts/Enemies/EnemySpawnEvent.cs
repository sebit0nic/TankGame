using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnEvent {

	public enum EventType {SPAWN, WAIT_FOR_ENEMYCOUNT};
	public EventType eventType;

	[Header("SPAWN")]
	public EnemySpawner[] enemySpawner;

	[Header("TIMEOUTS")]
	public float beforeTimeout;
	public float afterTimeout;

	[Header("WAIT_FOR_ENEMYCOUNT")]
	public int enemyCount;
}
