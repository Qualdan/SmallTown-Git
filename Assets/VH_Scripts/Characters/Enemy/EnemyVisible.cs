using UnityEngine;

namespace SmallTown {
public class EnemyVisible : MonoBehaviour {

	// Private variables
	[SerializeField] private EnemyController controller;


	// ************************************************************************************************************ UNITY FUNCTIONS

	void OnBecameVisible() {
		controller.Visible(true);
	}

	void OnBecameInvisible() {
		controller.Visible(false);
	}
}
}