using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SmallTown {
public class SettingsToggle : MonoBehaviour, ISetting {

	// Objects
	[SerializeField] private TextMeshProUGUI title;
	[SerializeField] private Toggle toggle;

	// Variables
	private string key = "";
	private bool setup = false;


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup(string newKey, MinMax value) {
		setup = true;
		title.SetText(Localize.GetText((newKey)));
		key = newKey;
		toggle.isOn = value.Current == 1.0f ? true : false;
		setup = false;
	}

	public void OnValueChanged(bool value) {
		if (!setup && key != "") {
			GameManager.Settings.ChangeOption(key, value ? 1 : 0);
		}
	}

	public void LabelClick() {
		toggle.isOn = !toggle.isOn;
	}
}
}