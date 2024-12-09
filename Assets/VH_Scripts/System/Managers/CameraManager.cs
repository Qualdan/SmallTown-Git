using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.AI;

namespace SmallTown {
public class CameraManager : MonoBehaviour, IManager {

	// Variables
	[SerializeField] Camera mainCamera;
	[SerializeField] Camera seethroughCamera;
	[SerializeField] CinemachineVirtualCamera virtualCamera;
	[SerializeField] CinemachineVirtualCamera zoomedCamera;
	[SerializeField] CinemachineInputProvider inputProvider;
	private CinemachineOrbitalTransposer _virtualTransposer;
	private CinemachineTransposer _zoomedTransposer;
	private int _viewIndex = 0;
	private int _forcedView = 0;
	private bool _mapView = false;
	private bool _forceViewOnBuilding = false;

	// Public variables
	public Camera Main { get { return mainCamera; } }
	public Vector3 Position { get { return mainCamera.transform.position; } }
	public bool Initialized { get; private set; }


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {
		GameManager.Input.Actions.Player.CameraControl.performed += ctx => ToggleControl(ctx, true);

		// GameManager.Input.Actions.Player.CameraMovement.performed += ctx => MoveCamera(ctx);

		GameManager.Input.Actions.Camera.Control.canceled += ctx => ToggleControl(ctx, false);
		GameManager.Input.Actions.Camera.ChangeView.performed += ctx => ChangeView(ctx);
		GameManager.Input.Actions.Player.MapView.performed += ctx => MapView(true);
		GameManager.Input.Actions.Player.MapView.canceled += ctx => MapView(false);

		GameManager.Settings.OnSettingChanged += SettingsChanged;

		_virtualTransposer = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
		_zoomedTransposer = zoomedCamera.GetCinemachineComponent<CinemachineTransposer>();
		_zoomedTransposer.m_FollowOffset = GameManager.Config.Camera.MapView;

		ViewChanger();

		Initialized = true;
		Helpers.Initialized(GetType().Name);
	}


	// void MoveCamera(InputAction.CallbackContext ctx) {
	// 	Vector2 movement = ctx.ReadValue<Vector2>();


	// 	Debug.LogWarning("Movement is " + movement);

	// 	if(movement.x > 0.0f) {
	// 		virtualCamera.transform.localEulerAngles = new Vector3(virtualCamera.transform.localEulerAngles.x, virtualCamera.transform.localEulerAngles.y + 10.0f, virtualCamera.transform.localEulerAngles.z);
	// 	}
	// }


	void ToggleControl(InputAction.CallbackContext ctx, bool toggle) {
		if(!GameManager.CurrentState(GameStates.Player) && !GameManager.CurrentState(GameStates.Camera)) { return; }

		GameManager.Player.Actions.StopWalking();
		GameManager.GameState = toggle ? GameStates.Camera : GameStates.Player;
		inputProvider.enabled = toggle;
	}

	void SettingsChanged(string key, float value) {
		// Debug.LogWarning("Key is " + key + " and value is " + value);

		if(key == "Settings/CameraSensitivity") {
			_virtualTransposer.m_XAxis.m_MaxSpeed = 15.0f * value;
		}
		else if(key == "Settings/AutoCamera") {
			_forceViewOnBuilding = value == 1.0f ? true : false;
		}

		// if(key == "Options/Fov") {
		// 	ChangeFov(value);
		// }
		// else if(key == "Options/Bobbing") {
		// 	ChangeBobbing(value);
		// }
		// else if(key == "Options/CameraReset") {
		// 	ChangeCameraReset(value);
		// }
		// else if(key == "Options/SensitivityHorizontal") {
		// 	ChangeSensitivityHorizontal(value);
		// }
		// else if(key == "Options/SensitivityVertical") {
		// 	ChangeSensitivityVertical(value);
		// }
		// else if(key == "Options/InvertHorizontal") {
		// 	ChangeInvertHorizontal(value);
		// }
		// else if(key == "Options/InvertVertical") {
		// 	ChangeInvertVertical(value);
		// }
	}

	public void SetPlayer(Transform target) {
		virtualCamera.Follow = target;
		virtualCamera.LookAt = target;
		zoomedCamera.Follow = target;
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: VIEWS

	void MapView(bool toggle) {
		_mapView = toggle;
		// Vector3 target = _mapView ? GameManager.Config.Camera.MapView : GameManager.Config.Camera.ViewList[_viewIndex];
		zoomedCamera.Priority = toggle ? virtualCamera.Priority + 1 : virtualCamera.Priority -1;
		mainCamera.cullingMask = toggle ? GameManager.Config.Camera.MapviewMask : GameManager.Config.Camera.DefaultMask;
		seethroughCamera.cullingMask = toggle ? GameManager.Config.Camera.SeethroughMapviewMask : GameManager.Config.Camera.SeethroughDefaultMask;
		GameManager.Effects.ZoomEffects(_mapView);
	}

	void ChangeView(InputAction.CallbackContext ctx) {
		if(!GameManager.CurrentState(GameStates.Camera)) { return; }

		Vector2 direction = ctx.ReadValue<Vector2>();
		if(direction.y < 0.0f) {
			_viewIndex++;
		}
		else if(direction.y > 0.0f) {
			_viewIndex--;
		}
		_viewIndex = Mathf.Clamp(_viewIndex, 0, GameManager.Config.Camera.ViewList.Count - 1);
		ViewChanger();
	}

	void ViewChanger() {
		if(_mapView) { return; }

		DOTween.To(()=> _virtualTransposer.m_FollowOffset, x=> _virtualTransposer.m_FollowOffset = x, GameManager.Config.Camera.ViewList[_viewIndex], GameManager.Config.Camera.ViewSpeed);
	}

	public void ForceView(bool toggle) {
		if(!_forceViewOnBuilding) {
			return;
		}

		if (toggle) {
			_forcedView++;
		}
		else {
			_forcedView--;
		}
		_viewIndex = _forcedView > 0 ? 1 : 0;
		ViewChanger();
	}
}
}