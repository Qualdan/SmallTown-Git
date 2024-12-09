using System.Collections;
using System.Collections.Generic;
using Knife.PostProcessing;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SmallTown {
public class PickableItem : MonoBehaviour, IActivate { //, IPointerEnterHandler, IPointerExitHandler {

	// Private variables
	[SerializeField] private List<OutlineRegister> outlines = new List<OutlineRegister>();
	[SerializeField] private string tooltipTitle;
	[SerializeField] private string tooltipDetails;


	// ************************************************************************************************************ UNITY FUNCTIONS

	// public void OnPointerEnter(PointerEventData eventData) {
	// 	Debug.LogWarning("Pickable item entered");

	// 	Highlight(true);
	// 	GameManager.Gui.Hud.ShowTooltip(tooltipTitle, tooltipDetails, transform.position);
	// }

	// public void OnPointerExit(PointerEventData eventData) {
	// 	Debug.LogWarning("Pickable item exited");
	// 	Highlight(false);
	// 	GameManager.Gui.Hud.HideTooltip();
	// }


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Highlight(bool toggle) {
		for (int i = 0; i < outlines.Count; i++) {
			outlines[i].enabled = toggle;
		}
	}

	public void Activate() {

	}

}
}