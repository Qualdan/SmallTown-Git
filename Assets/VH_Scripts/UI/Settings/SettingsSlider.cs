using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SmallTown {
public class SettingsSlider : MonoBehaviour, ISetting {

	// Objects
	[SerializeField] private TextMeshProUGUI title;
	[SerializeField] private Slider slider;

	// Variables
	public string key = "";
	private bool setup = false;


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup(string newKey, MinMax value) {
		setup = true;
		title.SetText(Localize.GetText(newKey));
		key = newKey;
		slider.minValue = value.Minimum;
		slider.maxValue = value.Maximum;
		slider.value = value.Current;
		setup = false;
		UpdateText(value.Current);
	}

	void UpdateText(float value) {
		if(key.Contains("Difficulty")) {
			title.SetText(Localize.GetText(key) + " <color=#4B8BB9>" + Localize.GetText("Settings/Difficulty" + value.ToString()) + "</color>");
		}
		else {
			title.SetText(Localize.GetText(key) + " <color=#4B8BB9>" + Mathf.RoundToInt(value).ToString() + "</color>");
		}
	}

	public void OnValueChanged(float value) {
		UpdateText(value);
		if (!setup && key != "") {
			GameManager.Settings.ChangeOption(key, value);
		}
	}
}
}