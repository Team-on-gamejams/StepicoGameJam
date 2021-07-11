using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarPart : MonoBehaviour {
	[Header("UI")]
	[SerializeField] Slider firstBar;
	[SerializeField] Slider secondBar;

	float currValue;
	float maxValue;

	public void Init(float curr, float max) {
		currValue = curr;
		maxValue = max;

		firstBar.minValue = secondBar.minValue = 0;
		firstBar.maxValue = secondBar.maxValue = max;

		UpdateVisuals(true, currValue);
	}

	public void ChangeValues(float curr) {
		UpdateVisuals(false, curr);
	}

	void UpdateVisuals(bool isForce, float newValue) {
		if (isForce) {
			firstBar.value = secondBar.value = currValue;
		}
		else {
			if (newValue != currValue || newValue != currValue) {
				currValue = newValue;
				
				LeanTween.cancel(gameObject);

				LeanTween.value(firstBar.value, currValue, 0.2f)
				.setEase(LeanTweenType.linear)
				.setOnUpdate((float val) => {
					firstBar.value = val;
				});

				LeanTween.value(secondBar.value, currValue, 0.7f)
				.setEase(LeanTweenType.easeInOutQuart)
				.setOnUpdate((float val) => {
					secondBar.value = val;
				});
			}
		}
	}
}
