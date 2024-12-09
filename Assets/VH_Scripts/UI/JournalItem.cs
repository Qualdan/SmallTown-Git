using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SmallTown {
public class JournalItem : MonoBehaviour {

	// Private variables
	[SerializeField] private TextMeshProUGUI titleText;
	[SerializeField] private Image iconImage;
	[SerializeField] private MultiImageButton button;

	// Public variables
	public MultiImageButton Button { get { return button; } }


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup(string title, Sprite icon, bool buttonActive) {
		titleText.SetText(title);
		iconImage.sprite = icon;
		button.interactable = buttonActive;
	}
}
}