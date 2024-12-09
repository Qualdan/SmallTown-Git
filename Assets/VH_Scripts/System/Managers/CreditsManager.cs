using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SmallTown {
public class CreditsManager : MonoBehaviour, IManager {

	// Private variables
	[SerializeField] private RectTransform creditsPanel;
	[SerializeField] private RectTransform creditsParent;
	[SerializeField] private ScrollRect creditsScroll;
	[SerializeField] private GameObject creditsHeadlinePrefab;
	[SerializeField] private GameObject creditsEntryPrefab;
	private Coroutine _creditsRoutine = null;
	private bool _creditsPause = false;

	// Public variables
	public bool Initialized { get; private set; }


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {
		GameManager.OnLocaleChanged += ResetCredits;

		SetupCredits();

		Initialized = true;
		Helpers.Initialized(GetType().Name);
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: SETUP

	void SetupCredits() {

		// Padding
		VerticalLayoutGroup group = creditsParent.GetComponent<VerticalLayoutGroup>();
		RectOffset padding = group.padding;
		padding.top = Mathf.RoundToInt(creditsPanel.sizeDelta.y);
		padding.bottom = Mathf.RoundToInt(creditsPanel.sizeDelta.y);
		group.padding = padding;

		// Texts
		GameObject go = null;
		for (int i = 0; i < GameManager.Credits.Titles.Count; i++) {
			go = Instantiate(creditsHeadlinePrefab, creditsParent);
			SetCreditsText(go, GameManager.Credits.Titles[i].Key);
			for (int c = 0; c < GameManager.Credits.Titles[i].Entries.Count; c++) {
				go = Instantiate(creditsEntryPrefab, creditsParent);
				SetCreditsText(go, GameManager.Credits.Titles[i].Entries[c].Title, GameManager.Credits.Titles[i].Entries[c].Name);
			}
		}
	}

	void SetCreditsText(GameObject go, string title, string name = "") {
		CreditsText creditsText = go.GetComponent<CreditsText>();
		if (creditsText != null) {
			creditsText.Setup(title, name);
		}
	}

	public void ResetCredits() {
		foreach (Transform child in creditsParent) {
			Destroy(child.gameObject);
		}
		SetupCredits();
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: AUTOSCROLL

	public void RunRoutine() {
			creditsScroll.verticalNormalizedPosition = 1.0f;
		if (_creditsRoutine != null) {
			StopCoroutine(_creditsRoutine);
		}
		if (GameManager.Gui.Menu.CheckState(MenuStates.Credits))  {
			_creditsRoutine = StartCoroutine(CreditsAutoscroll());
		}
	}

	IEnumerator CreditsAutoscroll() {
		float startup = GameManager.Config.UI.CreditsStartup;
		while(true) {
			if (!_creditsPause) {
				creditsScroll.verticalNormalizedPosition -= (GameManager.Config.UI.CreditsSpeed + startup) * Time.smoothDeltaTime;
				startup = Mathf.Clamp(startup -= 1.0f * Time.deltaTime, 0.0f, GameManager.Config.UI.CreditsStartup);
			}
			if (creditsScroll.verticalNormalizedPosition <= 0.0f) {
				creditsScroll.verticalNormalizedPosition = 1.0f;
			}
			else if (creditsScroll.verticalNormalizedPosition >= 1.0f) {
				creditsScroll.verticalNormalizedPosition = 0.0f;
			}
			yield return null;
		}
	}

	public void CreditsMove(bool up) {
		if(GameManager.GameState == GameStates.Menu && creditsPanel.gameObject.activeSelf) {
			float speed = GameManager.Config.UI.CreditsSpeed * 10.0f * Time.deltaTime;
			if (up) {
				creditsScroll.verticalNormalizedPosition -= speed;
			}
			else {
				creditsScroll.verticalNormalizedPosition += speed;
			}
		}
	}

	public void CreditsPause(bool pause) {
		_creditsPause = pause;
	}
}
}