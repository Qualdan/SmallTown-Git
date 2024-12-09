using UnityEditor;
using UnityEngine;

namespace SmallTown {
public class PlayerManager : MonoBehaviour, IManager {

	// Variables
	[SerializeField] private PlayerController playerPrefab;
	[SerializeField] private PlayerCar playerCarPrefab;
	[SerializeField] private ShaderCrew.SeeThroughShader.PlayersPositionManager positionManager;
	private PlayerController _player;
	private PlayerCar _playerCar;

	// Public variables
	public bool Exists { get { return _player != null; } }
	public PlayerController Controller { get { return _player; } }
	public PlayerGraphics Graphics { get { return _player.Graphics; } }
	public PlayerActions Actions { get { return _player.Actions; } }
	public PlayerStates State { get { return _player.PlayerState; } }
	public Transform Form { get { return _player.Form; } }
	public Vector3 Position { get { return _player.Position; } }
	public Vector3 BulletPosition { get { return _player.Graphics.BulletPosition; } }
	public Vector3 EnemyTarget { get { return _player.EnemyTarget; } }
	public bool Moving { get { return _player.Moving; } }
	public bool Initialized { get; private set; }
	public Vector3 CameraTarget { get { return _player.Graphics.CameraTarget.position; } }


	// ************************************************************************************************************ UNITY FUNCTIONS

	public void Setup() {
		GameManager.Area.OnBeforeAreaChanged += Unload;

		if (GameManager.Config.Debug.EditorState == EditorStates.LevelTesting) {
			_player = GetPlayer();
			_playerCar = GetPlayerCar();
			positionManager.AddPlayerAtRuntime(_player.FadeTarget);
			if(_player == null || _playerCar == null) {
				Debug.LogError("Level Testing Failure: Config/Debug/EditorState is set to LevelTesting, which requires Player and Car prefabs to be present in the scene!");
			}
		}
		else {

			// Spawn players to logical place?

		}


		Initialized = true;
		Helpers.Initialized(GetType().Name);
	}

	void Unload() {
		positionManager.RemovePlayerAtRuntime(_player.FadeTarget);
		_player.Unload();
		_player = null;
	}


	public void FastTravel(Transform playerTarget, Transform carTarget) {

		// Player
		if(_player == null) {
			_player = GetPlayer();
			positionManager.AddPlayerAtRuntime(_player.FadeTarget);
		}
		_player.FastTravel(playerTarget);

		// Car
		if(_playerCar == null) {
			_playerCar = GetPlayerCar();
		}
		_playerCar.FastTravel(carTarget);
	}



	PlayerController GetPlayer() {
		PlayerController player = null;
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		if (go != null) {
			player = go.transform.parent.GetComponent<PlayerController>();
			Helpers.Log("PlayerManager", "PlayerController", "Found in scene", LogStates.Extended);
		}
		if(player == null) {
			player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
			Helpers.Log("PlayerManager", "PlayerController", "Spawned new", LogStates.Extended);
		}
		player?.Setup();
		return player;
	}

	PlayerCar GetPlayerCar() {
		PlayerCar playerCar = null;
		GameObject go = GameObject.FindGameObjectWithTag("PlayerCar");
		if (go != null) {
			playerCar = go.GetComponent<PlayerCar>();
		}
		if(playerCar == null) {
			playerCar = Instantiate(playerCarPrefab, Vector3.zero, Quaternion.identity);
		}
		playerCar?.Setup();
		return playerCar;
	}




	// PlayerController GetPlayer() {
	// 	GameObject go = GameObject.FindGameObjectWithTag("Player");
	// 	if (go != null) {
	// 		PlayerController player = go.transform.parent.GetComponent<PlayerController>();
	// 		player.Setup();
	// 		return player;
	// 	}
	// 	PlayerController newPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
	// 	if (newPlayer != null) {
	// 		newPlayer.Setup();
	// 		return newPlayer;
	// 	}
	// 	return null;
	// }



	// PlayerCar GetPlayerCar() {
	// 		GameObject go = GameObject.FindGameObjectWithTag("Player");
	// 		if (go != null) {
	// 			_player = go.transform.parent.GetComponent<PlayerController>();
	// 			_player.Setup();
	// 		}
	// 		else {
	// 			Debug.LogError("Level Testing Failure: Config/Debug/EditorState is set to LevelTesting, which requires Player prefab to be present in the scene!");
	// 		}

	// 		go = GameObject.FindGameObjectWithTag("PlayerCar");
	// 		if (go != null) {
	// 			_playerCar = go.GetComponent<PlayerCar>();
	// 		}
	// 		else {
	// 			Debug.LogError("Level Testing Failure: Config/Debug/EditorState is set to LevelTesting, which requires Player Car prefab to be present in the scene!");
	// 		}

	// 					PlayerCar newCar = Instantiate(playerCarPrefab, playerTarget.position, Quaternion.identity);
	// 		if (newCar != null) {
	// 			_playerCar = newCar;
	// 			_playerCar.Setup();
	// 		}

	// }

}
}