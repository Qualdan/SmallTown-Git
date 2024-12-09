using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTown {
public class MediaManager : MonoBehaviour, IManager {

	// Variables
	[SerializeField] AudioSource mainMenu;
	//[SerializeField] AudioSource mainMenu;

	// Public variables
	public bool Initialized { get; private set; }


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {
		GameManager.OnGameStateChanged += OnGameStateChanged;

		Initialized = true;
		Helpers.Initialized(GetType().Name);
	}



	void OnGameStateChanged(GameStates value) {
		if (value == GameStates.Menu) {
			PlayMenu();
		}
		else {
			StopMusic();
		}
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: MUSIC

	// public void FadeMusic(AudioSource a, AudioSource b) {
	// 	a.Play();
	// }

	public void PlayMenu() {
		StopMusic();
		mainMenu.Play();
	}


	void StopMusic() {
		mainMenu.Stop();
	}


}
}