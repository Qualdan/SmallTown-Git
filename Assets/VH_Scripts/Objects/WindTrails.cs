using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTown {
public class WindTrails : MonoBehaviour {

	// Private variables
	[SerializeField] private ParticleSystem particles;
	private ParticleSystem.EmissionModule _emission;


	// ************************************************************************************************************ UNITY FUNCTIONS

	void OnTriggerEnter(Collider collider) {
		if (collider.CompareTag("Player")) {
			ToggleTrails(true);
		}
	}

	void OnTriggerExit(Collider collider) {
		if (collider.CompareTag("Player")) {
			ToggleTrails(false);
		}
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	void ToggleTrails(bool toggle) {
		_emission = particles.emission;
		_emission.rateOverTime = toggle ? 4.0f : 0.0f;
	}
}
}