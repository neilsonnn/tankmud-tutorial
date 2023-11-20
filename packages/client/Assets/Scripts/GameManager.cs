using IWorld.ContractDefinition;
using mud;
using mudworld;
using UnityEngine;

public class GameManager : MonoBehaviour {

	void Start() {
		NetworkManager.OnInitialized += Spawn;
	}

	async void Spawn() {

		var currentPlayer = MUDTable.GetRecord<PlayerTable>(NetworkManager.LocalKey);

		if (currentPlayer == null) {
			await TxManager.SendDirect<SpawnFunction>(0, 0);
		}

	}

}
