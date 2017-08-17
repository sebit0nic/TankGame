using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicTutorialLevel : MonoBehaviour {

	public GameObject[] disabledHUD;
	public Animator[] instructionKeys;
	public GameObject firstDummy;

	private CameraFollow cameraFollow;
	private PlayerShootingScript playerShootingScript;
	private float timeMoved, timeMovedExpected = 3;
	private bool canAim;

	private void Start() {
		cameraFollow = Camera.main.GetComponent<CameraFollow> ();
		cameraFollow.SetFollowMouse (false);

		playerShootingScript = GameObject.Find ("Player").GetComponent<PlayerShootingScript> ();
		playerShootingScript.enabled = false;

		for (int i = 0; i < disabledHUD.Length; i++) {
			disabledHUD [i].SetActive (false);
		}
	}

	private void FixedUpdate() {
		float vertical = Input.GetAxis ("Vertical");
		float horizontal = Input.GetAxis ("Horizontal");

		if (vertical != 0 || horizontal != 0) {
			timeMoved += Time.fixedDeltaTime;
		}

		if (timeMoved > timeMovedExpected && !canAim) {
			for (int i = 0; i < instructionKeys.Length; i++) {
				instructionKeys [i].SetTrigger ("OnOutro");
			}
			cameraFollow.SetFollowMouse (true);
			playerShootingScript.enabled = true;
			firstDummy.SetActive (true);
			canAim = true;
		}
	}
}
