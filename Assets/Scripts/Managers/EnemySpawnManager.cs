using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour {

	public EnemyManager enemyManager;
	public int concurrentAllowedEnemies;
	public EnemySpawnEvent[] events;

	private int currentConcurrentEnemies;
	private int eventPointer;
	private bool stoppedForEnemyCount, stoppedForWave;

	private void Start() {
		enemyManager.Init (this);
		NextEvent ();
	}

	private void NextEvent() {
		if (eventPointer < events.Length) {
			switch (events [eventPointer].eventType) {
			case EnemySpawnEvent.EventType.SPAWN:
				StartCoroutine (Spawn (events [eventPointer].enemySpawner, events[eventPointer].beforeTimeout, events[eventPointer].afterTimeout));
				break;
			case EnemySpawnEvent.EventType.WAIT_FOR_ENEMYCOUNT:
				StartCoroutine (WaitForEnemycount (events [eventPointer].beforeTimeout));
				break;
			}
		}
	}

	private IEnumerator Spawn(EnemySpawner[] enemySpawner, float beforeTimeout, float afterTimeout) {
		yield return new WaitForSeconds (beforeTimeout);
		for (int i = 0; i < enemySpawner.Length; i++) {
			enemySpawner[i].Spawn ();
			currentConcurrentEnemies++;
		}

		StartCoroutine (WaitAfterEvent (afterTimeout));
	}

	private IEnumerator WaitForEnemycount(float beforeTimeout) {
		yield return new WaitForSeconds (beforeTimeout);
		stoppedForEnemyCount = true;
	}

	private IEnumerator WaitAfterEvent(float afterTimeout) {
		yield return new WaitForSeconds (afterTimeout);
		eventPointer++;
		NextEvent ();
	}

	public void NotifyEnemyDestroyed() {
		currentConcurrentEnemies--;

		if (stoppedForEnemyCount) {
			if (currentConcurrentEnemies <= events [eventPointer].enemyCount) {
				stoppedForEnemyCount = false;
				StartCoroutine (WaitAfterEvent (events[eventPointer].afterTimeout));
			}
		}
	}
}
