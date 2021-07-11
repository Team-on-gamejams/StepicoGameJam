using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	public Action<Enemy> onDie;

	[Header("Refs"), Space]
	[SerializeField] ProjectileWeapon projectileWeapon;
	[SerializeField] Transform movingPart;
	[SerializeField] Health health;
	[SerializeField] float radius = 0.5f;
	[SerializeField] float maxAngularSpeed = 180.0f;

	Transform target;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!projectileWeapon)
			projectileWeapon = GetComponentInChildren<ProjectileWeapon>();
		if (!health)
			health = GetComponentInChildren<Health>();
	}
#endif

	void Start() {
		projectileWeapon.StartAttackSequence();

		target = GameManager.Instance.player.mover.transform;

		health.onDie += OnDie;
	}

	void OnDestroy() {
		health.onDie -= OnDie;
	}

	void OnDie() {
		onDie?.Invoke(this);
	}

	private void Update() {
		if (!target) {
			projectileWeapon.StopAttackSequence();
			return;
		}

		Vector3 dir = target.position - movingPart.position;

		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		if (angle < 0)
			angle = 360 + angle;
		float maxDelta = maxAngularSpeed * Time.deltaTime;
		float currDelta = Mathf.Clamp(angle - projectileWeapon.transform.eulerAngles.z, -maxDelta, maxDelta);

		if (Mathf.Abs(angle - projectileWeapon.transform.eulerAngles.z) > 180)
			currDelta = -currDelta;

		projectileWeapon.transform.eulerAngles = new Vector3(0, 0, projectileWeapon.transform.eulerAngles.z + currDelta);

		projectileWeapon.transform.localPosition = projectileWeapon.transform.right.normalized * radius;
	}

	
}
