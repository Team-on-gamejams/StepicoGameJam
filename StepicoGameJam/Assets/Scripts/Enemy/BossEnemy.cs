using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossEnemy : MonoBehaviour {
	[Header("UI")]
	[SerializeField] string bossNameTextKey = "Boss name text key";
	[SerializeField] TextMeshProUGUI bossNameTextField;

	void Awake() {
		bossNameTextField.text = Polyglot.Localization.Get(bossNameTextKey);
	}
}
