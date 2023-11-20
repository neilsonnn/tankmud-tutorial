using IWorld.ContractDefinition;
using mud;
using mudworld;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public Button spawn;

	void Start() {

		UpdateSpawn();

		NetworkManager.OnInitialized += UpdateSpawn;
		PlayerComponent.OnPlayerSpawned += SetupLocalPlayer;
	}

	void SetupLocalPlayer() {
		PlayerComponent.LocalPlayer.OnUpdated += UpdateSpawn;
	}

	void UpdateSpawn() {
		var currentPlayer = MUDTable.GetRecord<PlayerTable>(NetworkManager.LocalKey);
		spawn.gameObject.SetActive(currentPlayer == null);
	}

	public async void SpawnPlayer() {
		await TxManager.SendDirect<SpawnFunction>(0, 0);
	}

}
