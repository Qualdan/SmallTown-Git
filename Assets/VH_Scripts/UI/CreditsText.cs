using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SmallTown {
public class CreditsText : MonoBehaviour {

	// Objects
	[SerializeField] private TextMeshProUGUI titleText;
	[SerializeField] private TextMeshProUGUI nameText;


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup(string title, string name) {
		titleText.SetText(Localize.GetText(title, true));
		if(nameText != null) {
			nameText.SetText(name);
		}
	}
}
}