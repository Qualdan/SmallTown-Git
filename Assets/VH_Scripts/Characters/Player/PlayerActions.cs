// using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace SmallTown {
public class PlayerActions : MonoBehaviour {

	// Private variables
	[SerializeField] private PlayerController controller;
	[SerializeField] private GameObject testingSphere;
	private Vector2 _mousePosition = Vector2.zero;
	private EnemyController _targetEnemy = null;
	private Coroutine _targetingRoutine = null;
	private Coroutine _walkingRoutine = null;
	private Coroutine _activateRoutine = null;
	private bool _targetingEnemy = false;
	private float _targetingSize = 0.0f;

	private IActivate _activatedObject;
	// private List<IActivate> _activates = new List<IActivate>();
	// private List<IActivate> _activatesCurrent = new List<IActivate>();
	// private List<IActivate> _activatesTemp = new List<IActivate>();

	// Public variables
	public Vector2 Mouse { get { return _mousePosition; } }


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {
		GameManager.Input.Actions.Player.Point.performed += Point;
		GameManager.Input.Actions.Player.Action.performed += ActionPerformed;
		GameManager.Input.Actions.Player.Action.canceled += ActionCanceled;
	}

	public void Unload() {
		GameManager.Input.Actions.Player.Point.performed -= Point;
		GameManager.Input.Actions.Player.Action.performed -= ActionPerformed;
		GameManager.Input.Actions.Player.Action.canceled -= ActionCanceled;
	}

	public void Pause(bool toggle) {

	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: POINT

	void Point(InputAction.CallbackContext ctx) {
		if(!GameManager.CurrentState(GameStates.Player)) { return; }

		_mousePosition = ctx.ReadValue<Vector2>();

		if(_activatedObject != null) {
			_activatedObject.Highlight(false);
			_activatedObject = null;
		}

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(_mousePosition);
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, ~Physics.IgnoreRaycastLayer, QueryTriggerInteraction.Ignore)) {
			if (hit.transform.CompareTag("Activate")) {
				IActivate activate = hit.transform.GetComponent<IActivate>();
				if(activate != null) {
					_activatedObject = activate;
					activate.Highlight(true);
				}
			}
			// else if (hit.transform.CompareTag("Walkable")) {
			// else {
			// 	if(controller.Agent.CanReach(hit.point)) {
			// 		// Debug.LogWarning("Can reach");
			// 		controller.Graphics.PointerMarker(hit.point, GameManager.Config.Player.MarkerAllowedColor);
			// 	}
			// 	else {
			// 		// Debug.LogWarning("Can't reach");
			// 		controller.Graphics.PointerMarker(hit.point, GameManager.Config.Player.MarkerDisallowedColor);
			// 	}
			// }

		}
	}


// ************************************************************************************************************ CUSTOM FUNCTIONS: ACTION

	void ActionPerformed(InputAction.CallbackContext ctx) {
		Action(ctx);
	}

	void ActionCanceled(InputAction.CallbackContext ctx) {
		Action(ctx, true);
	}

	void Action(InputAction.CallbackContext ctx, bool canceled = false) {
		if(!GameManager.CurrentState(GameStates.Player)) { return; }

		if(_targetingEnemy) {
			if(!canceled && _targetEnemy != null) {

				// ON Circle (not inside): Random.insideUnitCircle.normalized;

				// If using firearm: shoot
				Vector2 randomPosition = Random.insideUnitCircle * _targetingSize;
				Vector3 position = _targetEnemy.ShootPosition + new Vector3(randomPosition.x, 0.0f, randomPosition.y);
				Vector3 direction = position - controller.Graphics.BulletPosition;
				Vector3 endPosition = Vector3.zero;
				bool hitEnemy = false;
				RaycastHit raycastHit;
				if (Physics.Raycast(controller.Graphics.BulletPosition, direction, out raycastHit, 100.0f, ~GameManager.Config.Enemy.TargetingMask, QueryTriggerInteraction.Ignore)) {
					endPosition = raycastHit.point;
					if(raycastHit.transform.CompareTag("Enemy")) {
						hitEnemy = true;
					}
				}
				else {
					Debug.LogWarning("Didn't hit anything");
					endPosition = controller.Graphics.BulletPosition + direction * 100.0f;
				}
				CharacterHit hit = new CharacterHit(hitEnemy ? _targetEnemy : null, controller.gameObject, WeaponTypes.Pistol, controller.Graphics.BulletPosition, endPosition);
				GameManager.Area.Level.Item.ShootBullet(hit);
				GameManager.Area.Level.Item.ShowMuzzleFlash(controller.Graphics.BulletPosition);

				if(GameManager.Config.Debug.Logging == LogStates.Extended) {
					Debug.DrawRay(controller.Graphics.BulletPosition, direction, Color.red, 30.0f);
					Instantiate(testingSphere, position, Quaternion.identity);
				}


				// If using melee: move to Enemy, if near Enemy, stab
				// If no weapon, do nothing
			}
		}
		else {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(_mousePosition);
			if (Physics.Raycast (ray, out hit, Mathf.Infinity, ~Physics.IgnoreRaycastLayer, QueryTriggerInteraction.Ignore)) {

				// Debug.LogWarning("Hit: " + hit.transform.name + " (" + hit.transform.tag + ")");

				if (hit.transform.CompareTag("Activate")) {
					if (!canceled) {
						if(Vector3.Distance(controller.Position, hit.transform.position) <= GameManager.Config.Player.ActivationDistance) {
							ActivateObject(hit.transform);
						}
						else {
							controller.Movement(hit.transform.position, false);
							StopRoutine(_activateRoutine);
							_activateRoutine = StartCoroutine(ActivateRoutine(hit.transform));
						}
					}
				}
				else if (hit.transform.CompareTag("Item")) {
					if (!canceled) {
						Debug.LogWarning("Item");
						// Item(_targetObject);
						controller.Movement(hit.point);
					}
				}
				else if (hit.transform.CompareTag("Walkable")) {
					if (canceled) {
						StopRoutine(_walkingRoutine);
					}
					else {
						if(ctx.interaction is HoldInteraction) {
							StopRoutine(_walkingRoutine);
							_walkingRoutine = StartCoroutine(WalkingRoutine());
						}
						else {
							if(!controller.Agent.CanReach(hit.point)) {
								controller.Graphics.DenyMarker(hit.point);
							}
							controller.Movement(hit.point);
						}
					}
				}
				else {
					controller.Graphics.DenyMarker(hit.point);
				}
			}
			else {
				if (canceled) {
					StopRoutine(_walkingRoutine);
				}
			}
		}
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: MOVEMENT

	IEnumerator WalkingRoutine() {
		RaycastHit hit;
		Ray ray;
		while(true) {
			ray = Camera.main.ScreenPointToRay(_mousePosition);
			if (Physics.Raycast (ray, out hit, Mathf.Infinity, ~Physics.IgnoreRaycastLayer, QueryTriggerInteraction.Ignore)) {
				if (hit.transform.CompareTag("Walkable")) {
					controller.Movement(hit.point);
				}
			}
			yield return null;
		}
	}

	public void StopWalking() {
		StopRoutine(_walkingRoutine);
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: ACTIVATE

	void ActivateObject(Transform target) {
		IActivate activateObject = target.GetComponent<IActivate>();
		if(activateObject != null) {
			activateObject.Activate();
		}
	}

	IEnumerator ActivateRoutine(Transform target) {
		float distance = Vector3.Distance(GameManager.Player.Position, target.position);
		while(distance > GameManager.Config.Player.ActivationDistance) {
			distance = Vector3.Distance(GameManager.Player.Position, target.position);
			yield return null;
		}
		controller.StopAgent(true);
		ActivateObject(target);


		// while(!controller.Agent.HasFinished(GameManager.Config.Player.ActivationDistance)) {
			// Debug.LogWarning("> Still trying to reach activate object");
		// 	yield return null;
		// }
		// ActivateObject(target);


		// Vector3 targetPosition = target.position;
		// targetPosition.y = GameManager.Player.Position.y;

		// float currentDistance = Vector3.Distance(GameManager.Player.Position, targetPosition);
		// Debug.LogWarning("> Current Distance is " + currentDistance + ", " + GameManager.Player.Position + " / " + targetPosition);
		// if(Vector3.Distance(GameManager.Player.Position, targetPosition) <= GameManager.Config.Player.ActivationDistance) {
		// 	// Debug.LogWarning("> Close enough");
		// 	ActivateObject(target);
		// }
	}





	// ************************************************************************************************************ CUSTOM FUNCTIONS: ENEMIES

	public void TargetEnemy(EnemyController target) {
		if(!GameManager.CurrentState(GameStates.Player)) { return; }

		if (Helpers.CanSee(target.CirclePosition)) {
			_targetingEnemy = true;
			_targetEnemy = target;
			controller.Graphics.ToggleLine(true);
			StopRoutine(_targetingRoutine);
			_targetingRoutine = StartCoroutine(TargetingEnemy());
		}
	}

	public void ResetEnemy() {

		_targetingEnemy = false;
		_targetEnemy = null;

		controller.Graphics.ToggleLine(false);
		StopRoutine(_targetingRoutine);
	}

	IEnumerator TargetingEnemy() {
		_targetingSize = 0.0f;
		float offset = 0.0f;
		float distance = 0.0f;
		float minimum = GameManager.Config.Enemy.TargetingCircle.Minimum;
		float maximum = GameManager.Config.Enemy.TargetingCircle.Maximum;
		while(_targetingEnemy && GameManager.CurrentState(GameStates.Player)) {
			if(_targetEnemy != null && Helpers.CanSee(_targetEnemy.CirclePosition) && !GameManager.Player.Moving) {
				controller.TurnTowards(_targetEnemy.Position);
				controller.Graphics.ToggleLine(true);
				offset += GameManager.Difficulty.TargetingSpeed * Time.deltaTime;
				distance = (Vector3.Distance(GameManager.Player.Position, _targetEnemy.Position) / 5.0f) - offset;
				_targetingSize = Mathf.Clamp(distance, minimum, maximum);
				controller.Graphics.SetupLine(_targetEnemy.CirclePosition, _targetingSize);
			}
			else {
				controller.Graphics.ToggleLine(false);
			}
			yield return null;
		}
	}






	// ************************************************************************************************************ CUSTOM FUNCTIONS: COROUTINES

	void StopRoutine(Coroutine routine) {
		if(routine != null) {
			StopCoroutine(routine);
		}
	}


}
}