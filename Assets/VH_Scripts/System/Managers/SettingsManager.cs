using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;
using DG.Tweening;

namespace SmallTown {
public class SettingsManager : MonoBehaviour, IManager {

	// Private variables
	[SerializeField] private SettingsData settingsData;

	[Header("Objects")]
	[SerializeField] private Transform content;
	[SerializeField] private CanvasGroup contentGroup;
	[SerializeField] private RectTransform buttonsParent;
	[SerializeField] private RectTransform contentParent;
	[SerializeField] private ScrollRect contentScroll;
	[SerializeField] private GameObject confirmPanel;
	[SerializeField] private TextMeshProUGUI confirmText;

	[Header("Prefabs")]
	[SerializeField] private GameObject menuPrefab;
	[SerializeField] private GameObject categoryPrefab;
	[SerializeField] private GameObject separatorPrefab;
	[SerializeField] private GameObject buttonPrefab;
	[SerializeField] private GameObject togglePrefab;
	[SerializeField] private GameObject dropdownPrefab;
	[SerializeField] private GameObject sliderPrefab;

	// Private variables
	private Dictionary<string, float> options = new Dictionary<string, float>();
	private Resolution _currentResolution;
	private string _currentKey = "";
	private float _resolutionTimer = 0.0f;
	private bool _resolutionApplied = false;
	private int _languageIndex = 0;
	private int _resolutionIndex = 0;
	private Vector3 contentDefaultPosition;

	// Public variables
	public Action<string, float> OnSettingChanged;
	public int CurrentDifficulty { get { return Mathf.RoundToInt(CheckOption("Settings/Difficulty")); }}
	public bool Initialized { get; private set; }


	// ************************************************************************************************************ UNITY FUNCTIONS

	public void Setup() {
		// OnSettingChanged += SettingsChanged;
		GameManager.OnLocaleChanged += ResetOptions;

		contentDefaultPosition = content.position;

		SetupEntries();

		Initialized = true;
		Helpers.Initialized(GetType().Name);
	}

	void SetupEntries() {
		SettingsEntry entry;
		for (int i = 0; i < settingsData.settings.Count; i++) {
			GenerateHeadline(settingsData.settings[i].key);
			for (int c = 0; c < settingsData.settings[i].categories.Count; c++) {
				for (int e = 0; e < settingsData.settings[i].categories[c].entries.Count; e++) {
					entry = settingsData.settings[i].categories[c].entries[e];
					entry.value = entry.initial.Current;
					options.Add(entry.key, entry.value);
				}
			}
		}
		GenerateOptions(settingsData.settings[0].key, false);
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: SETTINGS CHANGED

	// void SettingsChanged(string key, float value) {
	// 	Debug.LogWarning("Settings changed: Key is " + key + " and value is " + value);

	// 	if(key == "Settings/Difficulty") {
	// 		_currentDifficulty = Mathf.RoundToInt(value);
	// 	}
	// }


	// ************************************************************************************************************ CUSTOM FUNCTIONS: GENERATE

	void GenerateHeadline(string key) {
		GameObject go = null;
		go = Instantiate(menuPrefab, buttonsParent);
		SettingsMenuButton menu = go.GetComponent<SettingsMenuButton>();
		menu?.Setup(key);
	}

	void GenerateOptions(string key, bool showPanel = true) {
		foreach (Transform child in contentParent) {
			Destroy(child.gameObject);
		}
		GameObject go = null;
		SettingsEntry entry = null;
		for (int i = 0; i < settingsData.settings.Count; i++) {
			if (settingsData.settings[i].key == key) {
				for (int c = 0; c < settingsData.settings[i].categories.Count; c++) {
					if (c > 0) {
						go = Instantiate(separatorPrefab, contentParent);
					}
					go = Instantiate(categoryPrefab, contentParent);
					SettingsHeadlineText category = go.GetComponent<SettingsHeadlineText>();
					if (category != null)  {
						category.Setup(settingsData.settings[i].categories[c].key);
					}
					for (int e = 0; e < settingsData.settings[i].categories[c].entries.Count; e++) {
						entry = settingsData.settings[i].categories[c].entries[e];
						if (entry.type == OptionTypes.Toggle) {
							go = Instantiate(togglePrefab, contentParent);
							SettingsToggle toggle = go.GetComponent<SettingsToggle>();
							if (toggle != null)  {
								toggle.Setup(entry.key, entry.initial);
							}
						}
						else if (settingsData.settings[i].categories[c].entries[e].type == OptionTypes.Slider) {
							go = Instantiate(sliderPrefab, contentParent);
							SettingsSlider slider = go.GetComponent<SettingsSlider>();
							if (slider != null)  {
								slider.Setup(entry.key, entry.initial);
							}
						}
						else if (settingsData.settings[i].categories[c].entries[e].type == OptionTypes.Dropdown) {
							go = Instantiate(dropdownPrefab, contentParent);
							SettingsDropdown dropdown = go.GetComponent<SettingsDropdown>();
							if (dropdown != null)  {
								dropdown.Setup(entry.key, entry.initial);
							}
						}
						// else if (optionEntries[i].type == OptionTypes.Input) {
						// 	go = Instantiate(buttonPrefab, contentParent);
						// 	OptionsSlider slider = go.GetComponent<OptionsSlider>();
						// 	if (slider != null)  {
						// 		slider.Setup(optionEntries[i].key, optionEntries[i].initial);
						// 	}
						// }
					}
				}
				break;
			}
		}
		contentScroll.verticalNormalizedPosition = 1.0f;
		if(showPanel) {
			_currentKey = key;
			content.position = contentDefaultPosition;
			// content.gameObject.SetActive(true);
			contentGroup.blocksRaycasts = true;
			contentGroup.DOFade(1.0f, GameManager.Config.UI.ChangePanelDuration);
		}
	}

	public void ResetOptions() {
		HeadlineClicked(_currentKey);
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: ACTIVATE

	public void HeadlineClicked(string key) {
		if(_currentKey == key) {
			HideContent();
		}
		else {
			contentGroup.DOFade(0.0f, GameManager.Config.UI.ChangePanelDuration).OnComplete(()=> GenerateOptions(key));
		}
	}

	public void HideContent() {
		_currentKey = "";
		// content.gameObject.SetActive(false);
		contentGroup.blocksRaycasts = false;
		contentGroup.DOFade(0.0f, GameManager.Config.UI.ChangePanelDuration).OnComplete(DisableContent);
	}

	void DisableContent() {
		content.position = contentDefaultPosition;
		contentGroup.blocksRaycasts = false;
		// content.gameObject.SetActive(true);
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: OPTIONS

	public void ChangeOption(string key, float value) {
		if(options.ContainsKey(key)) {
			options[key] = value;
		}
		OnSettingChanged?.Invoke(key, value);
	}

	public float CheckOption(string key) {
		if(options.ContainsKey(key)) {
			return options[key];
		}
		return 0.0f;
	}

	public void DropdownApply(string key, int index, string value = "") {
		if (key.ToLower().Contains("language")) {
			ApplyLanguage(value, index);
		}
		else if (key.ToLower().Contains("resolution")) {
			ApplyResolution(value, index);
		}
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: LANGUAGE

	public int LanguageIndex() {
		return _languageIndex;
	}

	void ApplyLanguage(string value, int index) {
		_languageIndex = index;
		Locale locale = Localize.GetLocale(value);
		if (locale != null) {
			LocalizationSettings.SelectedLocale = locale;
		}
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: RESOLUTION

	public int ResolutionIndex() {
		return _resolutionIndex;
	}

	void ApplyResolution(string value, int index) {
		_resolutionIndex = index;

		// Check value for resolution value and check that it matches one that is available


		Screen.SetResolution(_currentResolution.width, _currentResolution.height, FullScreen());
		StartCoroutine(ConfirmPanel());
	}

	IEnumerator ConfirmPanel() {
		confirmPanel.SetActive(true);
		_resolutionTimer = 10;
		_resolutionApplied = false;
		while(_resolutionTimer > 0) {
			_resolutionTimer -= 1.0f * Time.deltaTime;
			confirmText.SetText(Localize.GetText("Options/ConfirmResolution", false, (int)_resolutionTimer));
			yield return null;
		}
		confirmPanel.SetActive(false);
		ConfirmResolution();
	}

	public void OnResolutionTimer(bool answer) {
		_resolutionApplied = answer;
		_resolutionTimer = 0.0f;
	}

	void ConfirmResolution() {
		if (_resolutionApplied) {
			_currentResolution = Screen.currentResolution;
			// Debug.LogWarning("Saved current resolution");
		}
		else {
			// Debug.LogWarning("Reverted resolution changes");
			Screen.SetResolution(_currentResolution.width, _currentResolution.height, FullScreen());
		}
	}

	FullScreenMode FullScreen() {
		return CheckOption("Video/Windowed") == 1.0f ? FullScreenMode.Windowed : FullScreenMode.FullScreenWindow;
	}
}
}