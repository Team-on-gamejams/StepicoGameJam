using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {
	public float MaxHealth => maxHealth;

	[Header("Values")]
	[SerializeField] bool isPlayer = false;
	[SerializeField] float maxHealth = 100;

	[Header("UI"), Space]
	[SerializeField] HealthBar healthBar;
	[SerializeField] PlayerHealthUI playerHealthUI;

	[Header("Refs"), Space]
	[SerializeField] GameObject parentToDestroy;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!healthBar)
			healthBar = GetComponentInChildren<HealthBar>();
		if (!playerHealthUI)
			playerHealthUI = GetComponentInChildren<PlayerHealthUI>();
	}
#endif

	float currHealth;
	bool isDead = false;

	void Awake() {
		ReInitHealth(maxHealth, maxHealth);
	}

	public void ReInitHealth(float currHp, float maxHp) {
		currHealth = currHp;
		maxHealth = maxHp;

		if (healthBar)
			healthBar.Init(currHealth, maxHealth, 50);
		if (playerHealthUI)
			playerHealthUI.Init(currHealth, maxHealth);
	}

	public void ChangeHp(float delta) {
		bool isNeedLastChance = isPlayer && 1 < currHealth && currHealth + delta <= 0;

		if (isNeedLastChance)
			currHealth = 1;
		else
			currHealth = Mathf.Clamp(currHealth + delta, 0, maxHealth);

		if(healthBar)
			healthBar.UpdateCurr(currHealth);
		if(playerHealthUI)
			playerHealthUI.UpdateCurr(currHealth);

		if (currHealth == 0) {
			Die();
		}
	}

	void Die() {
		if (isDead)
			return;
		isDead = true;

		if (isPlayer) {
			Destroy(parentToDestroy);

			LeanTween.delayedCall(1.0f, () => {
				SceneLoader.Instance.LoadScene(TemplateMainMenu.sceneIdToLoad, true, true);
			});
		}
		else {
			Destroy(parentToDestroy);
		}
	}
}
