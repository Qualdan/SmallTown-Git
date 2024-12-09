using UnityEngine;
using UnityEngine.EventSystems;

namespace SmallTown {
public class MessageTrigger : MonoBehaviour {

	// Private variables
	[SerializeField] private string key = "";


	// ************************************************************************************************************ UNITY FUNCTIONS

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			GameManager.Gui.Hud.ShowMessage(Localize.GetText(key));
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			GameManager.Gui.Hud.HideMessage();
		}
	}
}
}