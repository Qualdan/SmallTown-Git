using System.Collections.Generic;
using UnityEngine;

namespace SmallTown {
[CreateAssetMenu(fileName = "New Settings", menuName = "SmallTown/System/Settings")]
public class SettingsData : ScriptableObject {

	public List<SettingsHeadline> settings;

}
[System.Serializable]
public class SettingsHeadline {
	public string key;
	public List<SettingsCategory> categories;
}
[System.Serializable]
public class SettingsCategory {
	public string key;
	public List<SettingsEntry> entries;
}
[System.Serializable]
public class SettingsEntry {
	public string key;
	public OptionTypes type;
	public MinMax initial;
	[HideInInspector] public float value;
}
}