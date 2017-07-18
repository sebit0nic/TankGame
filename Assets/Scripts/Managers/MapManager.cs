using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

	public GameObject[] levels;

	public void LoadLevel(int index) {
		levels [index].SetActive (true);
	}
}
