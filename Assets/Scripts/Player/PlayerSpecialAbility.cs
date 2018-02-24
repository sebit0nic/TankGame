using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialAbility : MonoBehaviour {

	public enum AbilityType {DODGE_JUMP, BULLET_STOP, SHIELD, SHOCKWAVE, TIME_STOP};
	public AbilityType currentAbilityType;
	public ParticleSystem jumpParticles;
	public GameObject dynamicObjects;
	public bool alwaysAbilityAvailable;

	[Header("DODGE_JUMP Variables")]
	public float upwardsThrust = 125000f;
	public float sidewaysThrust = 50000f;
	public float downwardsThrust = -90000f;
	public float airTime = 0.5f;
	public float fallTime = 1.2f;

	[Header("BULLET_STOP Variables")]


	private Rigidbody thisRigidbody;
	private Animator thisAnimator;
	private PlayerMovement playerMovement;
	private PlayerShootingScript playerShootingScript;
	private BoxCollider playerBoxCollider;
	private GameManager gameManager;

	private bool usesAbility = false;

	private void Start() {
		thisRigidbody = GetComponent<Rigidbody> ();
		thisAnimator = GetComponentInParent<Animator> ();
		playerMovement = GetComponent<PlayerMovement> ();
		playerShootingScript = GetComponent<PlayerShootingScript> ();
		playerBoxCollider = GetComponent<BoxCollider> ();
		gameManager = GameManager.GetInstance ();
	}

	private void Update () {
		if (Input.GetKeyDown (KeyCode.Mouse1) && !usesAbility) {
			switch (currentAbilityType) {
			case AbilityType.DODGE_JUMP:
				DoDodgeJump ();
				break;
			case AbilityType.BULLET_STOP:
				DoBulletStop ();
				break;
			case AbilityType.SHIELD:
				DoShield ();
				break;
			}
		}
	}

	private void DoDodgeJump() {
		if (gameManager.CanUseSpecialAbility () || alwaysAbilityAvailable) {
			usesAbility = true;
			thisAnimator.SetTrigger ("OnDodgeJump");
			jumpParticles.Play ();
			StartCoroutine (DodgeJumpCoroutine());
			gameManager.NotifyUseSpecialAbility ();
		}
	}

	private void DoBulletStop() {
		if (gameManager.CanUseSpecialAbility () || alwaysAbilityAvailable) {
			
		}
	}

	private void DoShield() {
		//TODO: Implement Shield
	}

	private IEnumerator DodgeJumpCoroutine() {
		playerMovement.SetUsesAbility (true);
		playerBoxCollider.enabled = false;
		playerShootingScript.SetCanShoot (false);
		gameManager.NotifyPlayerStateChanged ('A');
		thisRigidbody.AddForce (new Vector3(Input.GetAxis("Horizontal") * sidewaysThrust, upwardsThrust, Input.GetAxis("Vertical") * sidewaysThrust), ForceMode.Impulse);

		yield return new WaitForSeconds (airTime);

		thisRigidbody.AddForce (new Vector3(0, downwardsThrust, 0), ForceMode.Impulse);

		yield return new WaitForSeconds (fallTime);

		playerBoxCollider.enabled = true;
		playerMovement.SetUsesAbility (false);
		usesAbility = false;
		gameManager.NotifyPlayerStateChanged ('N');

		//Wait for a little bit more before enabling shooting script
		//otherwise the targeting line just jumps around
		yield return new WaitForSeconds (0.05f);
		playerShootingScript.SetCanShoot (true);
	}
}
