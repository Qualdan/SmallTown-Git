using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SmallTown {
public class AreaManager : MonoBehaviour, IManager {

	// Variables
	[SerializeField] private LocationManager locationManager;
	private LevelManager _levelManager;
	private List<FastTravel> fastTravelPoints = new List<FastTravel>();
	private Coroutine _fastTravelRoutine;
	private bool _dayTime = false;
	private string _currentTime = "";

	// Public variables
	public LevelManager Level { get { return _levelManager; } }
	public LocationManager Location { get { return locationManager; } }
	public bool Initialized { get; private set; }
	public Action OnBeforeAreaChanged;
	public Action OnAfterAreaChanged;


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {
		locationManager.Setup();

		GatherLevelManager();
		GatherFastTravelPoints();

		Initialized = true;
		Helpers.Initialized(GetType().Name);
	}

	void GatherLevelManager() {
		_levelManager = FindObjectOfType<LevelManager>();
		if(_levelManager != null) {
			_levelManager.Setup();
		}
		else {
			Debug.LogError("LevelManager is not found! All levels should have LevelManager!");
		}
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: FAST TRAVEL

	void GatherFastTravelPoints(string areaKey = "") {
		fastTravelPoints.Clear();
		GameObject[] fastTravels = GameObject.FindGameObjectsWithTag("FastTravel");
		foreach (GameObject go in fastTravels) {
			FastTravel ft = go.GetComponent<FastTravel>();
			if(ft != null) {
				fastTravelPoints.Add(ft);
			}
		}

		#if UNITY_EDITOR
		if(!string.IsNullOrEmpty(areaKey)) {
			string missingFastTravelPoints = "";
			for (int i = 0; i < GameManager.Config.Area.FastTravelAreas.Count; i++) {
				if(GameManager.Config.Area.FastTravelAreas[i].AreaKey == areaKey) {
					for (int f = 0; f < GameManager.Config.Area.FastTravelAreas[i].FastTravelPoints.Count; f++) {
						bool found = false;
						for (int p = 0; p < fastTravelPoints.Count; p++) {
							if(fastTravelPoints[p].Location == GameManager.Config.Area.FastTravelAreas[i].FastTravelPoints[f]) {
								found = true;
								break;
							}
						}
						if(!found) {
							if(!string.IsNullOrEmpty(missingFastTravelPoints)) {
								missingFastTravelPoints += ", ";
							}
							missingFastTravelPoints += GameManager.Config.Area.FastTravelAreas[i].FastTravelPoints[f];
						}
					}
					break;
				}
			}
			if(!string.IsNullOrEmpty(missingFastTravelPoints)) {
				Debug.LogWarning("Scene is missing fast travel points: " + missingFastTravelPoints);
			}
			missingFastTravelPoints = "";
			for (int p = 0; p < fastTravelPoints.Count; p++) {
				for (int i = 0; i < GameManager.Config.Area.FastTravelAreas.Count; i++) {
					if(GameManager.Config.Area.FastTravelAreas[i].AreaKey == areaKey) {
						bool found = false;
						if(GameManager.Config.Area.FastTravelAreas[i].FastTravelPoints.Contains(fastTravelPoints[p].Location)) {
							found = true;
						}
						if(!found) {
							if(!string.IsNullOrEmpty(missingFastTravelPoints)) {
								missingFastTravelPoints += ", ";
							}
							missingFastTravelPoints += fastTravelPoints[p].Location;
						}
						break;
					}
				}
			}
			if(!string.IsNullOrEmpty(missingFastTravelPoints)) {
				Debug.LogWarning("Scene has extra fast travel points: " + missingFastTravelPoints);
			}
		}
		#endif
	}

	Transform FastTravelPoint(string locationKey, bool car = false) {
		for (int i = 0; i < fastTravelPoints.Count; i++) {
			if(fastTravelPoints[i].Location == locationKey) {
				return car ? fastTravelPoints[i].CarPosition : fastTravelPoints[i].PlayerPosition;
			}
		}
		return null;
	}

	public void FastTravel(string areaKey, string fastTravelKey, bool dayTime) {
		GameManager.GameState = GameStates.Loading;
		GameManager.Gui.Hud.Loader(true, Localize.GetText(areaKey), Localize.GetText(fastTravelKey) + ", " + GetTime(dayTime));
		string sceneName = ConvertScene(areaKey);
		if(!string.IsNullOrEmpty(sceneName)) {
			if(SceneManager.GetActiveScene().name == sceneName) {
				Helpers.Log("AreaManager", "Fast Travel in " + areaKey + " ('" + sceneName + "')", fastTravelKey);
				if(_fastTravelRoutine != null) {
					StopCoroutine(_fastTravelRoutine);
				}
				_fastTravelRoutine = StartCoroutine(FastTravelRoutine(fastTravelKey, dayTime));
			}
			else {
				Helpers.Log("AreaManager", "Area change to " + areaKey + " ('" + sceneName + "')", fastTravelKey);
				if(_fastTravelRoutine != null) {
					StopCoroutine(_fastTravelRoutine);
				}
				_fastTravelRoutine = StartCoroutine(LoadAreaRoutine(sceneName, areaKey, fastTravelKey, dayTime));
			}
		}
	}

	string ConvertScene(string areaKey) {
		for (int i = 0; i < GameManager.Config.Area.AreaScenes.Count; i++) {
			if(GameManager.Config.Area.AreaScenes[i].AreaKey == areaKey) {
				return GameManager.Config.Area.AreaScenes[i].AreaName;
			}
		}
		return "";
	}

	IEnumerator FastTravelRoutine(string fastTravelKey, bool dayTime) {
		GameManager.Player.FastTravel(FastTravelPoint(fastTravelKey), FastTravelPoint(fastTravelKey, true));
		ChangeTime(dayTime);
		yield return new WaitForSeconds(GameManager.Config.Area.FastTravelDelay);
		GameManager.GameState = GameStates.Player;
		GameManager.Gui.Hud.Loader(false);
	}

	IEnumerator LoadAreaRoutine(string sceneName, string areaKey, string fastTravelKey, bool dayTime) {
		OnBeforeAreaChanged?.Invoke();
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
		while (!asyncLoad.isDone) {
			GameManager.Gui.Hud.LoaderProgress(asyncLoad.progress);
			yield return null;
		}
		GatherFastTravelPoints(areaKey);
		ChangeTime(dayTime);
		GameManager.Player.FastTravel(FastTravelPoint(fastTravelKey), FastTravelPoint(fastTravelKey, true));
		yield return new WaitForSeconds(GameManager.Config.Area.FastTravelDelay);
		GameManager.GameState = GameStates.Player;
		GatherLevelManager();
		OnAfterAreaChanged?.Invoke();
		GameManager.Gui.Hud.Loader(false);
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: TIME

	string GetTime(bool dayTime) {
		if(_dayTime != dayTime) {
			int hour = dayTime ? UnityEngine.Random.Range(10, 15) : Helpers.Random(0.5f) ? UnityEngine.Random.Range(22, 24) : UnityEngine.Random.Range(0, 3);
			int minute = UnityEngine.Random.Range(0, 60);
			_currentTime = (hour < 10 ? "0" + hour.ToString() : hour.ToString()) + ":" + (minute < 10 ? "0" + minute.ToString() : minute.ToString());
			_dayTime = dayTime;
		}
		return _currentTime;
	}

	void ChangeTime(bool dayTime) {
		if(dayTime) {
			// Debug.LogWarning("Should be day");
			// Change time to day
		}
		else {
			// Debug.LogWarning("Should be night");
			// Change time to night
		}
	}
}
}