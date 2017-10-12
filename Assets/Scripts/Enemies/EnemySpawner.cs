using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	
public class EnemySpawner : MonoBehaviour {

	private ObjectPool objectPool;

	private void Awake() {
		objectPool = GetComponent<ObjectPool> ();
	}

	public void Spawn() {
		GameObject enemy = objectPool.GetPooledObjects ();
		enemy.SetActive (true);
		enemy.transform.position = transform.position;
	}
}
