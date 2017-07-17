using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour {

	public GameObject[] healthSections;

	public void UpdateHealthbar (int currentHealth) {
		foreach (GameObject g in healthSections) {
			g.SetActive (false);
		}

		for (int i = 0; i < currentHealth; i++) {
			healthSections [i].SetActive (true);
		}
	}
}
