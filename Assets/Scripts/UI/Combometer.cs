using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combometer : MonoBehaviour {

	public Text comboText;

	private Image radialCombometer;
	private float fillAmount;

	private void Awake() {
		radialCombometer = GetComponent<Image> ();
	}

	public void UpdateUI(float comboDropTimer, float comboDropTime, int currentCombo) {
		if (currentCombo == 1) {
			radialCombometer.fillAmount = 1;
		} else {
			fillAmount = (comboDropTimer - Time.time) / comboDropTime;
			radialCombometer.fillAmount = fillAmount;
		}

		comboText.text = "x" + currentCombo;
	}
}
