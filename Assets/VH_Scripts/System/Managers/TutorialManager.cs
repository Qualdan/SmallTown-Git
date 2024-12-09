using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SmallTown {
public class TutorialManager : MonoBehaviour, IManager, IMenuGroup {

	// Variables
	[SerializeField] List<CanvasGroup> canvasGroups = new List<CanvasGroup>();
	private CanvasGroup _currentGroup;

	// Public variables
	public bool Initialized { get; private set; }


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {


		Initialized = true;
		Helpers.Initialized(GetType().Name);
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: GROUPS

	public List<CanvasGroup> CanvasGroups() {
		return canvasGroups;
	}

	public CanvasGroup GetCurrentGroup() {
		return _currentGroup;
	}

	public void SetCurrentGroup(CanvasGroup group) {
		_currentGroup = group;
	}

	public void CheckGroups(CanvasGroup group, bool alsoCurrent = false) {
		for (int i = 0; i < canvasGroups.Count; i++) {
			if(alsoCurrent) {
				canvasGroups[i].gameObject.SetActive(group == canvasGroups[i] || _currentGroup == canvasGroups[i]);
			}
			else {
				canvasGroups[i].gameObject.SetActive(_currentGroup == canvasGroups[i]);
			}
		}
	}

	public void ToggleGroup(CanvasGroup newGroup) {
		GameManager.Gui.Menu.ToggleSubGroup(newGroup, this);
	}

	// public void ToggleGroups(CanvasGroup newGroup) {
	// 	for (int i = 0; i < groups.Count; i++) {
	// 		groups[i].DOComplete();
	// 	}
	// 	if(newGroup == _currentGroup) {
	// 		FadeGroup(_currentGroup, 0.0f);
	// 		_currentGroup = null;
	// 	}
	// 	else {
	// 		CheckGroups(newGroup);
	// 		if (_currentGroup != null) {
	// 			_currentGroup.DOFade(0.0f, GameManager.Config.UI.ChangePanelSpeed).OnComplete(()=> FadeGroup(newGroup, 1.0f));
	// 		}
	// 		else {
	// 			FadeGroup(newGroup, 1.0f);
	// 		}
	// 		_currentGroup = newGroup;
	// 	}
	// }

	// void FadeGroup(CanvasGroup group, float value) {
	// 	group.DOFade(value, GameManager.Config.UI.ChangePanelSpeed).OnComplete(() => CheckGroups(_currentGroup));
	// }

	// void CheckGroups(CanvasGroup group) {
	// 	for (int i = 0; i < groups.Count; i++) {
	// 		groups[i].gameObject.SetActive(group == groups[i]);
	// 	}
	// }

	public void HideContent() {
		_currentGroup = null;
		CheckGroups(null);
		// _currentGroup = null;
		// GameManager.Gui.Menu.CheckSubGroups(null, canvasGroups);
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS:

}
}