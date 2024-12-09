using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SmallTown {
public class EnemyController : MonoBehaviour, ICharacter {

	[Header("Data")]
	public CharacterStats Stats;
	[SerializeField] private EnemyData enemyData;
	[SerializeField] private WeaponData weaponData;

	[Header("Objects")]
	[SerializeField] private Transform form;
	[SerializeField] private Rigidbody body;
	[SerializeField] private NavMeshAgent agent;
	[SerializeField] private CapsuleCollider capsuleCollider;
	[SerializeField] private EnemyActions enemyActions;
	[SerializeField] private EnemyGraphics enemyGraphics;
	[SerializeField] private Transform shootTarget;
	[SerializeField] private Transform circleTarget;
	[SerializeField] private EnemyWeapon enemyWeapon;
	[SerializeField] private List<EnemyColliders> colliders = new List<EnemyColliders>();
	private bool _isPaused = false;
	private bool _isHostile = false;
	private bool _isVisible = false;
	private Coroutine _movementRoutine;

	// Public variables
	public EnemyData Data { get { return enemyData; } }
	public WeaponData Weapon { get { return weaponData; } }
	public Transform Form { get { return form; } }
	public Rigidbody Body { get { return body; } }
	public EnemyActions Actions { get { return enemyActions; } }
	public EnemyGraphics Graphics { get { return enemyGraphics; } }
	public NavMeshAgent Agent { get { return agent; } }
	public Vector3 Position { get { return form.position; } }
	public Vector3 ShootPosition { get { return shootTarget.position; } }
	public Vector3 CirclePosition { get { return circleTarget.position; } }
	public bool IsHostile { get { return _isHostile; } }
	public bool IsVisible { get { return _isVisible; } }
	public bool Initialized { get; private set; }


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {

		// _isHostile = true;


		GameManager.OnPauseChanged += OnPauseChanged;
		GameManager.Settings.OnSettingChanged += SettingsChanged;

		enemyGraphics.Setup();
		SetColliders();

		agent.speed = enemyData.MovementSpeed;
		agent.isStopped = false;

		Initialized = true;
	}

	void SettingsChanged(string key, float value) {
		if(key == "Settings/Difficulty") {
			SetColliders();
		}
	}

	void OnPauseChanged(bool value) {
		_isPaused = value;
		enemyGraphics.Pause(value);
	}

	public void SetHostile() {
		_isHostile = true;
	}

	public void Visible(bool toggle) {
		_isVisible = toggle;
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: MOVEMENT

	public void Movement(Vector3 target) {
		if(!Stats.IsAlive) { return; }

		if (Helpers.CanReach(target, agent)) {
			StopAgent(false);
			agent.SetDestination(target);
			enemyGraphics.DrawLine(agent.path);
			if(_movementRoutine != null) {
				StopCoroutine(_movementRoutine);
			}
			_movementRoutine = StartCoroutine(MovementRoutine());
		}
	}

	IEnumerator MovementRoutine() {
		while(!agent.HasFinished(1.0f)) {
			yield return null;
		}
		// if(!IsVisible) {
		// 	Debug.LogWarning("Lost player");
		// }

		Actions.StoppedMovement();
		Graphics.StoppedMovement();
	}

	void StopAgent(bool toggle) {
		if(!Stats.IsAlive) {
			agent.isStopped = true;
		}
		else if(_isPaused) {
			agent.isStopped = _isPaused;
		}
		else {
			agent.isStopped = toggle;
		}
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: COMBAT

	public void Hit(CharacterHit hit) {
		if(!Stats.IsAlive) { return; }

		Stats.IsAlive = false;
		agent.isStopped = true;
		agent.enabled = false;
		capsuleCollider.enabled = false;
		for (int i = 0; i < colliders.Count; i++) {
			colliders[i].collider.gameObject.SetActive(false);
		}
		GameManager.Player.Actions.ResetEnemy();
		enemyWeapon?.Detach();
		enemyGraphics.Dead(hit);
	}

	void SetColliders() {
		for (int i = 0; i < colliders.Count; i++) {
			colliders[i].collider.gameObject.SetActive(colliders[i].difficulty == GameManager.Difficulty.Level);
		}
	}
}
[Serializable]
public class EnemyColliders {
	public Collider collider;
	public Difficulties difficulty;
}
}