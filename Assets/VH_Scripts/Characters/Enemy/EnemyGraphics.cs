using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace SmallTown {
public class EnemyGraphics : MonoBehaviour {

	[Header("Data")]
	[SerializeField] private EnemyData enemyData;

	[Header("Objects")]
	[SerializeField] private EnemyController controller;
	[SerializeField] private Animator animator;
	[SerializeField] private LineRenderer line;
	[SerializeField] private Light visionLight;
	[SerializeField] private Rigidbody hitTarget;
	[SerializeField] private GameObject showVision;
	private Collider[] _colliders;
	private Rigidbody[] _rigidbodies;
	private float _graphicsVelocity = 0.0f;
	// private Coroutine _graphicsRoutine;

	// Public variables
	public Animator Animator { get { return animator; } }


	// ************************************************************************************************************ UNITY FUNCTIONS

	void Update() {
		UpdateGraphics();
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {
		visionLight.spotAngle = controller.Data.DetectionAngle;
		visionLight.range = controller.Data.DetectionDistance;
		_colliders = GetComponentsInChildren<Collider>();
		_rigidbodies = GetComponentsInChildren<Rigidbody>();

		CheckTraits();
	}

	public void Pause(bool toggle) {
		animator.enabled = !toggle;
	}

	void CheckTraits() {
		showVision.SetActive(GameManager.PlayerTraits.Contains("Prescience"));
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: GRAPHICS

	void UpdateGraphics() {
		if(!controller.Initialized || !controller.Stats.IsAlive) return;

		Vector3 velocity = controller.Agent.velocity;
		velocity.y = 0.0f;
		_graphicsVelocity = Mathf.Lerp(_graphicsVelocity, velocity.sqrMagnitude, 10.0f * Time.deltaTime);
		animator.SetFloat("Velocity", _graphicsVelocity);
	}

	public void DrawLine(NavMeshPath path) {


		// ONLY IF PLAYER HAS TRAIT "PRESCIENCE"


		line.positionCount = path.corners.Length;
		for (int i = 0; i < path.corners.Length; i++) {
			if (i == 0) {
				Vector3 pos = path.corners[i] + GameManager.Config.Enemy.LineOffset + controller.Form.forward.normalized;
				line.SetPosition(i, path.corners[i] + GameManager.Config.Enemy.LineOffset + controller.Form.forward.normalized);
			}
			else if (i == path.corners.Length - 1) {
				Vector3 pos = path.corners[i] + GameManager.Config.Enemy.LineOffset - (GameManager.Player.Position - controller.Position).normalized;
				line.SetPosition(i, path.corners[i] + GameManager.Config.Enemy.LineOffset - (GameManager.Player.Position - controller.Position).normalized);
			}
			else {
				line.SetPosition(i, path.corners[i] + GameManager.Config.Enemy.LineOffset);
			}
		}
	}

	public void ClearLine() {
		line.positionCount = 0;
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: COMBAT

	public void Dead(CharacterHit hit) {
		animator.enabled = false;
		foreach (Collider collider in _colliders) {
			collider.enabled = true;
		}
		foreach (Rigidbody ragdollBone in _rigidbodies) {
			if (ragdollBone != controller.Body) {
				ragdollBone.isKinematic = false;
			}
		}
		// Vector3 direction = endPosition - startPosition;
		// direction *= GameManager.Config.Enemy.RagdollMultiplier;
		// direction.y = GameManager.Config.Enemy.RagdollUpwards;

		Vector3 direction = hit.Direction.normalized;
		direction *= hit.WeaponType == WeaponTypes.Melee ? GameManager.Config.Enemy.RagdollMelee.Multiplier : GameManager.Config.Enemy.RagdollFirearms.Multiplier;
		direction.y = hit.WeaponType == WeaponTypes.Melee ? GameManager.Config.Enemy.RagdollMelee.Upwards : GameManager.Config.Enemy.RagdollFirearms.Upwards;
		hitTarget.AddForce(direction, ForceMode.Impulse);
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: MOVEMENT

	public void StoppedMovement() {
		ClearLine();
	}

}
}