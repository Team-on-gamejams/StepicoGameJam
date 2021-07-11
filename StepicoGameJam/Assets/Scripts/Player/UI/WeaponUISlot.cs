using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUISlot : MonoBehaviour {
	[Header("Refs"), Space]
	[SerializeField] Image cooldownImage;
	[SerializeField] Image selectionImage;

	public void Enable() {
		selectionImage.enabled = true;
	}

	public void Disable() {
		selectionImage.enabled = false;
	}

	public void UpdateCooldownVisual(float t) {
		cooldownImage.fillAmount = t;
	}
}
