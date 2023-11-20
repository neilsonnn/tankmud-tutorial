using System;
using System.Collections.Generic;
using IWorld.ContractDefinition;
using mud;
using UniRx;
using UnityEngine;
using ObservableExtensions = UniRx.ObservableExtensions;

public class GameManager : MonoBehaviour
{
	public GameObject playerPrefab;


	void Start() {
		NetworkManager.OnInitialized += Spawn;
	}

	void Spawn() {
		var addressKey = NetworkManager.LocalKey;
		// var playerTable = new TableId("", "Player");

		// var currentPlayer = NetworkManager.Datastore.GetValue(playerTable, addressKey);
		// if (currentPlayer == null) {
		// 	await nm.worldSend.TxExecute<SpawnFunction>(0, 0);
		// }

	}

}
