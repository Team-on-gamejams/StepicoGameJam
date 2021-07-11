using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.UI;


public class Player : MonoBehaviour {
	public float Radius => mover.Radius;

	[Header("Audio"), Space]
	[SerializeField] AudioClip onUpgradeClip;

	[Header("Refs"), Space]
	public PlayerMoving mover;
	[SerializeField] CameraFollowAnchor cameraFollowAnchor;
	[SerializeField] WeaponMouseFollow weaponMouseFollow;
	[SerializeField] ProjectileWeapon[] projectileWeapons;
	[SerializeField] MeleeWeapon[] meleeWeapons;
	[SerializeField] Health health;

	int selectedProjectileWeapon = 0;
	int selectedMeleeWeapon = 0;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!mover)
			mover = GetComponent<PlayerMoving>();
		if (!cameraFollowAnchor)
			cameraFollowAnchor = GetComponent<CameraFollowAnchor>();
		if (!weaponMouseFollow)
			weaponMouseFollow = GetComponent<WeaponMouseFollow>();
		if (!health)
			health = GetComponentInChildren<Health>();
	}
#endif

	private void Awake() {
		GameManager.Instance.player = this;
	}

	private void Start() {
		PickWeapons();
	}

	public void ApplyUpgrade(PlayerUpgradeEnum playerUpgradeEnum) {
		AudioManager.Instance.Play(onUpgradeClip);

		switch (playerUpgradeEnum) {
			case PlayerUpgradeEnum.MoreHPAndHeal:
				health.ReInitHealth(health.MaxHealth * 1.5f, health.MaxHealth * 1.5f);
				break;
			case PlayerUpgradeEnum.UnlockNewWeaponRange:
				break;
			case PlayerUpgradeEnum.UnlockNewWeaponMelee:
				break;
			case PlayerUpgradeEnum.MoreWeaponDamage:
				break;
			case PlayerUpgradeEnum.LessWeaponCDAndMoreBulletSpeed:
				break;
			case PlayerUpgradeEnum.MorePlayerSpeed:
				break;
			case PlayerUpgradeEnum.BetterDodge:
				break;
			default:
				Debug.Log("Wrong upgrade type");
				break;
		}
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

	public void OnAttackMelee(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Started:
				Debug.Log("Melee attack");
				break;
		}

		//meleeWeapons[selectedMeleeWeapon].AttackSingle();
	}

	public void OnAttackProjectile(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Started:
				if (selectedProjectileWeapon == 2)
					projectileWeapons[selectedProjectileWeapon].StartAttackSequence();
				else
					projectileWeapons[selectedProjectileWeapon].AttackSingle();
				break;
			case InputActionPhase.Canceled:
				if (selectedProjectileWeapon == 2)
					projectileWeapons[selectedProjectileWeapon].StopAttackSequence();
				break;
		}
	}

	public void OnDodge(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Started:
				mover.Dodge();
				break;
			case InputActionPhase.Canceled:
				mover.CancelDodge();
				break;
		}
	}

	public void SelectProjectileWeapon(int id) {
		selectedProjectileWeapon = id;
		PickWeapons();
	}

	public void SelectMeleeWeapon(int id) {
		selectedMeleeWeapon = id;
		PickWeapons();
	}

	public void ScrollMelee(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Started:
				if(context.ReadValue<float>() < 0) {
					selectedMeleeWeapon = (int)Mathf.Repeat(selectedMeleeWeapon - 1, meleeWeapons.Length);
				}
				else {
					selectedMeleeWeapon = (int)Mathf.Repeat(selectedMeleeWeapon + 1, meleeWeapons.Length);
				}

				PickWeapons();
				break;
		}
	}

	public void ScrollRange(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Started:
				if (context.ReadValue<float>() < 0) {
					selectedProjectileWeapon = (int)Mathf.Repeat(selectedProjectileWeapon - 1, projectileWeapons.Length);
				}
				else {
					selectedProjectileWeapon = (int)Mathf.Repeat(selectedProjectileWeapon + 1, projectileWeapons.Length);
				}

				PickWeapons();
				break;
		}
	}

	void PickWeapons() {
		for (int i = 0; i < projectileWeapons.Length; ++i) {
			if (i == selectedProjectileWeapon)
				projectileWeapons[i].Enable();
			else
				projectileWeapons[i].Disable();
		}

		//TODDO:
		//for (int i = 0; i < meleeWeapons.Length; ++i) {
		//	if (i == selectedMeleeWeapon)
		//		meleeWeapons[i].Enable();
		//	else
		//		meleeWeapons[i].Disable();
		//}
	}
}
