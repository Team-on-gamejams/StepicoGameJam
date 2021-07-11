using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeAltar : MonoBehaviour {
	public Action onUpgradeSelected;

	public PlayerUpgradeEnum type;
	static bool isApplied = false;

	private void Awake() {
		isApplied = false;
	}

	public void OnSelect() {
		GameManager.Instance.player.ApplyUpgrade(type);
		onUpgradeSelected?.Invoke();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (isApplied)
			return;

		if(collision.gameObject.layer == UnityConstants.Layers.Player) {
			isApplied = true;
			OnSelect();
		}
	}
}
