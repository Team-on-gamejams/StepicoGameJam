using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.UI;


public class Player : MonoBehaviour {
	public float Radius => mover.Radius;

	[Header("Refs"), Space]
	public PlayerMoving mover;
	[SerializeField] CameraFollowAnchor cameraFollowAnchor;
	[SerializeField] WeaponMouseFollow weaponMouseFollow;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!mover)
			mover = GetComponent<PlayerMoving>();
		if (!cameraFollowAnchor)
			cameraFollowAnchor = GetComponent<CameraFollowAnchor>();
		if (!weaponMouseFollow)
			weaponMouseFollow = GetComponent<WeaponMouseFollow>();
	}
#endif

	private void Awake() {
		GameManager.Instance.player = this;
	}

	public void OnMove(InputAction.CallbackContext context) {
		Vector2 newMove = context.ReadValue<Vector2>();

		switch (context.phase) {
			case InputActionPhase.Started:
				mover.StartMove(newMove);
				weaponMouseFollow.StartLookFromMoving(newMove);
				break;
			case InputActionPhase.Performed:
				mover.Move(newMove);
				weaponMouseFollow.LookFromMoving(newMove);
				break;
			case InputActionPhase.Canceled:
				mover.EndMove();
				weaponMouseFollow.EndLookFromMoving();
				break;
		}
	}

	public void OnLookMouse(InputAction.CallbackContext context) {
		Vector2 newLook = context.ReadValue<Vector2>();

		switch (context.phase) {
			case InputActionPhase.Started:
				cameraFollowAnchor.StartLookMouse(newLook);
				weaponMouseFollow.StartLookMouse(newLook);
				break;
			case InputActionPhase.Performed:
				cameraFollowAnchor.LookMouse(newLook);
				weaponMouseFollow.LookMouse(newLook);
				break;
			case InputActionPhase.Canceled:
				cameraFollowAnchor.EndLookMouse();
				weaponMouseFollow.EndLookMouse();
				break;
		}
	}

	public void OnLookGamepad(InputAction.CallbackContext context) {
		Vector2 newLook = context.ReadValue<Vector2>();

		switch (context.phase) {
			case InputActionPhase.Started:
				cameraFollowAnchor.StartLookGamepad(newLook);
				weaponMouseFollow.StartLookGamepad(newLook);
				break;
			case InputActionPhase.Performed:
				cameraFollowAnchor.LookGamepad(newLook);
				weaponMouseFollow.LookGamepad(newLook);
				break;
			case InputActionPhase.Canceled:
				cameraFollowAnchor.EndLookGamepad();
				weaponMouseFollow.EndLookGamepad();
				break;
		}
	}

	public void OnFire(InputAction.CallbackContext context) {
		bool isFiring = context.ReadValueAsButton();

		Debug.Log("Shoot");
	}
}
