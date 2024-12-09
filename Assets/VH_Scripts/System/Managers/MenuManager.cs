using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.Assertions.Must;

namespace SmallTown {
public class MenuManager : MonoBehaviour, IManager {

	// [Header("Logo")]
	// [SerializeField] private RectTransform logoPanel;
	// [SerializeField] private CanvasGroup logoGroup;

	[Header("Canvas Groups")]
	[SerializeField] private List<PanelGroups> canvasGroups = new List<PanelGroups>();
	// [SerializeField] private GameObject mainMenuPanel;
	// [SerializeField] private GameObject pauseMenuPanel;
	[SerializeField] private CanvasGroup mainMenuGroup;
	[SerializeField] private CanvasGroup pauseMenuGroup;
	[SerializeField] private GameObject mainMenuButton;
	private CanvasGroup _currentGroup = null;
	private MenuStates _menuState = MenuStates.None;

	// Public variables
	public bool Initialized { get; private set; }


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {
		GameManager.OnGameStateChanged += OnGameStateChanged;
		GameManager.OnPauseChanged += OnPauseChanged;
		GameManager.Input.Actions.Menu.Cancel.performed += ctx => CancelPressed();

		// StartCoroutine(StartupAnimations());

		Initialized = true;
		Helpers.Initialized(GetType().Name);
	}

	public void ChangeState(MenuStates newState) {
		if (_menuState == newState) {
			_menuState = MenuStates.None;
		}
		else {
			_menuState = newState;
		}
	}

	public bool CheckState(MenuStates checkState) {
		return _menuState == checkState;
	}

	void CancelPressed() {
		if (GameManager.GameState == GameStates.Menu || GameManager.GameState == GameStates.Pause) {
			if(_currentGroup != null) {
				ToggleGroup(_currentGroup);
			}
			else {
				GameManager.Instance.TogglePause(false);
			}
		}
	}

	public void OnGameStateChanged(GameStates value) {

	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: MAIN MENU

	void ShowMainMenu(bool value) {
		mainMenuGroup.blocksRaycasts = value;
		// mainButtonsPanel.DOAnchorPosY(toggle ? 80.0f : -160.0f, 0.5f);
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: PAUSE MENU

	void OnPauseChanged(bool value) {
		pauseMenuGroup.blocksRaycasts = value;
		pauseMenuGroup.DOComplete();
		pauseMenuGroup.DOFade(value ? 1.0f : 0.0f, GameManager.Config.General.PauseSpeed);
		mainMenuButton.SetActive(value);
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: ANIMATIONS

	// IEnumerator StartupAnimations() {
	// 	yield return new WaitForSeconds(2.0f);
	// 	yield return blocker.DOFade(0.0f, 2.0f).WaitForCompletion();
	// 	logoGroup.DOFade(1.0f, 6.0f);
	// 	yield return logoPanel.DOAnchorPosX(0.0f, 2.0f).SetEase(Ease.OutQuart).WaitForCompletion();
	// 	yield return mainButtonsPanel.DOAnchorPosY(80.0f, 0.5f).WaitForCompletion();
	// 	logoO.DOFade(1.0f, 10.0f);
	// }


	// ************************************************************************************************************ CUSTOM FUNCTIONS: BUTTONS

	public void ResumeButton() {
		if(_currentGroup != null) {
			ToggleGroup(_currentGroup);
		}
		GameManager.Instance.TogglePause(false);
	}

	public void MainMenuButton() {
		Debug.LogWarning("Main menu button pressed");
	}

	public void QuitButton() {
		Debug.LogWarning("Quit button pressed");
		Application.Quit();
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: PANELS

	public void ToggleGroup(CanvasGroup newGroup) {
		for (int i = 0; i < canvasGroups.Count; i++) {
			canvasGroups[i].group.DOComplete();
		}
		for (int i = 0; i < canvasGroups.Count; i++) {
			if(canvasGroups[i].group == newGroup) {
				if (CheckState(canvasGroups[i].menu)) {
					ChangeState(MenuStates.None);
					FadeGroup(canvasGroups[i].group, 0.0f);
					_currentGroup = null;
				}
				else {
					canvasGroups[i].group.blocksRaycasts = true;
					CanvasGroup changeGroup = canvasGroups[i].group;
					ChangeState(canvasGroups[i].menu);
					if(_currentGroup != null) {
						_currentGroup.DOFade(0.0f, GameManager.Config.UI.ChangePanelDuration).OnComplete(()=> FadeGroup(changeGroup, 1.0f));
					}
					else {
						FadeGroup(changeGroup, 1.0f);
					}
					_currentGroup = changeGroup;
				}
				break;
			}
		}
	}

	void FadeGroup(CanvasGroup group, float value) {
		group.DOFade(value, GameManager.Config.UI.ChangePanelDuration).OnComplete(CheckGroups);
	}

	void CheckGroups() {
		for (int i = 0; i < canvasGroups.Count; i++) {
			canvasGroups[i].group.blocksRaycasts = canvasGroups[i].group.alpha > 0.0f;
		}
		if(!CheckState(MenuStates.Play)) {
			GameManager.Gui.NewGame.HideContent();
		}
		if(!CheckState(MenuStates.Settings)) {
			GameManager.Gui.Settings.HideContent();
		}
		if(!CheckState(MenuStates.Tutorial)) {
			GameManager.Gui.Tutorial.HideContent();
		}
		GameManager.Gui.Credits.RunRoutine();
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: SUBPANELS

	public void ToggleSubGroup(CanvasGroup newGroup, IMenuGroup menuGroup) {
		for (int i = 0; i < menuGroup.CanvasGroups().Count; i++) {
			menuGroup.CanvasGroups()[i].DOComplete();
		}
		if(newGroup == menuGroup.GetCurrentGroup()) {
			FadeSubGroup(menuGroup.GetCurrentGroup(), 0.0f, menuGroup);
			menuGroup.SetCurrentGroup(null);
		}
		else {
			menuGroup.CheckGroups(newGroup, true);
			if (menuGroup.GetCurrentGroup() != null) {
				menuGroup.GetCurrentGroup().DOFade(0.0f, GameManager.Config.UI.ChangePanelDuration).OnComplete(()=> FadeSubGroup(menuGroup.GetCurrentGroup(), 1.0f, menuGroup));
			}
			else {
				FadeSubGroup(newGroup, 1.0f, menuGroup);
			}
			menuGroup.SetCurrentGroup(newGroup);
		}
	}

	void FadeSubGroup(CanvasGroup group, float value, IMenuGroup menuGroup) {
		group.DOFade(value, GameManager.Config.UI.ChangePanelDuration).OnComplete(() => menuGroup.CheckGroups(group));
	}
}

[Serializable]
public class PanelGroups {
	public GameObject panel;
	public CanvasGroup group;
	public MenuStates menu;
}
}