using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour {

	public Text healthText;

	public void UpdateUI (float currentHealth) {
		int healthRaw = Mathf.RoundToInt (currentHealth);
		healthText.text = "" + healthRaw;
	}
}
