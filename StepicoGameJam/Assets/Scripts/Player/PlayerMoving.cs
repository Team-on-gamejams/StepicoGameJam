using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour {
	public float Radius => circleCollider.radius;

	[Header("Values"), Space]
	public float startSpeed = 4.0f;
	public float maxSpeed = 6.0f;
	public float timeToMaxSpeed = 1.0f;

	[Header("Dodge"), Space]
	[SerializeField] WeaponUISlot dodgeUI;
	public float dodgeForce = 10f;
	public float doodgeTime = 0.2f;
	public float dodgeCooldown = 1.0f;

	[Header("Audio"), Space]
	[SerializeField] AudioClip dodgeClip;

	[Header("Refs"), Space]
	[SerializeField] Rigidbody2D rb;
	[SerializeField] CircleCollider2D circleCollider;
	[SerializeField] Collider2D hitbox;
	[SerializeField] ParticleSystem dodgePs;

	Vector2 moveVector;
	float moveTime;

	bool isMoving = false;

	bool isNeedDodge;
	bool isDodging;
	Vector2 dodgeVector;
	float currDodgeTime;
	float currDodgeCooldown;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!rb)
			rb = GetComponent<Rigidbody2D>();
		if (!circleCollider)
			circleCollider = GetComponent<CircleCollider2D>();
	}
#endif

	private void Awake() {
		currDodgeCooldown = dodgeCooldown;
	}

	private void Update() {
		if (isMoving)
			moveTime += Time.deltaTime;

		if (isDodging)
			dodgeUI.UpdateCooldownVisual(1.0f);
		else
			dodgeUI.UpdateCooldownVisual(1.0f - Mathf.Clamp01(currDodgeCooldown / dodgeCooldown));

		if (isNeedDodge) {
			if (currDodgeCooldown >= dodgeCooldown && moveVector != Vector2.zero) {
				currDodgeCooldown = 0;

				dodgeVector = moveVector;

				hitbox.enabled = false;

				AudioManager.Instance.Play(dodgeClip);
				dodgePs.Play();

				isDodging = true;
				isNeedDodge = false;
			}
		}

		if (isDodging) {
			currDodgeTime += Time.deltaTime;

			if(currDodgeTime >= doodgeTime) {
				currDodgeTime = 0;
				isDodging = false;
				dodgeVector = Vector2.zero;
				hitbox.enabled = true;
				dodgePs.Stop();
				rb.velocity = Vector2.zero;
				moveTime = 0;
			}
		}
		else {
			currDodgeCooldown += Time.deltaTime;
		}
	}

	public void FixedUpdate() {
		if (isDodging) {
			rb.velocity = dodgeVector * dodgeForce;
		}
		else if (isMoving) {
			rb.velocity = moveVector * Mathf.Lerp(startSpeed, maxSpeed, moveTime / timeToMaxSpeed);
		}
	}

	public void StartMove(Vector2 move) {
		isMoving = true;
		
		moveTime = 0;
		moveVector = move.normalized;
	}

	public void Move(Vector2 move) {
		moveVector = move.normalized;
	}

	public void EndMove() {
		moveVector = Vector2.zero;
		if(rb)
			rb.velocity = Vector2.zero;

		isMoving = false;
	}

	public void Dodge() {
		isNeedDodge = true;
	}

	public void CancelDodge() {
		isNeedDodge = false;
	}
}
