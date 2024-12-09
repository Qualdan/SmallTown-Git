using UnityEngine;
using UnityEngine.EventSystems;

namespace SmallTown {
public class LocationTrigger : MonoBehaviour {

	// Private variables
	[SerializeField] private string flag = "";
	[SerializeField] private string primary = "";
	[SerializeField] private string secondary = "";


	// ************************************************************************************************************ UNITY FUNCTIONS

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			GameManager.Area.Location.Triggered(flag, true);
			if(!string.IsNullOrEmpty(primary)) {
				GameManager.Area.Location.Entered(primary, secondary);
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			GameManager.Area.Location.Triggered(flag, false);
			if(!string.IsNullOrEmpty(primary)) {
				GameManager.Area.Location.Exited(primary, secondary);
			}
		}
	}
}
}