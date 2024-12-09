using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Localization.Tables;
// using UnityEngine.Localization.Settings;
using UnityEditor;
using UnityEditor.Localization;

namespace SmallTown {
public class LocalizationTools {


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	[MenuItem("Tools/Small Town/Sort Localization", false, -25)]
	public static void SortTable() {
		StringTableCollection collection = LocalizationEditorSettings.GetStringTableCollection("LocalizedStrings");
		collection.SharedData.Entries.Sort((a, b) => { return a.Key.CompareTo(b.Key); });
		EditorUtility.SetDirty(collection.SharedData);
	}
}
}