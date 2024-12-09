using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SmallTown {
public class LocationManager : MonoBehaviour, IManager {

	// Variables
	private List<LocationKeys> _locationKeys = new List<LocationKeys>();
	private bool _henrisRoom = false;
	private int _hostileLocation = 0;

	// Public variables
	public bool Hostile { get { return _hostileLocation > 0; } }
	public bool Initialized { get; private set; }
	public Action OnLocationChanged;


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {
		GameManager.Area.OnAfterAreaChanged += OnAfterAreaChanged;

		Initialized = true;
		Helpers.Initialized(GetType().Name);
	}

	void OnAfterAreaChanged() {
		_henrisRoom = false;
		_hostileLocation = 0;
		_locationKeys.Clear();
		OnLocationChanged?.Invoke();
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: AREAS

	public void Entered(string primaryKey, string secondaryKey = "") {
		_locationKeys.Insert(0, new LocationKeys(primaryKey, secondaryKey));
		OnLocationChanged?.Invoke();
	}

	public void Exited(string primaryKey, string secondaryKey = "") {
		int index = Contains(primaryKey, secondaryKey);
		if (index > -1) {
			_locationKeys.RemoveAt(Contains(primaryKey, secondaryKey));
			OnLocationChanged?.Invoke();
		}
	}

	public void Triggered(string flag, bool toggle) {
		switch (flag) {
			case "Hostile":
				_hostileLocation += toggle ? 1 : -1;
				GameManager.Effects.HostileLocation();
				break;

			case "HenrisRoom":
				_henrisRoom = toggle;
				GameManager.Effects.HenrisRoom(toggle);
				break;
		}
		OnLocationChanged?.Invoke();
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: LOCALIZATION

	public string Primary() {
		if(_locationKeys.Count > 0) {
			if(_hostileLocation > 0) {
				return Localize.GetText(_locationKeys[0].Primary) + " <color=#" + ColorUtility.ToHtmlStringRGB(GameManager.Config.UI.HostileTextColor) + ">"  + Localize.GetText("Area/Hostile")  + "</color>";
			}
			else if(_henrisRoom) {
				return "<color=#" + ColorUtility.ToHtmlStringRGB(GameManager.Config.UI.HenrisRoomTextColor) + ">"  + Localize.GetText(_locationKeys[0].Primary) + "</color>";
			}
			return Localize.GetText(_locationKeys[0].Primary);
		}
		return "";
	}
	public string Secondary() {
		if(_locationKeys.Count > 0) {
			return Localize.GetText(_locationKeys[0].Secondary);
		}
		return "";
	}

	public int Contains(string primaryKey, string secondaryKey) {
		for (int i = 0; i < _locationKeys.Count; i++) {
			if (_locationKeys[i].Primary == primaryKey && _locationKeys[i].Secondary == secondaryKey) {
				return i;
			}
		}
		return -1;
	}
}
public class LocationKeys {
	public string Primary;
	public string Secondary;
	public LocationKeys(string newPrimary, string newSecondary) {
		Primary = newPrimary;
		Secondary = newSecondary;
	}
}
}