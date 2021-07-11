using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polyglot;

public class RoomManager : MonoBehaviour {
	Room CurrRoom => roomsSequence[currRoomId];

	[Header("Sequence"), Space]
	[SerializeField] List<Room> roomsSequence = new List<Room>();
	[SerializeField] string winMessageKey = "WIN_MESSAGE";

	[Header("Audio"), Space]
	[SerializeField] AudioClip gameClip;
	AudioSource gameas;


	int currRoomId = 0;
	int aliveEnemies;

	void Start() {
		OnStartNewRoom();

		gameas = AudioManager.Instance.PlayLoop(gameClip);
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

			AudioManager.Instance.ChangeASVolume(gameas, 0.25f, 0.5f);

			LeanTween.delayedCall(gameObject, 2.0f, () => {
				GameManager.Instance.player.ApplyUpgrade(PlayerUpgradeEnum.MoreHPAndHeal);
			});

			LeanTween.delayedCall(gameObject, 3.0f, () => {
				OnUpgradeSelected();
			});
		}
	}

	public void OnUpgradeSelected() {
		if(currRoomId != roomsSequence.Count) {
			AudioManager.Instance.ChangeASVolume(gameas, 1.0f, 1.0f);

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
