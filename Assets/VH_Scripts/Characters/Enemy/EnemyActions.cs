using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

namespace SmallTown {
public class EnemyActions : MonoBehaviour {

	[Header("Objects")]
	[SerializeField] private EnemyController controller;
	private bool _hasSeenPlayer = false;
	// [SerializeField] private Animator animator;
	// private float _graphicsVelocity = 0.0f;
	// private Coroutine _graphicsRoutine;

	// Public variables
	// public Animator Animator { get { return animator; } }

	private bool _meleeAttack = false;



	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {

	}

	public void Pause(bool toggle) {
		// animator.enabled = !toggle;
	}



	// ************************************************************************************************************ CUSTOM FUNCTIONS: DETECTION

	void Update() {
		Look();
	}


#if UNITY_EDITOR
	void OnDrawGizmosSelected() {
		if(controller != null && controller.Data != null) {
			UnityEditor.Handles.color = new Color(0.8f, 0.0f, 0.0f, 0.25f);
			Vector3 rotatedForward = Quaternion.Euler(0.0f, -controller.Data.DetectionAngle * 0.5f, 0.0f) * controller.Form.forward;
			UnityEditor.Handles.DrawSolidArc(controller.Position, Vector3.up, rotatedForward, controller.Data.DetectionAngle, controller.Data.DetectionDistance);
		}
	}
#endif

	public void Look() {
		if(controller.Data.LookingForPlayer) {

				// If can see player: stop patrol
				// > If is hostile:
				// >> Attack

				if(!GameManager.Initialized || !GameManager.Player.Initialized || !controller.IsVisible) { return; }

				Vector3 direction = GameManager.Player.Position - controller.Position;
				direction.y = 0.0f;

				if(direction.magnitude <= controller.Data.DetectionDistance) {
					if(Vector3.Dot(direction.normalized, controller.Form.forward) > Mathf.Cos(controller.Data.DetectionAngle * 0.5f * Mathf.Deg2Rad)) {
						if(Helpers.CanSee(controller.ShootPosition)) {
							_hasSeenPlayer = true;
							if(controller.IsHostile) {
								Attack();
							}
							else {
								controller.Movement(GameManager.Player.EnemyTarget);
							}
							// Debug.LogWarning("Player was detected " + Time.realtimeSinceStartup);
						}
					}
				}
		}
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: COMBAT

	public void Attack() {

		if (controller.Weapon.Type == WeaponTypes.Melee) {
			// Move to player and attack
			_meleeAttack = true;
			controller.Movement(GameManager.Player.Position);

		}
		else {
			// Shoot
		}

	/*s

	Bullet hit ratio:
	> Easy: every 3rd bullet can hit
	> Medium: every 2rd bullet can hit
	> Easy: every bullet can hit

	*/


	}


	public void StoppedMovement() {
		if(!controller.Stats.IsAlive) { return; }

		if(Helpers.PlayerDistance(controller.Form, 2.0f)) {
			controller.Form.DOLookAt(GameManager.Player.Position, 0.5f);
			if (_meleeAttack) {
				CharacterHit hit = new CharacterHit(GameManager.Player.Controller, controller.gameObject, WeaponTypes.Melee, controller.Form.position, GameManager.Player.Position);
				GameManager.Player.Controller.Hit(hit);
			}
		}
	}



}
}