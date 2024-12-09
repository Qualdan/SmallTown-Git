using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace SmallTown {
public class JournalManager : MonoBehaviour, IManager {

	//
	[SerializeField] CanvasGroup canvasGroup;

	[Header("Areas")]
	[SerializeField] JournalItem journalItemPrefab;
	[SerializeField] Transform fastTravelAreas;
	[SerializeField] Transform fastTravelPoints;
	[SerializeField] CanvasGroup fastTravelGroup;
	[SerializeField] CanvasGroup timeGroup;
	[SerializeField] TextMeshProUGUI timeTitle;

	// [SerializeField] Transform separatorPanel;
	[SerializeField] Sprite fastTravelIcon;
	private string _currentArea;
	private string _currentPoint;

	// Public variables
	public bool Initialized { get; private set; }


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {
		GameManager.Input.Actions.Player.ToggleJournal.performed += ctx => ToggleJournal(true);
		GameManager.Input.Actions.Journal.ToggleJournal.performed += ctx => ToggleJournal(false);

		GenerateFastTravel();

		Initialized = true;
		Helpers.Initialized(GetType().Name);
	}

	void ToggleJournal(bool toggle, bool instant = false) {
		if(!GameManager.CurrentState(GameStates.Player) && !GameManager.CurrentState(GameStates.Journal)) { return; }

		if(toggle) {
			GameManager.Player.Actions.StopWalking();
		}
		GameManager.GameState = toggle ? GameStates.Journal : GameStates.Player;
		canvasGroup.DOFade(toggle ? 1.0f : 0.0f, instant ? 0.0f :  GameManager.Config.UI.ChangePanelDuration);
		canvasGroup.blocksRaycasts = toggle;
	}



	// ************************************************************************************************************ CUSTOM FUNCTIONS: AREAS

	void GenerateFastTravel() {
		for (int i = 0; i < GameManager.Config.Area.FastTravelAreas.Count; i++) {
			string key = GameManager.Config.Area.FastTravelAreas[i].AreaKey;
			JournalItem journalItem = Instantiate(journalItemPrefab, fastTravelAreas);
			journalItem?.Setup(Localize.GetText(key), GameManager.Config.Area.FastTravelAreas[i].AreaSprite, true);
			journalItem?.Button?.onClick.AddListener( () => { OnFastTravelArea(key); } );
		}
	}

	void OnFastTravelArea(string areaKey) {
		timeGroup.DOFade(0.0f, GameManager.Config.UI.ChangePanelDuration);
		if(_currentArea == areaKey) {
			_currentArea = string.Empty;
			_currentPoint = string.Empty;
			fastTravelGroup.DOFade(0.0f, GameManager.Config.UI.ChangePanelDuration);
			fastTravelGroup.blocksRaycasts = false;
			timeGroup.blocksRaycasts = false;
		}
		else {
			_currentArea = areaKey;
			foreach (Transform child in fastTravelPoints) {
				Destroy(child.gameObject);
			}
			for (int i = 0; i < GameManager.Config.Area.FastTravelAreas.Count; i++) {
				if(GameManager.Config.Area.FastTravelAreas[i].AreaKey == areaKey) {
					for (int f = 0; f < GameManager.Config.Area.FastTravelAreas[i].FastTravelPoints.Count; f++) {
						string key = GameManager.Config.Area.FastTravelAreas[i].FastTravelPoints[f];
						JournalItem journalItem = Instantiate(journalItemPrefab, fastTravelPoints);
						journalItem?.Setup(Localize.GetText(key), fastTravelIcon, true);
						int index = f;
						journalItem?.Button?.onClick.AddListener( () => { OnFastTravelPoint(key); } );
					}
					break;
				}
			}
			fastTravelGroup.DOFade(1.0f, GameManager.Config.UI.ChangePanelDuration);
			fastTravelGroup.blocksRaycasts = true;
		}
	}

	void OnFastTravelPoint(string pointKey) {
		if(_currentPoint == pointKey) {
			_currentPoint = string.Empty;
			timeGroup.DOFade(0.0f, GameManager.Config.UI.ChangePanelDuration);
			timeGroup.blocksRaycasts = false;
		}
		else {
			_currentPoint = pointKey;
			timeTitle.SetText(Localize.GetText("Time/Arrive") + " " + Localize.GetText(pointKey));
			timeGroup.DOFade(1.0f, GameManager.Config.UI.ChangePanelDuration);
			timeGroup.blocksRaycasts = true;
		}
	}

	public void OnTime(bool dayTime) {
		ToggleJournal(false, true);
		GameManager.Area.FastTravel(_currentArea, _currentPoint, dayTime);
	}
}
}