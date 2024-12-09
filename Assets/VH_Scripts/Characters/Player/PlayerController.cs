using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using DG.Tweening;


namespace SmallTown {
public class PlayerController : MonoBehaviour, ICharacter {

	// Stats
	// public CharacterStats Stats;

	[Header("Components")]
	[SerializeField] private Transform form;
	[SerializeField] private Rigidbody body;
	[SerializeField] private NavMeshAgent agent;
	[SerializeField] private CapsuleCollider capsuleCollider;

	[Header("Scripts")]
	[SerializeField] private PlayerGraphics playerGraphics;
	[SerializeField] private PlayerActions playerActions;

	[Header("Targets")]
	[SerializeField] private Transform enemyTarget;
	[SerializeField] private GameObject fadeTarget;

	// Public variables
	public Transform Form { get { return form; } }
	public Rigidbody Body { get { return body; } }
	public NavMeshAgent Agent { get { return agent; } }
	public PlayerGraphics Graphics { get { return playerGraphics; } }
	public PlayerActions Actions { get { return playerActions; } }
	public Vector3 Position { get { return form.position; } }
	public Vector3 EnemyTarget { get { return enemyTarget.position; } }
	public GameObject FadeTarget { get { return fadeTarget; } }
	public bool Moving { get { return agent.velocity.sqrMagnitude > 0.1f; } }
	public bool Initialized { get; private set; }

	// Player state
	private PlayerStates _playerState = PlayerStates.Walking;
	public PlayerStates PlayerState {
		get {
			return _playerState;
		}
		set {
			if (_playerState != value)  {
				_playerState = value;
				OnPlayerChanged?.Invoke(value);
				Helpers.Log("GameManager", "Player state was changed", value.ToString());
			}
		}
	}
	public static Action<PlayerStates> OnPlayerChanged;


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {
		GameManager.Input.Actions.Player.StopMoving.performed += StopMoving;
		GameManager.Input.Actions.Player.Sneaking.performed += Sneaking;
		GameManager.Input.Actions.Player.Running.performed += Running;

		GameManager.OnGameStateChanged += OnGameStateChanged;
		GameManager.OnPauseChanged += OnPauseChanged;

		playerGraphics.Setup();
		playerActions.Setup();

		AgentSpeed();

		GameManager.Camera.SetPlayer(playerGraphics.CameraTarget);

		Initialized = true;
	}

	public void Unload() {
		GameManager.Input.Actions.Player.StopMoving.performed -= StopMoving;
		GameManager.Input.Actions.Player.Sneaking.performed -= Sneaking;
		GameManager.Input.Actions.Player.Running.performed -= Running;

		GameManager.OnGameStateChanged -= OnGameStateChanged;
		GameManager.OnPauseChanged -= OnPauseChanged;

		playerGraphics.Unload();
		playerActions.Unload();
	}

	void OnGameStateChanged(GameStates value) {
		if(!GameManager.CurrentState(GameStates.Player)) { return; }

	}

	void OnPauseChanged(bool value) {
		StopAgent(value);
		Actions.Pause(value);
		Graphics.Pause(value);
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: MOVEMENT

	public void Movement(Vector3 target, bool checkReach = true) {
		if(!GameManager.CurrentState(GameStates.Player)) { return; }

		if(checkReach) {
			if(agent.CanReach(target)) {
				StopAgent(false);
				agent.SetDestination(target);
				playerGraphics.MoveMarker(target);
			}
		}
		else {
			StopAgent(false);
			agent.SetDestination(target);
			playerGraphics.MoveMarker(target);
		}
	}

	public void StopAgent(bool toggle) {
		if(!GameManager.CurrentState(GameStates.Player)) { return; }

		agent.isStopped = toggle;
	}

	void StopMoving(InputAction.CallbackContext ctx) {
		if(!GameManager.CurrentState(GameStates.Player)) { return; }

		StopAgent(true);
		agent.ResetPath();
	}

	void Sneaking(InputAction.CallbackContext ctx) {
		if(!GameManager.CurrentState(GameStates.Player)) { return; }

		if(PlayerState != PlayerStates.Sneaking) {
			PlayerState = PlayerStates.Sneaking;
		}
		else if(PlayerState == PlayerStates.Sneaking) {
			PlayerState = PlayerStates.Walking;
		}
		AgentSpeed();
		Graphics.ToggleSneak(PlayerState == PlayerStates.Sneaking);
	}

	void Running(InputAction.CallbackContext ctx) {
		if(!GameManager.CurrentState(GameStates.Player)) { return; }

		if(PlayerState != PlayerStates.Running) {
			PlayerState = PlayerStates.Running;
		}
		else if(PlayerState == PlayerStates.Running) {
			PlayerState = PlayerStates.Walking;
		}
		AgentSpeed();
		Graphics.ToggleSneak(false);
	}

	void AgentSpeed() {
		switch(_playerState) {
			case PlayerStates.Sneaking:
				agent.speed = GameManager.Config.Player.SneakingSpeed;
				break;
			case PlayerStates.Walking:
				agent.speed = GameManager.Config.Player.WalkingSpeed;
				break;
			case PlayerStates.Running:
				agent.speed = GameManager.Config.Player.RunningSpeed;
				break;
		}
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: ROTATION

	public void TurnTowards(Vector3 position) {
		if(!GameManager.CurrentState(GameStates.Player)) { return; }

		form.DOLookAt(position, 0.5f);
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: FAST TRAVEL

	public void FastTravel(Transform target) {
		if(target != null) {
			agent.isStopped = true;
			agent.updatePosition = false;
			agent.updateRotation = false;
			agent.Warp(target.position);
			agent.nextPosition = target.position;
			transform.rotation = target.rotation;
			agent.updatePosition = true;
			agent.updateRotation = true;
		}
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: COMBAT

	public void Hit(CharacterHit hit) {
		if(!GameManager.CurrentState(GameStates.Player)) { return; }

		Debug.LogWarning("Player is now dead");

		agent.isStopped = true;
		agent.enabled = false;
		capsuleCollider.enabled = false;
		playerGraphics.Dead(hit);
		GameManager.GameState = GameStates.Death;
		GameManager.OnPlayerDied?.Invoke(hit.Source.GetComponent<EnemyController>());
	}
}
}
