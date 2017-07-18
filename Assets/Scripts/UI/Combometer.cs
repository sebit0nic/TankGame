using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combometer : MonoBehaviour {

	public float comboDropTime = 2f;
	public Text comboText;

	private int currentCombo = 1;
	private float comboDropTimer;
	private Image radialCombometer;
	private float fillAmount;

	private void Awake() {
		radialCombometer = GetComponent<Image> ();
	}
		
	private void Update() {
		if (comboDropTimer > Time.time) {
			fillAmount = (comboDropTimer - Time.time) / comboDropTime;
			radialCombometer.fillAmount = fillAmount;
		} else {
			ComboDown ();
		}
	}

	public void ComboUp() {
		if (currentCombo < 9) {
			currentCombo++;
		}
		comboDropTimer = Time.time + comboDropTime;
		comboText.text = "x" + currentCombo;
	}

	public void ComboDown() {
		if (currentCombo != 1) {
			currentCombo--;
			if (currentCombo != 1) {
				comboDropTimer = Time.time + comboDropTime;
			}
		} else {
			radialCombometer.fillAmount = 1;
		}

		comboText.text = "x" + currentCombo;
	}

	public int GetCurrentCombo() {
		return currentCombo;
	}
}
