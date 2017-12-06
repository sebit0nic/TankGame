using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

	public static ObjectPool sharedInstance;

	public GameObject objectToPool;

	[Header("Multiple Objects in pool")]
	public bool multiplePools;
	public GameObject[] objectsToPool;
	public int amountToPool;

	private List<GameObject> pooledObjects;
	private List<KeyValuePair<int, GameObject>> multiplePooledObjects;
	private GameObject dynamicObjects;

	private void Awake() {
		sharedInstance = this;
	}

	//Only used for player projectile at runtime
	public void Init(GameObject template) {
		objectToPool = template;
	}

	private void Start() {
		dynamicObjects = GameObject.Find ("Dynamic Objects");

		if (multiplePools) {
			multiplePooledObjects = new List<KeyValuePair<int, GameObject>> ();

			for (int i = 0; i < objectsToPool.Length; i++) {
				for (int j = 0; j < amountToPool; j++) {
					GameObject obj = (GameObject)Instantiate (objectsToPool[i], transform.position, Quaternion.identity);
					obj.transform.parent = dynamicObjects.transform;
					obj.SetActive(false);
					multiplePooledObjects.Add (new KeyValuePair<int, GameObject> (i, obj));
				}
			}
		} else {
			pooledObjects = new List<GameObject> ();

			for (int i = 0; i < amountToPool; i++) {
				GameObject obj = (GameObject)Instantiate (objectToPool, transform.position, Quaternion.identity);
				obj.transform.parent = dynamicObjects.transform;
				obj.SetActive (false);
				pooledObjects.Add (obj);
			}
		}
	}

	public GameObject GetPooledObjects() {
		for (int i = 0; i < pooledObjects.Count; i++) {
			if (!pooledObjects [i].activeInHierarchy) {
				return pooledObjects [i];
			}
		}

		//Create a new object if we run out of pooled ones
		GameObject obj = (GameObject)Instantiate (objectToPool);
		obj.transform.parent = dynamicObjects.transform;
		obj.SetActive (false);
		pooledObjects.Add (obj);
		return obj;
	}

	public GameObject GetPooledObjectByIndex(int index) {
		foreach (KeyValuePair<int, GameObject> kvp in multiplePooledObjects) {
			if (!kvp.Value.activeInHierarchy && kvp.Key == index) {
				return kvp.Value;
			}
		}

		GameObject obj = (GameObject)Instantiate (objectsToPool[index], transform.position, Quaternion.identity);
		obj.transform.parent = dynamicObjects.transform;
		obj.SetActive(false);
		multiplePooledObjects.Add (new KeyValuePair<int, GameObject> (index, obj));
		return obj;
	}
}
