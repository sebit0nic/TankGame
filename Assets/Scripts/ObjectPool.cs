using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

	public static ObjectPool sharedInstance;

	public GameObject objectToPool;
	public int amountToPool;

	private List<GameObject> pooledObjects;
	private GameObject dynamicObjects;

	private void Awake() {
		sharedInstance = this;
	}

	public void Init(GameObject template) {
		objectToPool = template;
	}

	private void Start() {
		dynamicObjects = GameObject.Find ("Dynamic Objects");
		pooledObjects = new List<GameObject> ();

		for (int i = 0; i < amountToPool; i++) {
			GameObject obj = (GameObject)Instantiate (objectToPool, transform.position, Quaternion.identity);
			obj.transform.parent = dynamicObjects.transform;
			obj.SetActive (false);
			pooledObjects.Add (obj);
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
}
