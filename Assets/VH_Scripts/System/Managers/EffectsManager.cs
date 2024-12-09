using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;
// using MoreMountains.Feedbacks;
// using Knife.PostProcessing;
using SCPE;

namespace SmallTown {
public class EffectsManager : MonoBehaviour, IManager {

	// Variables
	[SerializeField] GameObject postProcessing;
	[SerializeField] private PostProcessVolume mainVolume;
	[SerializeField] private PostProcessVolume pauseVolume;
	[SerializeField] private PostProcessVolume henrisRoomVolume;
	[SerializeField] private PostProcessVolume hostileLocationVolume;

	// Effects
	private Kuwahara kuwahara;
	// private Vignette vignette;
	// private float vignetteValue;
	// private ColorGrading colorGrading;
	// private SCPE.Fog fog;

	// // Public variables
	// public AnimationCurve FogValue;
	// public AnimationCurve SaturationValue;
	// public AnimationCurve ContrastValue;


	// Public variables
	public bool Initialized { get; private set; }


	// void Update() {
	// 	if(!Initialized) return;
		// TimeEffects();
	// }


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {
		mainVolume.profile.TryGetSettings(out kuwahara);
		// playerVolume.profile.TryGetSettings(out vignette);
		// playerVolume.profile.TryGetSettings(out colorGrading);
		// playerVolume.profile.TryGetSettings(out fog);

		GameManager.OnGameStateChanged += OnGameStateChanged;
		GameManager.OnPauseChanged += OnPauseChanged;

		postProcessing.SetActive(true);

		Initialized = true;
		Helpers.Initialized(GetType().Name);
	}

	void OnGameStateChanged(GameStates value) {
	}

	void OnPauseChanged(bool toggle) {
		pauseVolume.DOComplete();
		DOTween.To(() => pauseVolume.weight, x => pauseVolume.weight = x, toggle ? 1.0f : 0.0f, GameManager.Config.General.PauseSpeed);
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: SWITCH PROFILES

	public void ZoomEffects(bool toggle) {
		if (toggle) {
			kuwahara.radius.value = 4;
		}
		else {
			kuwahara.radius.value = 2;
		}
	}

	public void HenrisRoom(bool toggle) {
		DOTween.To(() => henrisRoomVolume.weight, x => henrisRoomVolume.weight = x, toggle ? 1.0f : 0.0f, GameManager.Config.Effects.HenrisRoomDuration);
	}

	public void HostileLocation() {
		DOTween.To(() => hostileLocationVolume.weight, x => hostileLocationVolume.weight = x, GameManager.Area.Location.Hostile ? 1.0f : 0.0f, GameManager.Config.Effects.HostileAreaDuration);
	}





	// void TimeEffects() {
		// var saturation = colorGrading.saturation;
		// saturation.value = SaturationValue.Evaluate(RemapTime(0.0f, 24.0f));

		// var contrast = colorGrading.contrast;
		// contrast.value = ContrastValue.Evaluate(RemapTime(0.0f, 24.0f));

		// var density = fog.globalDensity;
		// density.value = FogValue.Evaluate(RemapTime(0.0f, 24.0f));

		// Debug.LogWarning("Fog density is " + density.value);
	// }

	// float RemapTime(float from, float to) {
	// 	return Helpers.Remap(CalculateTicks(), 0.0f, 86400.0f, from, to);
	// }

	// float CalculateTicks() {
	// 	return (int)(new System.TimeSpan(GameManager.Weather.Unistorm.Hour, GameManager.Weather.Unistorm.Minute, 0)).TotalSeconds;
	// }
}
}