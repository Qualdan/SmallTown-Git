using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

namespace SmallTown {
public class EnemyWaypoints : MonoBehaviour {

	[Header("Objects")]
	[SerializeField] private EnemyController controller;
	private List<Waypoint> _waypoints = new List<Waypoint>();

	private int _index = 0;
	private bool _isCircle = false;
	private Coroutine _waypointRoutine;


	// private bool _hasSeenPlayer = false;
	// // [SerializeField] private Animator animator;
	// // private float _graphicsVelocity = 0.0f;
	// // private Coroutine _graphicsRoutine;

	// // Public variables
	public int WaypointCount { get { return _waypoints.Count; } }

	// private bool _meleeAttack = false;



	// ************************************************************************************************************ UNITY FUNCTIONS

	// void Update() {
	// 	NextWaypoint();
	// }


#if UNITY_EDITOR
	void OnDrawGizmosSelected() {
	// 	Tools.current = Tool.None;
	// 	if (target == null) {
	// 		return;
	// 	}
	// 	Vector3 offset = transform.position;
	// 	Vector3 previousPosition = target.position - offset;
	// 	int labelIndex = 0;
	// 	Gizmos.color = new Color(0.12f, 1.0f, 0.43f, 1.0f);
	// 	Gizmos.matrix = transform.localToWorldMatrix;
	// 	GUIStyle style = new GUIStyle();
	// 	style.normal.textColor = Color.green;
	// 	for (int i = 0; i < actions.Count; i++) {
	// 		if (actions[i].action == TrapAction.actions.Move) {
	// 			labelIndex = 0;
	// 			Vector3 moveTo = actions[i].moveTo;
	// 			if (actions[i].moveRelative) {
	// 				moveTo += target.position;
	// 			}
	// 			Gizmos.DrawLine(previousPosition, moveTo);
	// 			Gizmos.DrawWireCube(moveTo, new Vector3(1.0f, 1.0f, 1.0f));
	// 			previousPosition = moveTo;
	// 			Vector3 labelPosition = previousPosition + new Vector3(0.65f, -0.8f, 0.0f);
	// 			Handles.Label(labelPosition + offset, i.ToString(), style);
	// 		}
	// 		else if (actions[i].action == TrapAction.actions.Rotate) {
	// 			labelIndex++;
	// 			if (actions[i].rotatePivot) {
	// 				Gizmos.DrawWireSphere(actions[i].rotatePivotPoint, 0.5f);
	// 				Gizmos.DrawLine(actions[i].rotatePivotPoint, target.position);
	// 			}
	// 			else{
	// 				Gizmos.DrawWireSphere(previousPosition, 0.75f);
	// 			}
	// 			Vector3 labelPosition = previousPosition + new Vector3(0.65f + (0.65f * labelIndex), -0.8f + (-0.6f * labelIndex), 0.0f);
	// 			Handles.Label(actions[i].rotatePivotPoint + offset, i.ToString(), style);
	// 		}
	// 		if (actions[i].action == TrapAction.actions.Teleport) {
	// 			Gizmos.DrawLine(previousPosition, actions[i].moveTo);
	// 			Gizmos.DrawCube(actions[i].moveTo, new Vector3(1.0f, 1.0f, 1.0f));
	// 			previousPosition = actions[i].moveTo;
	// 		}
	// 	}
	}
#endif


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {
		if(_waypointRoutine != null) {
			StopCoroutine(_waypointRoutine);
		}
		_waypointRoutine = StartCoroutine(WaypointRoutine());
	}

	public void Dead() {
		if(_waypointRoutine != null) {
			StopCoroutine(_waypointRoutine);
		}
	}


	public void Pause(bool toggle) {
		// animator.enabled = !toggle;
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: WAYPOINTS

	public void AddWaypoint() {
		Vector3 position = transform.position + GameManager.Config.Enemy.WaypointOffset;
		if(_waypoints.Count > 0) {
			position = _waypoints[_waypoints.Count - 1].transform.position + GameManager.Config.Enemy.WaypointOffset;
		}
		GameObject go = Instantiate(new GameObject(), position, Quaternion.identity);
		_waypoints.Add(new Waypoint(go.transform));
	}

	public void RemoveWaypoint(int index) {
		_waypoints.RemoveAt(index);
	}

	public void IsCircle(bool value) {
		_isCircle = value;
	}



	// void NextWaypoint() {
	// 	if(controller.Agent.HasFinished()) {
	// 		if(_nextWaypointRoutine != null) {
	// 			StopCoroutine(_nextWaypointRoutine);
	// 		}
	// 		_nextWaypointRoutine = StartCoroutine(NextWaypointRoutine());
	// 	}
	// }

	// IEnumerator NextWaypointRoutine() {
	// 	yield return new WaitForSeconds(_waypoints[_index].waitDelay);

	// 	_index++;
	// 	//
	// 	// Move player to next waypoint _waypoints[_index]
	// }



	IEnumerator WaypointRoutine() {
		while(true) {
			while(!controller.Agent.HasFinished()) {
				yield return null;
			}
			yield return new WaitForSeconds(_waypoints[_index].waitDelay);
			_index++;
			// Move player to next waypoint _waypoints[_index]
		}
	}


}
public class Waypoint {
	public Transform transform;
	public int waitDelay;
	public Waypoint(Transform newTransform) {
		transform = newTransform;
		waitDelay = 0;
	}
}
}