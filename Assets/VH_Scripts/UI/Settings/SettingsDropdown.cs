using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using TMPro;

namespace SmallTown {
public class SettingsDropdown : MonoBehaviour, ISetting {

	// Objects
	[SerializeField] private TextMeshProUGUI title;
	[SerializeField] private TMP_Dropdown dropdown;
	[SerializeField] private GameObject applyButton;

	// Variables
	private string _key = "";
	private string _current = "";
	private bool _setup = false;


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup(string newKey, MinMax value) {
		_setup = true;
		title.SetText(Localize.GetText((newKey)));
		_key = newKey;

		if (_key.ToLower().Contains("language")) {
			dropdown.ClearOptions();

			List<string> languages = new List<string>();
			List<Locale> locales = LocalizationSettings.AvailableLocales.Locales;
			for (int i = 0; i < locales.Count; i++) {
				languages.Add(Localize.GetLanguageName(locales[i].Identifier.Code));
			}
			dropdown.AddOptions(languages);
			dropdown.value = GameManager.Settings.LanguageIndex();
		}

		else if (_key.ToLower().Contains("resolution")) {
			dropdown.ClearOptions();
			List<string> resolutions = new List<string>();
			string resText = "";
			foreach (Resolution res in Screen.resolutions) {
				resText = string.Format(res.width + "x" + res.height);
				if (!resolutions.Contains(resText)) {
					resolutions.Add(resText);
				}
			}
			dropdown.AddOptions(resolutions);
			dropdown.value = GameManager.Settings.ResolutionIndex();
		}

		_setup = false;
	}

	public void SetValue(int index) {
		_setup = true;
		dropdown.value = index;
		_setup = false;
	}

	public void OnValueChanged(int value) {
		if (!_setup && _key != "") {
			_current = dropdown.options[value].text;
			applyButton.SetActive(Localize.GetLocale(_current) != LocalizationSettings.SelectedLocale);
			GameManager.Settings.ChangeOption(_key, (float)value);
		}
	}

	public void OnApplyButton() {
		applyButton.SetActive(false);
		GameManager.Settings.DropdownApply(_key, dropdown.value, _current);
	}
}
}