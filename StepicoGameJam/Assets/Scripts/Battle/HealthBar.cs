using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour {
	[Header("UI")]
	[SerializeField] GameObject partPrefab;
	[SerializeField] TextMeshProUGUI textField;

	List<HealthBarPart> parts = new List<HealthBarPart>(4);

	float curr;
	float max;
	float perPart;
	
	public void Init(float currHp, float maxHp, float hpPerPart) {
		curr = currHp;
		max = maxHp;
		perPart = hpPerPart;

		int neededParts = Mathf.CeilToInt(max / perPart);
		float stackedHp = curr;

		for(int i = 0; i < neededParts; ++i) {
			HealthBarPart newPart = Instantiate(partPrefab, transform).GetComponent<HealthBarPart>();

			newPart.Init(stackedHp > perPart ? perPart : stackedHp, perPart);

			stackedHp = Mathf.Clamp(stackedHp - perPart, 0, max);

			parts.Add(newPart);
		}

		UpdateVisuals();
	}

	public void UpdateCurr(float currHp) {
		curr = currHp;

		UpdateVisuals();
	}

	void UpdateVisuals() {
		if (textField)
			textField.text = $"{curr} / {max}";

		float stackedHp = curr;
		for (int i = 0; i < parts.Count; ++i) {
			parts[i].ChangeValues(stackedHp > perPart ? perPart : stackedHp);

			stackedHp = Mathf.Clamp(stackedHp - perPart, 0, max);
		}
	}
}
