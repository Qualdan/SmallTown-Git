using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SmallTown {
public class SaveManager : MonoBehaviour, IManager {

	// Variables
	// private List<SaveFile> _saves = new List<SaveFile>();
	[SerializeField] private SaveFile _current;

	// Public variables
	public bool Initialized { get; private set; }
	public SaveFile Current { get { return _current; } }


	void Update() {
		if(Input.GetKeyDown(KeyCode.F5)) {
			SaveGame();
		}
		if(Input.GetKeyDown(KeyCode.F9)) {
			LoadGame();
		}
	}


	// ************************************************************************************************************ UNITY FUNCTIONS

	public void Setup() {

		_current = new SaveFile();

		// LoadGame();


		Initialized = true;
		Helpers.Initialized(GetType().Name);
	}



	// Initial load (no save files found): Random.seed = (int)System.DateTime.Now.Ticks;


	public void SaveGame() {

		if(_current == null) {
			Debug.LogWarning("Current save is null");
		}

		SerializeObject(_current.CreateSave(), "savefile");
	}

	static void SerializeObject(object target, string file) {
		BinaryFormatter formatter = new BinaryFormatter();
		string directory = Application.persistentDataPath + "/SAVEGAME/";
		string fileName = file + ".sav";
		if(!Directory.Exists(directory)) {
			Directory.CreateDirectory(directory);
		}
		FileStream stream = new FileStream(directory + fileName, FileMode.Create);
		formatter.Serialize(stream, target);
		stream.Close();
	}





	public void LoadGame() {

		_current = (SaveFile)DeserializeObject("savefile");

		// UnityEngine.Random.InitState(_current.seed);

		// if (_current != null) {
		// 	GameManager.Gui.Inventory.LoadGame(_current);
		// }
	}

	static object DeserializeObject(string file) {
		string directory = Application.persistentDataPath + "/SAVEGAME/";
		string fileName = file + ".sav";
		if (File.Exists(directory + fileName)) {
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(directory + fileName, FileMode.Open);
			object target = formatter.Deserialize(stream);
			stream.Close();
			return target;
		}
		return null;
	}



}
}