using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SmallTown {
public class SettingsButton : MonoBehaviour, ISetting {

	// Objects
	[SerializeField] private TextMeshProUGUI title;
	[SerializeField] private Button button;

	// Variables
	private string key = "";
	private bool setup = false;


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup(string newKey, MinMax value) {
		setup = true;
		title.SetText(Localize.GetText((newKey)));
		key = newKey;
		// dropdown.value = (int)value.current;
		setup = false;
	}

	public void OnClick() {
		if (!setup && key != "") {
			GameManager.Settings.ChangeOption(key, 0.0f);
		}
	}
}
}