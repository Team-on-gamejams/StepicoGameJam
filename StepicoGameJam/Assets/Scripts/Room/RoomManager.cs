using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polyglot;

public class RoomManager : MonoBehaviour {
	Room CurrRoom => roomsSequence[currRoomId];

	[Header("Sequence"), Space]
	[SerializeField] List<Room> roomsSequence = new List<Room>();
	[SerializeField] string winMessageKey = "WIN_MESSAGE";

	int currRoomId = 0;
	int aliveEnemies;

	void Start() {
		OnStartNewRoom();
	}

	public void OnStartNewRoom() {
		CurrRoom.gameObject.SetActive(true);

		GameManager.Instance.player.mover.transform.position = CurrRoom.spawnPoint.position;

		aliveEnemies = CurrRoom.enemies.Length;
		foreach (var enemy in CurrRoom.enemies) 
			enemy.onDie += OnEnemyDie;
	}

	public void OnKillAllEnemies() {
		if (currRoomId + 1 == roomsSequence.Count) {
			Debug.Log($"{Localization.Get(winMessageKey)}");
		}
		else {
			++currRoomId;
			
			Debug.Log($"{string.Format(Localization.Get(CurrRoom.upgradeStringKey), CurrRoom.upgradeValue)}");

			LeanTween.delayedCall(gameObject, 1.0f, () => {
				OnUpgradeSelected();
			});
		}
	}

	public void OnUpgradeSelected() {
		if(currRoomId != roomsSequence.Count) {
			LeanTween.delayedCall(gameObject, 1.0f, () => {
				CurrRoom.gameObject.SetActive(false);
				OnStartNewRoom();
			});
		}
	}

	void OnEnemyDie(Enemy enemy) {
		if(enemy)
			enemy.onDie -= OnEnemyDie;
		--aliveEnemies;

		if(aliveEnemies == 0) {
			OnKillAllEnemies();
		}
	}
}
