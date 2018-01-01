using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialAbility : MonoBehaviour {

	public enum AbilityType {DODGE_JUMP, BULLET_STOP, SHIELD};
	public AbilityType currentAbilityType;
	public ParticleSystem jumpParticles;
	public bool alwaysAbilityAvailable;

	[Header("Dodge Jump Variables")]
	public float upwardsThrust = 125000f;
	public float sidewaysThrust = 50000f;
	public float downwardsThrust = -90000f;
	public float airTime = 0.5f;
	public float fallTime = 1.2f;

	private Rigidbody thisRigidbody;
	private Animator thisAnimator;
	private PlayerMovement playerMovement;
	private PlayerHealth playerHealth;
	private PlayerShootingScript playerShootingScript;
	private GameManager gameManager;

	private bool usesAbility = false;

	private void Start() {
		thisRigidbody = GetComponent<Rigidbody> ();
		thisAnimator = GetComponentInParent<Animator> ();
		playerMovement = GetComponent<PlayerMovement> ();
		playerHealth = GetComponent<PlayerHealth> ();
		playerShootingScript = GetComponent<PlayerShootingScript> ();
		gameManager = GameObject.Find ("Game Manager").GetComponent<GameManager> ();
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
		//TODO: Implement BulletStop
	}

	private void DoShield() {
		//TODO: Implement Shield
	}

	private IEnumerator DodgeJumpCoroutine() {
		playerMovement.SetUsesAbility (true);
		playerHealth.SetInvincible (true);
		playerShootingScript.SetCanShoot (false);
		gameManager.NotifyPlayerStateChanged ('A');
		thisRigidbody.AddForce (new Vector3(Input.GetAxis("Horizontal") * sidewaysThrust, upwardsThrust, Input.GetAxis("Vertical") * sidewaysThrust), ForceMode.Impulse);

		yield return new WaitForSeconds (airTime);

		thisRigidbody.AddForce (new Vector3(0, downwardsThrust, 0), ForceMode.Impulse);

		yield return new WaitForSeconds (fallTime);

		playerHealth.SetInvincible (false);
		playerMovement.SetUsesAbility (false);
		usesAbility = false;
		gameManager.NotifyPlayerStateChanged ('N');

		//Wait for a little bit more before enabling shooting script
		//otherwise the targeting line just jumps around
		yield return new WaitForSeconds (0.05f);
		playerShootingScript.SetCanShoot (true);
	}
}
