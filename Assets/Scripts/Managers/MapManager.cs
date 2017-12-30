using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

	public GameObject tree1, tree2, tree3, grass1;
	public float tree1MinPerl = 0.1f, tree1MaxPerl = 0.15f;
	public float tree2MinPerl = 0.15f, tree2MaxPerl = 0.2f;
	public float tree3MinPerl = 0.2f, tree3MaxPerl = 0.25f;

	public Vector3 minSpawnSafezone, maxSpawnSafezone;

	private void Start() {
		SpawnEnvironmentAroundPlayer ();
	}

	private void SpawnEnvironmentAroundPlayer() {
		float randomPerlinX = Random.Range (0.1f, 0.99f);
		float randomPerlinY = Random.Range (0.1f, 0.99f);
		for (int i = -200; i < 200; i += 5) {
			for (int j = -200; j < 200; j += 5) {
				if ((i >= minSpawnSafezone.x && i <= maxSpawnSafezone.x) && (j >= minSpawnSafezone.z && j <= maxSpawnSafezone.z)) {
					continue;
				}

				float perlinValue = Mathf.PerlinNoise (i + randomPerlinX, j + randomPerlinY);
				float randomOffsetX = Random.Range (-2f, 2f);
				float randomOffsetY = Random.Range (-2f, 2f);
				int randomDiceRoll = Random.Range (0, 8);
				if (randomDiceRoll == 0) {
					if (perlinValue < tree1MaxPerl && perlinValue >= tree1MinPerl) {
						GameObject newTree = Instantiate (tree1) as GameObject;
						newTree.transform.position = new Vector3 (i + randomOffsetX, 0, j + randomOffsetY);
					} else if (perlinValue < tree2MaxPerl && perlinValue >= tree2MinPerl) {
						GameObject newTree = Instantiate (tree2) as GameObject;
						newTree.transform.position = new Vector3 (i + randomOffsetX, 0, j + randomOffsetY);
					} else if (perlinValue < tree3MaxPerl && perlinValue >= tree3MinPerl) {
						GameObject newTree = Instantiate (tree3) as GameObject;
						newTree.transform.position = new Vector3 (i + randomOffsetX, 0, j + randomOffsetY);
					}
				}
			}
		}
	}
}
