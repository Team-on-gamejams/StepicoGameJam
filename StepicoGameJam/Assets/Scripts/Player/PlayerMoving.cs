using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour {
	public float Radius => circleCollider.radius;

	[Header("Values"), Space]
	[SerializeField] float startSpeed = 4.0f;
	[SerializeField] float maxSpeed = 6.0f;
	[SerializeField] float timeToMaxSpeed = 1.0f;

	[Header("Refs"), Space]
	[SerializeField] Rigidbody2D rb;
	[SerializeField] CircleCollider2D circleCollider;

	Vector2 moveVector;
	float moveTime;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!rb)
			rb = GetComponent<Rigidbody2D>();
		if (!circleCollider)
			circleCollider = GetComponent<CircleCollider2D>();
	}
#endif

	void Start() {
		enabled = false;
	}

	public void FixedUpdate() {
		moveTime += Time.deltaTime;
		rb.velocity = moveVector * Mathf.Lerp(startSpeed, maxSpeed, moveTime / timeToMaxSpeed);
	}

	public void StartMove(Vector2 move) {
		enabled = true;
		
		moveTime = 0;
		moveVector = move.normalized;
	}

	public void Move(Vector2 move) {
		moveVector = move.normalized;
	}

	public void EndMove() {
		moveVector = Vector2.zero;
		rb.velocity = Vector2.zero;

		enabled = false;
	}
}
