using System.Collections;
using System.Collections.Generic;
using Knife.PostProcessing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace SmallTown {
public class Door : MonoBehaviour, IActivate { //, IPointerEnterHandler, IPointerExitHandler {

	[Header("Objects")]
	[SerializeField] private List<OutlineRegister> outlines = new List<OutlineRegister>();
	[SerializeField] private List<BuildingTrigger> buildingTriggers = new List<BuildingTrigger>();
	[SerializeField] private Transform activationPivot;
	[SerializeField] private NavMeshObstacle navMeshObstacle;

	[Header("Values")]
	[SerializeField] private Vector3 openAngle;
	[SerializeField] private Vector3 closedAngle;
	[SerializeField] private DoorStates doorState;

	[Header("Localization")]
	[SerializeField] private string areaKey;

	// Private variables
	private Coroutine _delayRoutine;
	private bool _highlighted = false;


	// ************************************************************************************************************ UNITY FUNCTIONS

	void Start() {
		CheckTriggers(false);
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	void ChangeState(DoorStates state) {
		doorState = state;
		CheckTriggers();
	}

	bool CheckState(DoorStates state) {
		return doorState == state;
	}

	void CheckTriggers(bool playerExit = true) {
		for (int i = 0; i < buildingTriggers.Count; i++) {
			if(playerExit && CheckState(DoorStates.Closed)) {
				buildingTriggers[i].PlayerExit();
			}
			buildingTriggers[i].Toggle(CheckState(DoorStates.Open));
		}
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: HIGHLIGHT

	public void Highlight(bool toggle) {
		_highlighted = toggle;

		if(!toggle) {
			if(_delayRoutine != null) {
				StopCoroutine(_delayRoutine);
			}
			_delayRoutine = StartCoroutine(DelayHide());
		}
		else {
			GameManager.Gui.Hud.ShowTooltip(GetText(true), GetText(), transform.position);
			for (int i = 0; i < outlines.Count; i++) {
				outlines[i].enabled = true;
			}
		}
		for (int i = 0; i < outlines.Count; i++) {
			outlines[i].enabled = toggle;
		}
	}

	string GetText(bool title = false) {
		if(title) {
			if(CheckState(DoorStates.Locked)) {
				return Localize.GetText(areaKey) + " " + Helpers.TextColor(Localize.GetText("Actions/Door/Locked"), GameManager.Config.UI.LockedColor);
			}
			return Localize.GetText(areaKey);
		}
		if(CheckState(DoorStates.Locked)) {

			// if(playerDoestHaveLockPicks) {

			// }

			return Localize.GetText("Actions/Door/Lockpick");
			// return Helpers.TextColor(Localize.GetText("Actions/Door/NoLockpicks"), GameManager.Config.UI.LockedColor);
		}
		if(CheckState(DoorStates.Open)) {
			return Localize.GetText("Actions/Door/Close");
		}
		return Localize.GetText("Actions/Door/Open");
	}


	IEnumerator DelayHide() {
		yield return new WaitForEndOfFrame();
		if(!_highlighted) {
			for (int i = 0; i < outlines.Count; i++) {
				outlines[i].enabled = false;
			}
			GameManager.Gui.Hud.HideTooltip();
		}
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: ACTIVATE

	public void Activate() {

		// Debug.LogWarning("Activating door");

		if(CheckState(DoorStates.Open)) {
			ChangeState(DoorStates.Closed);
			navMeshObstacle.enabled = true;
			MoveDoor(closedAngle);
		}
		else {
			if(CheckState(DoorStates.Locked)) {


				// Lockpick the door if player has lockpicks?


			}
			else {
				ChangeState(DoorStates.Open);
				navMeshObstacle.enabled = false;
				MoveDoor(openAngle);
			}
		}
	}

	void MoveDoor(Vector3 value) {
		if(activationPivot != null) {
			activationPivot.DOKill();
			activationPivot.DOLocalRotate(value, GameManager.Config.Item.DoorMovementDuration);
		}
	}
}
}