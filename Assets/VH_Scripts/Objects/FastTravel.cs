using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTown {
public class FastTravel : MonoBehaviour {

	// Private variables
	[SerializeField] private Transform playerSpawner;
	[SerializeField] private Transform carSpawner;
	[SerializeField] private string locationKey;

	// Public variables
	public Transform PlayerPosition { get { return playerSpawner; } }
	public Transform CarPosition { get { return carSpawner; } }
	public string Location { get { return locationKey; } }

}
}