using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowAnchor : MonoBehaviour {
	[Header("Values"), Space]
	[SerializeField, Range(0f, 1f)] float keyboardCursorWeight = 0.7f;
	[Space]
	[SerializeField, Range(0f, 15f)] float gamepadcursorWeightMax= 0.5f;

	bool isUseGamepad;
	Vector2 mouseScreenPos;
	Vector2 gamepadDir;

	Transform playerMover;


	void Start() {
		playerMover = GameManager.Instance.player.mover.transform;
	}

	public void LateUpdate() {
		if (isUseGamepad) {
			Vector3 gamepadOffset = gamepadDir;
			Vector3 playerWorldPos = playerMover.position;

			transform.position = playerWorldPos + gamepadOffset * gamepadcursorWeightMax;
		}
		else {
			Vector3 cursorWorldPos = TemplateGameManager.Instance.Camera.ScreenToWorldPoint(mouseScreenPos).SetZ(0.0f);
			Vector3 playerWorldPos = playerMover.position;

			transform.position = playerWorldPos + (cursorWorldPos - playerWorldPos) * keyboardCursorWeight;
		}
	}

	public void StartLookMouse(Vector2 mouseScreenPos) {
		isUseGamepad = false;

		this.mouseScreenPos = mouseScreenPos;
	}

	public void LookMouse(Vector2 mouseScreenPos) {
		this.mouseScreenPos = mouseScreenPos;
	}

	public void EndLookMouse() {

	}

	public void StartLookGamepad(Vector2 gamepadPos) {
		isUseGamepad = true;

		this.gamepadDir = gamepadPos;
	}

	public void LookGamepad(Vector2 gamepadPos) {
		this.gamepadDir = gamepadPos;
	}

	public void EndLookGamepad() {
		this.gamepadDir = Vector2.zero;
	}
}
