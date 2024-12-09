using System.Collections.Generic;
using UnityEngine;

namespace SmallTown {
[CreateAssetMenu(fileName = "New Credits", menuName = "SmallTown/System/Credits")]
public class CreditsData : ScriptableObject {

	public List<CreditsHeadline> Titles;

}
[System.Serializable]
public class CreditsHeadline {
	public string Key;
	public List<CreditsEntry> Entries;
}
[System.Serializable]
public class CreditsEntry {
	public string Title;
	public string Name;
}
}