using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTown {
public class LevelManager : MonoBehaviour {

	// Variables
	[SerializeField] private ItemManager _itemManager;
	[SerializeField] private WeatherManager _weatherManager;


	// Post processing effect for the area? Does it need settings?


	// Public variables
	public ItemManager Item { get { return _itemManager; } }
	public WeatherManager Weather { get { return _weatherManager; } }
	public bool Initialized { get; private set; }


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {
		_itemManager.Setup();
		_weatherManager.Setup();

		Initialized = true;
		Helpers.Initialized(GetType().Name);
	}
}
}