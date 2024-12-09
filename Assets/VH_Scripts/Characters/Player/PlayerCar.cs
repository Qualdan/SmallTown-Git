using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SmallTown {
public class PlayerCar : MonoBehaviour {

	// Public variables
	public bool Initialized { get; private set; }


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {

		Initialized = true;
	}


	public void FastTravel(Transform target) {
		if(target != null) {
			transform.position = target.position;
			transform.rotation = target.rotation;
		}

		// Play sound of car door closing, then fade in
		// 		OR
		// Fade in, then show animation of the door closing
	}
}
}