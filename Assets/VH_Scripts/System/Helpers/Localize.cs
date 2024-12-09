using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace SmallTown {
public class Localize {

	// Variables
	private static StringTable _localizationTable;
	private static Dictionary<string, string> _localizedLanguages;


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public static IEnumerator Setup() {
		yield return LocalizationSettings.InitializationOperation;
		var locale = LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier("en"));
		var tableOp = LocalizationSettings.StringDatabase.GetTableAsync("LocalizedStrings", locale);
		yield return tableOp;
		_localizationTable = tableOp.Result;
		LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
		LocalizationSettings.SelectedLocale = locale;

		_localizedLanguages = new Dictionary<string, string>() {
			{ "en", "English" },
			{ "fi", "Suomi" },
		};

		Helpers.Log("Localize", "Localization tables loaded", _localizationTable.ToString());
	}

	public static void OnLocaleChanged(Locale locale) {
		Helpers.Log("Localize", "Locale changed tables loaded", locale.Identifier.Code);
		LoadTable(locale);
	}

	public static void LoadTable(Locale locale) {
		LocalizationSettings.StringDatabase.GetTableAsync("LocalizedStrings", locale).Completed += op => {
			_localizationTable = op.Result;
			GameManager.OnLocaleChanged?.Invoke();
		};
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: GET TEXT

	public static string GetText(string key, bool returnSame = false, params object[] args)
	{
		if (string.IsNullOrEmpty(key)) return null;

		var entry = _localizationTable.GetEntry(key);
		if (entry != null && !string.IsNullOrEmpty(entry.LocalizedValue)) {
			if (args != null && args.Length > 0) {
				return entry.GetLocalizedString(args);
			}
			return entry.GetLocalizedString();
		}
		if (returnSame) {
			return key;
		}
		return $"<color=#ff4040>!LOC!</color> {key}";
	}

	public static string GetLanguageName(string code) {
		if (_localizedLanguages.ContainsKey(code)) {
			return _localizedLanguages[code];
		}
		return null;
	}

	public static string GetLanguageCode(string language) {
		List<Locale> locales = LocalizationSettings.AvailableLocales.Locales;
		for (int i = 0; i < locales.Count; i++) {
			if (Localize.GetLanguageName(locales[i].Identifier.Code) == language || locales[i].Identifier.Code == language) {
				return locales[i].Identifier.Code;
			}
		}
		return null;
	}

	public static Locale GetLocale(string language) {
		List<Locale> locales = LocalizationSettings.AvailableLocales.Locales;
		for (int i = 0; i < locales.Count; i++) {
			if (Localize.GetLanguageName(locales[i].Identifier.Code) == language || locales[i].Identifier.Code == language) {
				return locales[i];
			}
		}
		return null;
	}
}
}