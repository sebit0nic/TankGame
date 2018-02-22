using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockonShotCollision : MonoBehaviour {

	public int damage = 10;
	public float turnTime = 1f;
	public float selfDestructTimeout = 6f;
	[Range(0, 1)]
	public float turnDamper;

	private GameObject target;
	private Enemy targetEnemy;
	private Projectile thisProjectile;
	private Collider thisCollider;
	private bool turnedToTarget, targetHit;
	private float projectileSpeed;
	private LockonShotPlayer lockonShotPlayer;

	private void Awake() {
		thisProjectile = GetComponent<Projectile> ();
		thisCollider = GetComponent<Collider> ();
	}

	public void Init(Vector3 position, Quaternion rotation, GameObject target, float projectileSpeed, LockonShotPlayer lockonShotPlayer) {
		transform.position = position;
		transform.rotation = rotation;
		thisCollider.enabled = true;
		this.projectileSpeed = projectileSpeed;
		this.lockonShotPlayer = lockonShotPlayer;
		turnedToTarget = false;
		targetHit = false;

		this.target = target;
		targetEnemy = target.GetComponent<Enemy> ();
		StartCoroutine (WaitForTurnTime ());
		StartCoroutine (WaitForSelfDestructTimeout ());
	}

	private void Update() {
		if (!targetHit) {
			if (turnedToTarget) {
				Quaternion targetRotation = Quaternion.LookRotation (target.transform.position - transform.position);
				transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, turnDamper);
			}
			transform.Translate (0, 0, projectileSpeed * Time.deltaTime, Space.Self);
		}

		if (!targetEnemy.IsTargetable () && !targetHit) {
			DestroyMissile ();
		}
	}

	private IEnumerator WaitForTurnTime() {
		yield return new WaitForSeconds (turnTime);
		turnedToTarget = true;
	}

	//Safety measure, destroy missile after x seconds
	private IEnumerator WaitForSelfDestructTimeout() {
		yield return new WaitForSeconds (selfDestructTimeout);
		DestroyMissile ();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag.Equals ("Enemy")) {
			DestroyMissile ();

			other.GetComponent<Enemy> ().HitByProjectile (damage);
		}
	}

	private void DestroyMissile() {
		transform.rotation = Quaternion.identity;
		targetHit = true;
		thisProjectile.OnProjectileDestroy ();
		lockonShotPlayer.NotifyLockonShotDestroyed ();
	}
}
