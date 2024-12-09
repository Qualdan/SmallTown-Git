using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UniStorm;

namespace SmallTown {
public class WeatherManager : MonoBehaviour, IManager {

	// Variables
	// [SerializeField] private List<MeshRenderer> cloudPlanes = new List<MeshRenderer>();
	// private List<Vector2> cloudOffsets = new List<Vector2>();
	// [SerializeField] private UniStormSystem unistorm;
	// public UniStormSystem Unistorm { get { return unistorm; } }

	// Public variables
	public bool Initialized { get; private set; }



	// void Update() {
	// 	MoveClouds();
	// }


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {
		// GameManager.OnPauseChanged += TogglePauseMenu;

		// for (int i = 0; i < cloudPlanes.Count; i++) {
		// 	cloudOffsets.Add(Vector2.zero);
		// }

		Initialized = true;
		Helpers.Initialized(GetType().Name);
	}

	// void TogglePauseMenu(bool value) {
		// unistorm.TimeFlow = value ? UniStormSystem.EnableFeature.Disabled : UniStormSystem.EnableFeature.Enabled;
	// }


	// ************************************************************************************************************ CUSTOM FUNCTIONS: UNISTORM

	// public void SetupPlayer() {
		// unistorm.PlayerTransform = GameManager.Player.Controller.Form;
		// unistorm.PlayerCamera = GameManager.Player.Controller.Camera.MainCamera;
		// unistorm.enabled = true;
		// unistorm.gameObject.SetActive(true);
	// }


	// void MoveClouds() {
	// 	if (!Initialized) return;

	// 	for (int i = 0; i < cloudOffsets.Count; i++) {
	// 		float x = cloudOffsets[i].x + ((i + 1) * 0.0005f * Time.deltaTime);
	// 		float y = cloudOffsets[i].y + ((i + 1) * 0.001f * Time.deltaTime);
	// 		cloudOffsets[i] = new Vector2(x, y);
	// 	}

	// 	for (int i = 0; i < cloudPlanes.Count; i++) {
	// 		cloudPlanes[i].material.SetTextureOffset("_MainTex", cloudOffsets[i]);
	// 	}
	// }


}
}