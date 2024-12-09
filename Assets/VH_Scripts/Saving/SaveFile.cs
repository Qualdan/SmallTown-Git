using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTown {
[System.Serializable]
public class SaveFile {

	// Variables
	public string Profile;
	public string Time;
	public int Seed;
	// public List<SaveClothes> Clothes;
	// public List<SaveItem> Items;



	// ************************************************************************************************************ CONSTRUCTOR

	public SaveFile() {
		// Debug.LogWarning("Creating a new save");
		Defaults();
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Defaults() {
		Profile = "Save";
		Seed = 0;
		Time = System.DateTime.Now.ToString();
	// 	Clothes = new List<SaveClothes>();
	// 	Items = new List<SaveItem>();
	}

	// public void Setup(SaveFile newSave) {

	// 	// Player

	// }



	// ************************************************************************************************************ CUSTOM FUNCTIONS: SAVE

	public SaveFile CreateSave() {

		Debug.LogWarning("Creating a save file");



		// Items = GameManager.Gui.Inventory.SaveItems();

		return this;
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: LOAD

	// public void Load(SaveFile newSave) {
	// 	if (newSave != null) {
	// 		Setup(newSave);
	// 	}
	// 	else {
	// 		Defaults();
	// 		Debug.LogError("No save file was found");
	// 	}
	// }
}
}