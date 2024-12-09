using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SmallTown {
public class ItemHighlighter : MonoBehaviour {

	// Private variables
	[SerializeField] private ParticleSystem particles;
	private ParticleSystem.EmissionModule _emission;
	private int _togglers = 0;


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {
		_emission = particles.emission;
	}

	public void Toggle(bool toggle) {
		if(toggle) {
			_togglers++;
			bool allow = true;
			if(GameManager.Difficulty.Level == Difficulties.Medium) {
				allow = Helpers.CanSee(transform.position);
			}
			else if(GameManager.Difficulty.Level == Difficulties.Hard) {
				allow = false;
			}
			if(allow) {
				_emission.rateOverTime = 25.0f;
			}
		}
		else {
			_togglers--;
			if(_togglers < 1) {
				_emission.rateOverTime = 0.0f;
			}
		}
	}
}
}