using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

	public GameObject tree1, tree2, tree3, grass1;

	private void Start() {
		SpawnEnvironmentAroundPlayer ();
	}

	private void SpawnEnvironmentAroundPlayer() {
		for (int i = -200; i < 200; i += 4) {
			for (int j = -200; j < 200; j += 4) {
				int random = Random.Range (0, 500);
				if (random <= 1) {
					GameObject newTree = Instantiate (tree1) as GameObject;
					newTree.transform.position = new Vector3 (i, 0, j);
				} else if (random > 2 && random <= 3) {
					GameObject newTree = Instantiate (tree2) as GameObject;
					newTree.transform.position = new Vector3 (i, 0, j);
				} else if (random > 3 && random <= 4) {
					GameObject newTree = Instantiate (tree3) as GameObject;
					newTree.transform.position = new Vector3 (i, 0, j);
				} else if (random > 4 && random <= 30) {
					GameObject newTree = Instantiate (grass1) as GameObject;
					newTree.transform.position = new Vector3 (i, 0, j);
				}
			}
		}
	}
}
