using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTown {
public class InputManager : MonoBehaviour, IManager  {

	// Variables
	private InputActions _inputActions;

	// Public Variables
	public InputActions Actions { get { return _inputActions; } }
	public bool Initialized { get; private set; }


	// ************************************************************************************************************ UNITY FUNCTIONS


	// **** TEMPORARY ****

	void Update() {
		if(Input.GetKeyDown(KeyCode.V)) {
			if (Cursor.lockState == CursorLockMode.Confined) {
				Cursor.lockState = CursorLockMode.None;
			}
			else {
				Cursor.lockState = CursorLockMode.Confined;
			}
		}

	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {
		_inputActions = new InputActions();

		_inputActions.Player.Pause.performed += ctx => GameManager.Instance.TogglePause(true);

		GameManager.OnGameStateChanged += OnGameStateChanged;

		// ToggleMenu(false);
		// TogglePlayer(true);

		Cursor.lockState = CursorLockMode.Confined;

		Initialized = true;
		Helpers.Initialized(GetType().Name);
	}



	// void OnGameStateChanged(GameStates gameState) {
	// 	Cursor.visible = gameState != GameStates.game;
	// 	if(gameState == GameStates.menu) {
	// 		Cursor.lockState = CursorLockMode.Confined;
	// 	}
	// 	else if(gameState == GameStates.pause) {
	// 		Debug.LogWarning("Cursor is confined");
	// 		Cursor.lockState = CursorLockMode.Confined;
	// 	}
	// 	else if(gameState == GameStates.game) {
	// 		Debug.LogWarning("Cursor should lock");
	// 		Cursor.lockState = CursorLockMode.Locked;
	// 	}
	// }



	// ************************************************************************************************************ CUSTOM FUNCTIONS: GAME STATE

	void OnGameStateChanged(GameStates value) {
		ToggleMenu(value == GameStates.Menu || value == GameStates.Pause);
		ToggleJournal(value == GameStates.Journal);
		TogglePlayer(value == GameStates.Player);
		ToggleCamera(value == GameStates.Camera);
	}

	void ToggleMenu(bool toggle) {
		if (toggle) {
			_inputActions.Menu.Enable();
			Helpers.Log(GetType().Name, "Enabled InputActions", "Menu", LogStates.Extended);
		}
		else {
			_inputActions.Menu.Disable();
			Helpers.Log(GetType().Name, "Disabled InputActions", "Menu", LogStates.Everything);
		}
	}

	void ToggleJournal(bool toggle) {
		if (toggle) {
			_inputActions.Journal.Enable();
			Helpers.Log(GetType().Name, "Enabled InputActions", "Journal", LogStates.Extended);
		}
		else {
			_inputActions.Journal.Disable();
			Helpers.Log(GetType().Name, "Disabled InputActions", "Journal", LogStates.Everything);
		}
	}

	void TogglePlayer(bool toggle) {
		if (toggle) {
			_inputActions.Player.Enable();
			Helpers.Log(GetType().Name, "Enabled InputActions", "Player", LogStates.Extended);
		}
		else {
			_inputActions.Player.Disable();
			Helpers.Log(GetType().Name, "Disabled InputActions", "Player", LogStates.Everything);
		}
	}

	void ToggleCamera(bool toggle) {
		Cursor.visible = !toggle;
		if (toggle) {
			_inputActions.Camera.Enable();
			Helpers.Log(GetType().Name, "Enabled InputActions", "Camera", LogStates.Extended);
		}
		else {
			_inputActions.Camera.Disable();
			Helpers.Log(GetType().Name, "Disabled InputActions", "Camera", LogStates.Everything);
		}
	}
}
}