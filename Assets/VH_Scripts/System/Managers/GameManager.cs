using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
// using DotLiquid.Util;

namespace SmallTown {

	// General
	public enum GameStates { None, Menu, Pause, Player, Camera, Journal, Death, Loading }
	public enum MenuStates { None, Play, Settings, Credits, Tutorial, Quit }
	public enum LogStates { None, Normal, Extended, Everything }
	public enum EditorStates { GameTesting, LevelTesting }
	public enum OptionTypes { Toggle, Slider, Dropdown, Input }
	public enum Difficulties { Easy, Medium, Hard }

	// Characters
	public enum PlayerStates { Idle, Sneaking, Walking, Running }
	public enum EnemyFactions { None, Town, Cultist, Scientist, Hermit, Police }

	// Items
	public enum WeaponTypes { Melee, Pistol, Shotgun, Rifle }
	public enum DoorStates { Open, Closed, Locked }

public class GameManager : MonoBehaviour {

	private static GameManager instance;
	public static bool HasInstance { get { return instance != null; } }

	[Header("Systems")]
	[SerializeField] private AssetManager assetManager;
	[SerializeField] private InputManager inputManager;
	[SerializeField] private PlayerManager playerManager;
	[SerializeField] private EnemyManager enemyManager;
	[SerializeField] private CameraManager cameraManager;
	[SerializeField] private GuiManager guiManager;
	[SerializeField] private MediaManager mediaManager;
	[SerializeField] private EffectsManager effectsManager;
	[SerializeField] private SaveManager saveManager;
	[SerializeField] private AreaManager areaManager;
	private static bool _initialized { get; set; }

	// Public objects
	public static GameManager Instance { get { return instance; } }
	public static AssetManager Asset { get { return instance.assetManager; } }
	public static InputManager Input { get { return instance.inputManager; } }
	public static PlayerManager Player { get { return instance.playerManager; } }
	public static EnemyManager Enemy { get { return instance.enemyManager; } }
	public static CameraManager Camera { get { return instance.cameraManager; } }
	public static GuiManager Gui { get { return instance.guiManager; } }
	public static MediaManager Media { get { return instance.mediaManager; } }
	public static EffectsManager Effects { get { return instance.effectsManager; } }
	public static SaveManager Save { get { return instance.saveManager; } }
	public static SettingsManager Settings { get { return instance.guiManager.Settings; } }
	public static AreaManager Area { get { return instance.areaManager; } }

	// Public variables
	public static bool Initialized { get {  return _initialized; } }
	public static bool Hostile { get {  return _initialized; } }

	[Header("Data")]
	[SerializeField] private ConfigData _config;
	[SerializeField] private CreditsData _credits;
	[SerializeField] private List<DifficultyData> _difficulties = new List<DifficultyData>();

	// Public data
	public static ConfigData Config { get { return instance._config; } }
	public static CreditsData Credits { get { return instance._credits; } }
	public static DifficultyData Difficulty { get { return instance._difficulties[Settings.CurrentDifficulty]; } }

	// Game state
	private static GameStates _gameState = GameStates.None;
	public static GameStates GameState {
		get {
			return _gameState;
		}
		set {
			if (_gameState != value)  {
				_gameState = value;
				OnGameStateChanged?.Invoke(value);
				Helpers.Log("GameManager", "Game state was changed", value.ToString());
			}
		}
	}
	public static Action<GameStates> OnGameStateChanged;
	public static bool CurrentState(GameStates state) { return GameState == state; }

	// Pause
	private static bool pause = false;
	public static bool Pause {
		get {
			return pause;
		}
		private set {
			if (pause != value) {
				pause = value;
				if (value) {
					DOTween.PauseAll();
				}
				else {
					DOTween.PlayAll();
				}
				OnPauseChanged?.Invoke(value);
			}
		}
	}
	public static Action<bool> OnPauseChanged;
	public static Action<EnemyController> OnPlayerDied;
	public static Action OnLocaleChanged;


	public static List<string> PlayerTraits = new List<string>();



	// ************************************************************************************************************ UNITY FUNCTIONS

	// ON AWAKE
	void Awake() {
		StartCoroutine(Setup());
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	// GAME SETUP
	IEnumerator Setup() {
		if(!Initialized) {
		#if UNITY_EDITOR
			Debug.Log("Smart scroll activated");
		#endif
			DontDestroyOnLoad(gameObject);
			instance = this;

			// PlayerTraits.Add("Prescience");

			// Init tween
			DOTween.Init();
			DOTween.SetTweensCapacity(1500, 50);

			// Setup managers
			// _assetManager.Setup();
			// while (!_assetManager.Initialized) yield return null;
			yield return Localize.Setup();
			inputManager.Setup();
			guiManager.Setup();

			cameraManager.Setup();

			playerManager.Setup();
			enemyManager.Setup();

			effectsManager.Setup();
			areaManager.Setup();

			saveManager.Setup();

			GameState = Config.General.StartupState;
			_initialized = true;
			Helpers.Initialized(GetType().Name);
		}
		else {
			Debug.Log("Removing this");
			Destroy(gameObject);
		}
		yield return null;
	}

	public void TogglePause(bool isPaused) {
		Pause = isPaused;
		GameState = isPaused ? GameStates.Pause : GameStates.Player;
	}
}
}