using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SmallTown {
public class PlayerGraphics : MonoBehaviour {

	// Private variables
	[SerializeField] private PlayerController controller;
	[SerializeField] private Animator animator;
	[SerializeField] private Transform cameraTarget;
	[SerializeField] private MoveMarker moveMarker;
	[SerializeField] private LineRenderer targetLine;
	[SerializeField] private Rigidbody hitTarget;
	private Collider[] _colliders;
	private Rigidbody[] _rigidbodies;
	private float _graphicsVelocity = 0.0f;
	private Coroutine _graphicsRoutine;
	private Coroutine _moveMarkerRoutine;
	private Vector3 _cameraTargetOffset;

	// Public variables
	public Transform CameraTarget { get { return cameraTarget; } }
	public Vector3 BulletPosition { get { return cameraTarget.position; } }


	// ************************************************************************************************************ UNITY FUNCTIONS

	void Update() {
		UpdateGraphics();
		MoveCameraTarget();
	}


	void MoveGraphics() {

		// Difference between Graphics position and ground position? (Sometimes the player seems to float or sink into ground)

	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {
		_cameraTargetOffset = cameraTarget.localPosition;
		targetLine.positionCount = GameManager.Config.Player.LineSegments + 1;
		_colliders = GetComponentsInChildren<Collider>();
		_rigidbodies = GetComponentsInChildren<Rigidbody>();
	}

	public void Unload() {
	}

	public void Pause(bool toggle) {
		animator.enabled = !toggle;
	}

	void UpdateGraphics() {
		if(!controller.Initialized) return;

		Vector3 velocity = controller.Agent.velocity;
		velocity.y = 0.0f;
		_graphicsVelocity = Mathf.Lerp(_graphicsVelocity, velocity.sqrMagnitude, 10.0f * Time.deltaTime);
		animator.SetFloat("Velocity", _graphicsVelocity);
	}

	void MoveCameraTarget() {
		if(!controller.Initialized) return;

		cameraTarget.position = Vector3.Lerp(cameraTarget.position, controller.Form.position + _cameraTargetOffset, GameManager.Config.Camera.FollowSpeed * Time.deltaTime);
	}

	public void ToggleSneak(bool toggle) {
		StartGraphicsRoutine(ToggleLayerRoutine("Sneak", toggle ? 1.0f : 0.0f));
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: ANIMATOR LAYERS

	void StartGraphicsRoutine(IEnumerator routine) {
		if (_graphicsRoutine != null) {
			StopCoroutine(_graphicsRoutine);
		}
		_graphicsRoutine = StartCoroutine(routine);
	}

	IEnumerator ToggleLayerRoutine(string layer, float end) {
		int layerIndex = animator.GetLayerIndex(layer);
		float start = animator.GetLayerWeight(layerIndex);
		float lerp = 0.0f;
		while(lerp < 1.0f) {
			lerp += 3.0f * Time.deltaTime;
			animator.SetLayerWeight(layerIndex, Mathf.Lerp(start, end, lerp));
			yield return null;
		}
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: MOVE MARKER

	public void MoveMarker(Vector3 target) {
		if (_moveMarkerRoutine != null) {
			StopCoroutine(_moveMarkerRoutine);
		}
		_moveMarkerRoutine = StartCoroutine(MoveMarkerRoutine(target));
	}

	IEnumerator MoveMarkerRoutine(Vector3 target) {
		moveMarker.Show(target);
		while(!controller.Agent.HasFinished(GameManager.Config.Player.MarkerHideDistance)) {
			yield return null;
		}
		moveMarker.Hide();
	}

	public void DenyMarker(Vector3 target) {
		moveMarker.Deny(target);
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: TARGET LINE

	public void ToggleLine(bool toggle) {
		targetLine.gameObject.SetActive(toggle);
	}

	public void SetupLine(Vector3 target, float size) {
		targetLine.transform.position = target;
		Vector3 linePosition = Vector3.zero;
		float angle = GameManager.Config.Player.LineAngle;
		for (int i = 0; i < targetLine.positionCount; i++) {
			linePosition.x = Mathf.Sin (Mathf.Deg2Rad * angle) * size;
			linePosition.z = Mathf.Cos (Mathf.Deg2Rad * angle) * size;
			targetLine.SetPosition(i, linePosition);
			angle += 360.0f / GameManager.Config.Player.LineSegments;
		}
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: COMBAT

	public void Dead(CharacterHit hit) {
		animator.enabled = false;
		foreach (Collider collider in _colliders) {
			collider.enabled = true;
		}
		foreach (Rigidbody rigidbody in _rigidbodies) {
			if (rigidbody != controller.Body) {
				rigidbody.isKinematic = false;
			}
		}
		Vector3 direction = hit.Direction.normalized;
		direction *= hit.WeaponType == WeaponTypes.Melee ? GameManager.Config.Player.RagdollMelee.Multiplier : GameManager.Config.Player.RagdollFirearms.Multiplier;
		direction.y = hit.WeaponType == WeaponTypes.Melee ? GameManager.Config.Player.RagdollMelee.Upwards : GameManager.Config.Player.RagdollFirearms.Upwards;
		hitTarget.AddForce(direction, ForceMode.Impulse);
	}
}
}