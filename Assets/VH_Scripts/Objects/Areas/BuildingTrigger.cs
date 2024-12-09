using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace SmallTown {
public class BuildingTrigger : MonoBehaviour {

	[Header("Building")]
	[SerializeField] private BuildingController controller;
	[SerializeField] private BoxCollider boxCollider;
	[SerializeField] private int floor = 0;

	[Header("Localization")]
	[SerializeField] private string primaryKey = "";
	[SerializeField] private string secondaryKey = "";


	// ************************************************************************************************************ UNITY FUNCTIONS

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			PlayerEnter();
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			PlayerExit();
		}
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void PlayerEnter() {
		controller.Entered(floor);
		GameManager.Area.Location.Entered(primaryKey, secondaryKey);
	}

	public void PlayerExit() {
		controller.Exited(floor);
		GameManager.Area.Location.Exited(primaryKey, secondaryKey);
	}

	public void Toggle(bool toggle) {
		boxCollider.enabled = toggle;
	}
}
}