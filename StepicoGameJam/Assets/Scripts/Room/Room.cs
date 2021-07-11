using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
	[Header("Upgrade"), Space]
	public string upgradeStringKey = "ROOM_UPGRADE_";
	public float upgradeValue = 50;
	public Transform spawnPoint;
	public Transform[] upgareSpawnPoints;

	public Enemy[] enemies;

#if UNITY_EDITOR
	private void OnValidate() {
		enemies = GetComponentsInChildren<Enemy>();
	}
#endif
}
