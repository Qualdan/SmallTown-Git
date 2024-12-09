using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using TMPro;
using UnityEngine.Localization.Components;

namespace SmallTown {
public class SettingsMenuButton : MonoBehaviour {

	// Objects
	[SerializeField] private LocalizeStringEvent stringEvent;

	// Variables
	private string key = "";


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup(string newKey) {
		key = newKey;

		LocalizedString localizedString = new LocalizedString();
		localizedString.TableReference = "LocalizedStrings";
		localizedString.TableEntryReference = newKey;
		stringEvent.StringReference = localizedString;
	}

	public void OnClick() {
		if (key != "") {
			GameManager.Settings.HeadlineClicked(key);
		}
	}
}
}