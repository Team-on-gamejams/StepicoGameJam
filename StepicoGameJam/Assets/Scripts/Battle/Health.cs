using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
	[Header("Values")]
	[SerializeField] bool isPlayer = false;
	[SerializeField] float maxHealth = 100;

	[Header("UI")]
	[SerializeField] HealthBar healthBar;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!healthBar)
			healthBar = GetComponentInChildren<HealthBar>();
	}
#endif

	float currHealth;
	bool isDead = false;

	void Awake() {
		currHealth = maxHealth;
	}

	public void ChangeHp(float delta) {
		currHealth = Mathf.Clamp(currHealth + delta, 0, maxHealth);

		if(currHealth == 0) {
			Die();
		}
	}

	void Die() {
		if (isDead)
			return;
		isDead = true;
	}
}
