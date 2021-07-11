using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMouseFollow : MonoBehaviour {
	[Header("Values")]
	[SerializeField] float maxAngularSpeed = 180.0f;

	bool isUseGamepad;
	Vector2 mouseScreenPos;
	Vector2 gamepadDir;
	Vector2 movingDir;

	Transform playerMover;
	float playerRadius;


	void Start() {
		playerMover = GameManager.Instance.player.mover.transform;
		playerRadius = 0.23f;
	}

	public void Update() {
		Vector3 dir;

		if (isUseGamepad) {
			dir = gamepadDir == Vector2.zero ? movingDir.normalized : gamepadDir.normalized;
		}
		else {
			Vector3 cursorWorldPos = TemplateGameManager.Instance.Camera.ScreenToWorldPoint(mouseScreenPos).SetZ(0.0f);
			Vector3 playerWorldPos = playerMover.position;

			dir = (cursorWorldPos - playerWorldPos).normalized;
		}

		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		if (angle < 0)
			angle = 360 + angle;
		float maxDelta = maxAngularSpeed * Time.deltaTime;
		float currDelta = Mathf.Clamp(angle - transform.eulerAngles.z, -maxDelta, maxDelta);

		if(Mathf.Abs(angle - transform.eulerAngles.z) > 180) {
			currDelta = -currDelta;
		}

		transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + currDelta);
		//transform.eulerAngles = new Vector3(0, 0, angle);

		transform.localPosition = transform.right.normalized * playerRadius;
	}

	public void StartLookMouse(Vector2 mouseScreenPos) {
		isUseGamepad = false;

		this.mouseScreenPos = mouseScreenPos;
	}

	public void LookMouse(Vector2 mouseScreenPos) {
		this.mouseScreenPos = mouseScreenPos;
	}

	public void EndLookMouse() {
		isUseGamepad = true;
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

	public void StartLookFromMoving(Vector2 movingDir) {
		this.movingDir = movingDir;
	}

	public void LookFromMoving(Vector2 movingDir) {
		this.movingDir = movingDir;
	}

	public void EndLookFromMoving() {

	}
}
