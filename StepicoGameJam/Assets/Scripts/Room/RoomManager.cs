using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polyglot;

public class RoomManager : MonoBehaviour {
	Room CurrRoom => roomsSequence[currRoomId];

	[Header("Sequence"), Space]
	[SerializeField] List<Room> roomsSequence = new List<Room>();
	[SerializeField] string winMessageKey = "WIN_MESSAGE";

	[Header("Upgrader"), Space]
	[SerializeField] List<GameObject> upgradePrefabs = new List<GameObject>();
	List<GameObject> upgradePrefabsPool = new List<GameObject>();


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
			AudioManager.Instance.ChangeASVolume(gameas, 0.25f, 0.5f);

			SpawnNewAltar(0, upgradePrefabs[0]);
			for (int i = 1; i < 4; ++i) {
				if(upgradePrefabsPool.Count == 0) {
					for (int j = 1; j < upgradePrefabs.Count; ++j) {
						upgradePrefabsPool.Add(upgradePrefabs[j]);
					}
				}

				int rand = Random.Range(0, upgradePrefabsPool.Count);
				SpawnNewAltar(i, upgradePrefabsPool[rand]);
				upgradePrefabsPool.RemoveAt(rand);
			}

			++currRoomId;
			//TODO:
			Debug.Log($"{string.Format(Localization.Get(CurrRoom.upgradeStringKey), CurrRoom.upgradeValue)}");
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

	void SpawnNewAltar(int id, GameObject prefab) {
		GameObject instantitatedAltar = Instantiate(prefab, CurrRoom.upgareSpawnPoints[id].position, Quaternion.identity, CurrRoom.transform);
		UpgradeAltar altar = instantitatedAltar.GetComponent<UpgradeAltar>();
		altar.onUpgradeSelected += OnUpgradeSelected;
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
