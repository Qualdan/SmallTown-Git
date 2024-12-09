using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SmallTown {
public class SettingsHeadlineText : MonoBehaviour {

	// Objects
	[SerializeField] private TextMeshProUGUI title;


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup(string newKey) {
		title.SetText(Localize.GetText((newKey)));
	}
}
}