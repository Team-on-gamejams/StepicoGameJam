using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUI : MonoBehaviour {
	[Header("UI")]
	[SerializeField] Image fillImage;
	[SerializeField] TextMeshProUGUI textField;

	float curr;
	float max;

	public void Init(float currHp, float maxHp) {
		curr = currHp;
		max = maxHp;

		UpdateVisuals();
	}

	public void UpdateCurr(float currHp) {
		curr = currHp;

		UpdateVisuals();
	}

	void UpdateVisuals() {
		if (textField)

			textField.text = $"<u>{curr:0}</u>\n{max:0}";

		LeanTween.value(gameObject, fillImage.fillAmount, Mathf.Lerp(0, 0.25f, curr / max), 0.1f)
			.setEase(LeanTweenType.easeInQuad)
			.setOnUpdate((float fill) => {
				fillImage.fillAmount = fill;
			});
	}
}
