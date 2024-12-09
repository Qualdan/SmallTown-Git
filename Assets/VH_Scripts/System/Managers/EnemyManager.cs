using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTown {
public class EnemyManager : MonoBehaviour, IManager {

	// Variables
	// [SerializeField] ItemType item;

	private List<EnemyController> enemies = new List<EnemyController>();

	// Public variables
	public bool Initialized { get; private set; }




	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {
		enemies.AddRange(FindObjectsOfType<EnemyController>());
		for (int i = 0; i < enemies.Count; i++) {
			enemies[i].Setup();
		}

		Initialized = true;
		Helpers.Initialized(GetType().Name);
	}


	public void TurnHostile(EnemyFactions faction) {
		for (int i = 0; i < enemies.Count; i++) {
			if(enemies[i].Data.Faction == faction) {
				enemies[i].SetHostile();
			}
		}
	}



	// public void SpawnEnemies(List<Transform> spawnpoints) {

	// 	// Spawn enemies randomly to the spawnpoints

	// 	// Not all spawnpoints get enemies

	// 	// Enemy spawn rate depends on difficulty settings


	// 	for (int i = 0; i < spawnpoints.Count; i++) {
	// 		if(Random.Range(0.0f, 1.0f) < 0.5f) {
	// 			// Spawn enemy
	// 		}
	// 	}



	// }



}
}