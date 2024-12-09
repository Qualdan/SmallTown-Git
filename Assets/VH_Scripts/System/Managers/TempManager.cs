using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTown {
public class TempManager : MonoBehaviour, IManager {

	// Variables
	// [SerializeField] ItemType item;

	// Public variables
	public bool Initialized { get; private set; }


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {


		Initialized = true;
		Helpers.Initialized(GetType().Name);
	}
}
}