using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyManager : MonoBehaviour {

	public GameObject nextDummyManager;
    public int dummyAmount;

	public void NotifyDummyDestroyed() {
		dummyAmount--;
		if (dummyAmount == 0) {
			if (nextDummyManager != null) {
				nextDummyManager.SetActive (true);
			}
			gameObject.SetActive (false);
		}
	}
}
