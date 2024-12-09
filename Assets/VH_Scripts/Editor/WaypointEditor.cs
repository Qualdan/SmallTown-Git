using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SmallTown {
public class WaypointEditor : Editor {

	// Objects
	private EnemyWaypoints waypoints;

	// Colors
	private Color guiGreen = new Color(0.22f, 0.8f, 0.53f, 1.0f);
	private Color guiBlue = new Color(0.42f, 0.73f, 1.0f, 1.0f);
	private Color guiRed = new Color(0.889f, 0.278f, 0.278f, 1.0f);

	// Styles
	private GUIStyle labelStyle;
	private GUIStyle buttonStyle;



	// ************************************************************************************************************ UNITY FUNCTIONS

	void OnEnable() {
		waypoints = (EnemyWaypoints)target;
		if (labelStyle == null) {
			labelStyle = new GUIStyle(EditorStyles.label);
			labelStyle.normal.textColor = guiGreen;
			labelStyle.fontStyle = FontStyle.Bold;
		}
		if (buttonStyle == null) {
			buttonStyle = new GUIStyle(EditorStyles.miniButton);
			buttonStyle.fontStyle = FontStyle.Bold;
		}
	}

	public override void OnInspectorGUI() {
		// bool disallowNew = false;																					// Disallow new button
		GUI.contentColor = guiBlue;
		EditorGUILayout.LabelField("WAYPOINT OPTIONS", EditorStyles.boldLabel);
		GUI.contentColor = Color.white;

		if (waypoints.WaypointCount > 0) {
			// Remove currently selected waypoint?
		}
		Line();
		EditorGUILayout.Space();
		GUI.backgroundColor = guiGreen;
		if(GUILayout.Button("New Waypoint", buttonStyle)) {
			waypoints.AddWaypoint();
		}
		GUI.backgroundColor = Color.white;
		// else {
		// 	Line();
		// 	EditorGUILayout.Space();
		// 	GUI.backgroundColor = guiGreen;
		// 	if(GUILayout.Button("Add new action", buttonStyle)) {
		// 		trap.AddAction();
		// 	}
		// 	GUI.backgroundColor = Color.white;
		// }
		// // EditorGUILayout.Space();
		// EditorGUILayout.Space();
	}



	// ************************************************************************************************************ CUSTOM FUNCTIONS

	// SMALL BUTTON
	bool Button(string text, string tooltip) {
		return GUILayout.Button(new GUIContent(text, tooltip), buttonStyle, GUILayout.Width(30));
	}

	// ADD LINE
	void Line() {
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
	}
}
}