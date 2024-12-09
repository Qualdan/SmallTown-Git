using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;
using TMPro;

namespace SmallTown {
public class HudManager : MonoBehaviour, IManager {

	[Header("Objects")]
	[SerializeField] private Canvas canvas;
	[SerializeField] private GameObject hudPanel;
	[SerializeField] private CanvasGroup cameraPanel;
	[SerializeField] private TooltipPanel tooltipPanel;

	[Header("Loader")]
	[SerializeField] private CanvasGroup loaderGroup;
	[SerializeField] private TextMeshProUGUI loaderTitle;
	[SerializeField] private TextMeshProUGUI loaderSubtitle;
	[SerializeField] private GameObject loaderProgressPanel;
	[SerializeField] private Image loaderProgressBar;

	[Header("Locations")]
	[SerializeField] private TextMeshProUGUI locationPrimaryText;
	[SerializeField] private TextMeshProUGUI locationSecondaryText;

	[Header("Messages")]
	[SerializeField] private TextMeshProUGUI messageText;

	[Header("Death")]
	[SerializeField] private CanvasGroup deathPanel;
	[SerializeField] private TextMeshProUGUI deathText;

	// Public variables
	public Canvas Canvas { get { return canvas; } }
	public bool Initialized { get; private set; }


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {

		GameManager.OnPlayerDied += OnPlayerDied;
		GameManager.OnGameStateChanged += OnGameStateChanged;

		GameManager.Area.Location.OnLocationChanged += OnLocationChanged;

		Initialized = true;
		Helpers.Initialized(GetType().Name);
	}

	public void OnGameStateChanged(GameStates value) {
		hudPanel.SetActive(value == GameStates.Player || value == GameStates.Camera);
		cameraPanel.DOComplete();
		cameraPanel.DOFade(value == GameStates.Camera ? 1.0f : 0.0f, 0.5f);
	}



	// ************************************************************************************************************ CUSTOM FUNCTIONS: LOCATIONS

	public void OnLocationChanged() {
		locationPrimaryText.SetText(GameManager.Area.Location.Primary());
		locationSecondaryText.SetText(GameManager.Area.Location.Secondary());
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: MESSAGES

	public void ShowMessage(string text) {
		messageText.SetText(text);
		messageText.DOFade(1.0f, GameManager.Config.UI.MessageFadeDuration);
	}

	public void HideMessage() {
		messageText.DOFade(0.0f, GameManager.Config.UI.MessageFadeDuration);
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: TOOLTIP

	public void ShowTooltip(string title, string description, Vector3 position) {
		tooltipPanel.Setup(title, description, position);
	}

	public void HideTooltip() {
		tooltipPanel.Hide();
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: LOADER

	public void Loader(bool toggle, string title = "", string subtitle = "") {
		if(!string.IsNullOrEmpty(title)) { loaderTitle.SetText(title); }
		if(!string.IsNullOrEmpty(subtitle)) { loaderSubtitle.SetText(subtitle); }
		loaderProgressPanel.SetActive(false);
		loaderGroup.DOFade(toggle ? 1.0f : 0.0f, GameManager.Config.UI.ChangePanelDuration);
	}

	public void LoaderProgress(float value) {
		loaderProgressPanel.SetActive(true);
		loaderProgressBar.fillAmount = value;
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: DEATH

	public void OnPlayerDied(EnemyController controller) {
		StartCoroutine(DeathDelay(controller));
	}

	IEnumerator DeathDelay(EnemyController controller) {
		yield return new WaitForSeconds(2.0f);
		deathText.SetText(Localize.GetText("Enemy/" + controller.Data.Faction.ToString() + "/EndScreen"));
		deathPanel.blocksRaycasts = true;
		deathPanel.DOComplete();
		deathPanel.DOFade(1.0f, 0.5f);
	}

	public void Retry() {
		deathPanel.blocksRaycasts = false;
		deathPanel.DOComplete();
		deathPanel.DOFade(0.0f, 0.0f);
	}

}
}