// "LineMup" by Matthew J. Collins (Délé) on Unity Wiki (12/21/2009)
// "Distributor" by Ville Hankipohja (27/02/2020)

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class Distributor : EditorWindow {

	// GUI
	private GUIStyle headlineStyle;
	private GUIStyle warningStyle;
	private GUIStyle buttonStyle;
	private Color buttonSelected = new Color(0.5f, 0.5f, 0.5f, 1.0f);

	// Toolbar
	private int toolbarInt = 0;
	private string[] toolbarTitle = {"Distribute", "Align", "Grid", "Circle" };

	// Distribute
	private int distributeIndexFrom = 0;
	private int distributeIndexTo = -1;
	private bool distributeMoveX = false;
	private bool distributeMoveY = false;
	private bool distributeMoveZ = false;
	private bool distributeScaleX = false;
	private bool distributeScaleY = false;
	private bool distributeScaleZ = false;
	private bool distributeRotation = false;

	// Align
	private int alignIndex = 0;
	private bool alignMoveX = false;
	private bool alignMoveY = false;
	private bool alignMoveZ = false;
	private bool alignScaleX = false;
	private bool alignScaleY = false;
	private bool alignScaleZ = false;
	private bool alignRotation = false;

	// Grid
	private int gridIndex = 0;
	private int gridItems = 2;
	private int gridPrimaryType = 0;
	private int gridSecondaryType = 1;
	private float gridPrimaryPadding = 2.0f;
	private float gridSecondaryPadding = 2.0f;
	private bool gridTwoAxisPadding = false;

	// Circle
	private int circleIndex = 0;
	private int circleAxis = 0;
	private float circleRadius = 2.0f;
	private float circleArc = 360.0f;
	private float circleStart = 0.0f;
	private bool circleCenter = false;
	private bool circleOverlap = false;


	// GUI
	[MenuItem ("Tools/Custom/Distributor #&d", false, 100)]
	public static void ShowWindow () {
		Distributor window = (Distributor)EditorWindow.GetWindow(typeof(Distributor), false, "Distributor");
	}
	void CheckStyles() {
		if (headlineStyle == null) {
			headlineStyle = new GUIStyle(EditorStyles.label);
			headlineStyle.fontStyle = FontStyle.Bold;
			headlineStyle.alignment = TextAnchor.MiddleLeft;
		}
		if (warningStyle == null) {
			warningStyle = new GUIStyle(EditorStyles.label);
			warningStyle.fontStyle = FontStyle.Bold;
			warningStyle.normal.textColor = new Color(1.0f, 0.4f, 0.4f, 1.0f);
		}
		if (buttonStyle == null) {
			buttonStyle = new GUIStyle(GUI.skin.button);
			buttonStyle.fontStyle = FontStyle.Bold;
		}
	}
	void OnGUI () {
		CheckStyles();
		if (Selection.transforms.Length > 1) {
			EditorGUILayout.Space();
			toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarTitle, GUILayout.Height(30.0f));
			switch(toolbarInt) {

				// Distribute objects
				case 0:
					string[] fromArray = CreateArray(false, "");
					string toName = fromArray[distributeIndexFrom];
					string[] toArray = CreateArray(true, "<Furthest Object from '" + toName + "'>");
					if (distributeIndexTo == -1) {
						distributeIndexTo = toArray.Length - 1;
					}
					EditorGUILayout.Space();
					EditorGUILayout.LabelField("DISTRIBUTE " +  Selection.transforms.Length.ToString() + " OBJECTS EVENLY", headlineStyle);
					distributeIndexFrom = Popup("Distribute From", "Start distribution of all selected objects from this object", distributeIndexFrom, fromArray);
					distributeIndexTo = Popup("Distribute To", "End distribution of all selected objects to this object", distributeIndexTo, toArray);
					EditorGUILayout.Space();
					if(HeadlineButton("DISTRIBUTE OBJECTS")) {
						Distribute("Distributed Objects");
					}
					Line();
					GUILayout.BeginHorizontal();
						PrefixLabel("Distribute Axis", "Distribute objects axes evenly");
						if(Button("X", distributeMoveX)) {
							distributeMoveX = !distributeMoveX;
						}
						if(Button("Y", distributeMoveY)) {
							distributeMoveY = !distributeMoveY;
						}
						if(Button("Z", distributeMoveZ)) {
							distributeMoveZ = !distributeMoveZ;
						}
						GUI.backgroundColor = Color.white;
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
						PrefixLabel("Distribute Scale", "Distribute objects scale evenly");
						if(Button("X", distributeScaleX)) {
							distributeScaleX = !distributeScaleX;
						}
						if(Button("Y", distributeScaleY)) {
							distributeScaleY = !distributeScaleY;
						}
						if(Button("Z", distributeScaleZ)) {
							distributeScaleZ = !distributeScaleZ;
						}
						GUI.backgroundColor = Color.white;
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
						PrefixLabel("Distribute Rotation", "Distribute objects rotation evenly");
						if(Button("Rotation", distributeRotation)) {
							distributeRotation = !distributeRotation;
						}
						GUI.backgroundColor = Color.white;
					GUILayout.EndHorizontal();
					Line();
					GUILayout.BeginHorizontal();
						PrefixLabel("Select Options", "Multiselect above options");
						if(Button("All", false)) {
							distributeMoveX = distributeMoveY = distributeMoveZ = distributeScaleX = distributeScaleY = distributeScaleZ = distributeRotation = true;
						}
						if(Button("Toggle", false)) {
							distributeMoveX = !distributeMoveX;
							distributeMoveY = !distributeMoveY;
							distributeMoveZ = !distributeMoveZ;
							distributeScaleX = !distributeScaleX;
							distributeScaleY = !distributeScaleY;
							distributeScaleZ = !distributeScaleZ;
							distributeRotation = !distributeRotation;
						}
						if(Button("None", false)) {
							distributeMoveX = distributeMoveY = distributeMoveZ = distributeScaleX = distributeScaleY = distributeScaleZ = distributeRotation = false;
						}
					GUILayout.EndHorizontal();
					EditorGUILayout.Space();
				break;

				// Align objects
				case 1:
					string[] alignWith = new string[Selection.transforms.Length + 1];
					for (int i = 0; i < Selection.transforms.Length; i++) {
						alignWith[i] = Selection.transforms[i].name;
					}
					EditorGUILayout.Space();
					EditorGUILayout.LabelField("ALIGN " +  Selection.transforms.Length.ToString() + " OBJECTS TO SELECTED OBJECT", headlineStyle);
					alignIndex = Popup("Align With", "Align all selected objects with this object", alignIndex, alignWith);
					EditorGUILayout.Space(26);
					if(HeadlineButton("ALIGN OBJECTS")) {
						Align("Aligned Objects");
					}
					Line();
					GUILayout.BeginHorizontal();
						PrefixLabel("Align Axis", "Aling all selected objects to selected object's axes");
						if(Button("X", alignMoveX)) {
							alignMoveX = !alignMoveX;
						}
						if(Button("Y", alignMoveY)) {
							alignMoveY = !alignMoveY;
						}
						if(Button("Z", alignMoveZ)) {
							alignMoveZ = !alignMoveZ;
						}
						GUI.backgroundColor = Color.white;
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
						PrefixLabel("Align Scale", "Aling all selected objects on selected object's scale");
						if(Button("X", alignScaleX)) {
							alignScaleX = !alignScaleX;
						}
						if(Button("Y", alignScaleY)) {
							alignScaleY = !alignScaleY;
						}
						if(Button("Z", alignScaleZ)) {
							alignScaleZ = !alignScaleZ;
						}
						GUI.backgroundColor = Color.white;
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
						PrefixLabel("Align Rotation", "Aling all selected objects on selected object's rotation");
						if(Button("Rotation", alignRotation)) {
							alignRotation = !alignRotation;
						}
						GUI.backgroundColor = Color.white;
					GUILayout.EndHorizontal();
					Line();
					GUILayout.BeginHorizontal();
						PrefixLabel("Select Options", "Multiselect above options");
						if(Button("All", false)) {
							alignMoveX = alignMoveY = alignMoveZ = alignScaleX = alignScaleY = alignScaleZ = alignRotation = true;
						}
						if(Button("Toggle", false)) {
							alignMoveX = !alignMoveX;
							alignMoveY = !alignMoveY;
							alignMoveZ = !alignMoveZ;
							alignScaleX = !alignScaleX;
							alignScaleY = !alignScaleY;
							alignScaleZ = !alignScaleZ;
							alignRotation = !alignRotation;
						}
						if(Button("None", false)) {
							alignMoveX = alignMoveY = alignMoveZ = alignScaleX = alignScaleY = alignScaleZ = alignRotation = false;
						}
					GUILayout.EndHorizontal();
					EditorGUILayout.Space();
				break;

				// Form grid
				case 2:
					string[] gridFrom = new string[Selection.transforms.Length + 1];
					for (int i = 0; i < Selection.transforms.Length; i++) {
						gridFrom[i] = Selection.transforms[i].name;
					}
					EditorGUILayout.Space();
					EditorGUILayout.LabelField("FORM A GRID FROM " +  Selection.transforms.Length.ToString() + " OBJECTS", headlineStyle);
					gridIndex = Popup("Start Grid From", "Grid is started from this object", gridIndex, gridFrom);
					EditorGUILayout.Space(26);
					if(HeadlineButton("FORM A GRID")) {
						Grid("Objects Formed Grid");
					}
					Line();
					GUILayout.BeginHorizontal();
						PrefixLabel("Primary Axis", "Primary axis where objects are placed first");
						if(Button("X", gridPrimaryType == 0)) {
							gridPrimaryType = 0;
						}
						if(Button("Y",  gridPrimaryType == 1)) {
							gridPrimaryType = 1;
						}
						if(Button("Z", gridPrimaryType == 2)) {
							gridPrimaryType = 2;
						}
						GUI.backgroundColor = Color.white;
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
						PrefixLabel("Secondary Axis", "Secondary axis where objects are placed after first axis reaches maximum allotted objects");
						if(Button("X", gridSecondaryType == 0)) {
							gridSecondaryType = 0;
						}
						if(Button("Y", gridSecondaryType == 1)) {
							gridSecondaryType = 1;
						}
						if(Button("Z", gridSecondaryType == 2)) {
							gridSecondaryType = 2;
						}
						GUI.enabled = true;
						GUI.backgroundColor = Color.white;
					GUILayout.EndHorizontal();
					if (gridPrimaryType == gridSecondaryType) {
						GUILayout.BeginHorizontal();
							PrefixLabel(" ", "");
							EditorGUILayout.LabelField("Axes need to be different", warningStyle);
						GUILayout.EndHorizontal();
					}
					GUILayout.BeginHorizontal();
						PrefixLabel("Objects in Primary Axis", "Maximum allotted objects per primary axis");
						gridItems = EditorGUILayout.IntSlider(gridItems, 1, 100);
					GUILayout.EndHorizontal();
					if (!gridTwoAxisPadding) {
						gridPrimaryPadding = Slider("Object Padding", "Set padding between different items for both axes", gridPrimaryPadding, 0.0f, 100.0f);
						gridSecondaryPadding = gridPrimaryPadding;
					}
					else {
						gridPrimaryPadding = Slider("Primary Axis Padding", "Padding for primary axis", gridPrimaryPadding, 0.0f, 100.0f);
						gridSecondaryPadding = Slider("Secondary Axis Padding", "Padding for secondary axis", gridSecondaryPadding, 0.0f, 100.0f);
					}
					GUILayout.BeginHorizontal();
						PrefixLabel("Padding Type", "Items can have equal padding on each axis or separate ones");
						if(Button("Uniform", !gridTwoAxisPadding)) {
							gridTwoAxisPadding = false;
						}
						if(Button("Separate", gridTwoAxisPadding)) {
							gridTwoAxisPadding = true;
						}
					GUILayout.EndHorizontal();
					Line();
					GUILayout.BeginHorizontal();
						PrefixLabel("Reset Options", "Reset all options to their default value");
						if(Button("Reset", false)) {
							gridItems = 2;
							gridPrimaryType = 0;
							gridSecondaryType = 1;
							gridPrimaryPadding = 2.0f;
							gridSecondaryPadding = 2.0f;
							gridTwoAxisPadding = false;
						}
					GUILayout.EndHorizontal();
					EditorGUILayout.Space();
				break;

				// Form circle
				case 3:
					string[] circleFrom = new string[Selection.transforms.Length + 1];
					for (int i = 0; i < Selection.transforms.Length; i++) {
						circleFrom[i] = Selection.transforms[i].name;
					}
					EditorGUILayout.Space();
					EditorGUILayout.LabelField("FORM A CIRCLE FROM " +  Selection.transforms.Length.ToString() + " OBJECTS", headlineStyle);
					circleIndex = Popup("Form Circle Around", "Circle can form around the currently selected object", circleIndex, circleFrom);
					EditorGUILayout.Space(26);
					if(HeadlineButton("FORM A CIRCLE")) {
						Circle("Objects Formed Circle");
					}
					Line();
				GUILayout.BeginHorizontal();
						PrefixLabel("Form Circle on Axis", "Form a circle on selected axis (the formed circle will face that direction)");
						if(Button("X", circleAxis == 0)) {
							circleAxis = 0;
						}
						if(Button("Y", circleAxis == 1)) {
							circleAxis = 1;
						}
						if(Button("Z", circleAxis == 2)) {
							circleAxis = 2;
						}
						GUI.backgroundColor = Color.white;
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
						PrefixLabel("Form Circle Around", "Form a circle around either selected object or the centerpont of all selected objects");
						if(Button("Selection", !circleCenter)) {
							circleCenter = false;
						}
						if(Button("Centerpoint", circleCenter)) {
							circleCenter = true;
						}
						GUI.backgroundColor = Color.white;
					GUILayout.EndHorizontal();
					circleRadius = Slider("Circle Radius", "Radius of the formed circle", circleRadius, 0.0f, 100.0f);
					circleStart = Slider("Start Circle From", "Degree in which to place the first object (0 is at top)", circleStart, 0.0f, 360.0f);
					circleArc = Slider("Circle Arc Length", "Length of the circle arc in degrees", circleArc, 0.0f, 360.0f);
					GUILayout.BeginHorizontal();
						PrefixLabel("Object Overlap", "Last position is left empty to prevent overlap on full circle, but enabling this makes it easier to set partial arcs");
						if(Button("Allow", circleOverlap)) {
							circleOverlap = !circleOverlap;
						}
					GUILayout.EndHorizontal();
					Line();
					GUILayout.BeginHorizontal();
						PrefixLabel("Reset Options", "Reset all options to their default value");
						if(Button("Reset", false)) {
							circleAxis = 0;
							circleRadius = 2.0f;
							circleArc = 360.0f;
							circleStart = 0.0f;
							circleCenter = false;
							circleOverlap = false;
						}
						GUI.backgroundColor = Color.white;
					GUILayout.EndHorizontal();
					EditorGUILayout.Space();
				break;
			}
		}
		else {
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("SELECT AT LEAST TWO OBJECTS", warningStyle);
			EditorGUILayout.LabelField("Move cursor over window to update selection");
		}
	}
	bool Button(string text, bool changeColor) {
		GUI.backgroundColor = changeColor ? buttonSelected : Color.white;
		return GUILayout.Button(text, GUILayout.Height(20));
	}
	bool HeadlineButton(string text) {
		return GUILayout.Button(text, buttonStyle, GUILayout.Height(30));
	}
	float Slider(string text, string tooltip, float value, float min, float max) {
		return EditorGUILayout.Slider(new GUIContent(text, tooltip), value, min, max);
	}
	void PrefixLabel(string text, string tooltip) {
		EditorGUILayout.PrefixLabel(new GUIContent(text, tooltip));
	}
	int Popup(string text, string tooltip, int value, string[] array) {
		return EditorGUILayout.Popup(new GUIContent(text, tooltip), value, array);
	}
	void Line() {
		EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
	}
	string[] CreateArray(bool extra, string text) {
		List<string> selectionArray = new List<string>();
		for (int i = 0; i < Selection.transforms.Length; i++) {
			selectionArray.Add(Selection.transforms[i].name);
		}
		if (extra) {
			selectionArray.Add(text);
		}
		return selectionArray.ToArray();
	}


	// DISTRIBUTE
	void Distribute(string undoText) {
		List<Transform> selections = new List<Transform>(Selection.transforms);
		Transform firstObj = null;
		if (distributeIndexFrom > 0 && distributeIndexFrom < selections.Count) {
			firstObj = selections[distributeIndexFrom];
		}
		else {
			firstObj = selections[0];
		}
		Transform lastObj = null;
		if (distributeIndexTo < selections.Count) {
			lastObj = selections[distributeIndexTo];
		}
		else {
			lastObj = FurthestObject(firstObj);
		}
		float lerp = 1.0f / (selections.Count - 1.0f);
		Vector3 spacing = new Vector3(lerp, lerp, lerp);
		Vector3 moveValue = spacing;
		Vector3 scaleValue = spacing;
		float rotationValue = lerp;
		for (int i = 0; i < selections.Count; i++) {
			if ((selections[i] != firstObj) && (selections[i] != lastObj)) {
				Vector3 position = selections[i].position;
				Quaternion rotation = selections[i].rotation;
				Vector3 scale = selections[i].localScale;
				if (distributeMoveX) {
					position.x = Mathf.Lerp(firstObj.position.x, lastObj.position.x, moveValue.x);
					moveValue.x += spacing.x;
				}
				if (distributeMoveY) {
					position.y = Mathf.Lerp(firstObj.position.y, lastObj.position.y, moveValue.y);
					moveValue.y += spacing.y;
				}
				if (distributeMoveZ) {
					position.z = Mathf.Lerp(firstObj.position.z, lastObj.position.z, moveValue.z);
					moveValue.z += spacing.z;
				}

				if (distributeScaleX) {
					scale.x = Mathf.Lerp(firstObj.localScale.x, lastObj.localScale.x, scaleValue.x);
					scaleValue.x += spacing.x;
				}
				if (distributeScaleY) {
					scale.y = Mathf.Lerp(firstObj.localScale.y, lastObj.localScale.y, scaleValue.y);
					scaleValue.y += spacing.y;
				}
				if (distributeScaleZ) {
					scale.z = Mathf.Lerp(firstObj.localScale.z, lastObj.localScale.z, scaleValue.z);
					scaleValue.z += spacing.z;
				}
				if (distributeRotation) {
					rotation = Quaternion.Slerp(firstObj.rotation, lastObj.rotation, rotationValue);
					rotationValue += lerp;
				}
				Undo.RecordObject(selections[i], undoText);
				selections[i].position = position;
				selections[i].rotation = rotation;
				selections[i].localScale = scale;
			}
		}
	}
	Transform FurthestObject(Transform firstObj) {
		Transform furthestObj = firstObj;
		float currentDistance = 0.0f;
		float lastDistance = 0.0f;
		foreach (var transform in Selection.transforms) {
			currentDistance = Vector3.Distance(firstObj.position, transform.position);
			if (currentDistance > lastDistance) {
				furthestObj = transform;
				lastDistance = currentDistance;
			}
		}
		return furthestObj;
	}
	int FurthestObjectIndex(Transform current) {
		Transform obj = FurthestObject(current);
		for (int i = 0; i < Selection.transforms.Length; i++) {
			if (obj = Selection.transforms[i]) {
				return i;
			}
		}
		return -1;
	}


	// ALIGN
	void Align(string undoText) {
		Transform firstObj = Selection.transforms[alignIndex];
		foreach (var transform in Selection.transforms) {
			Vector3 position = transform.position;
			Quaternion rotation = transform.rotation;
			Vector3 scale = transform.localScale;
			if (alignMoveX) {
				position.x = firstObj.position.x;
			}
			if (alignMoveY) {
				position.y = firstObj.position.y;
			}
			if (alignMoveZ) {
				position.z = firstObj.position.z;
			}
			if (alignScaleX) {
				scale.x = firstObj.localScale.x;
			}
			if (alignScaleY) {
				scale.y = firstObj.localScale.y;
			}
			if (alignScaleZ) {
				scale.z = firstObj.localScale.z;
			}
			if (alignRotation) {
				rotation = firstObj.rotation;
			}
			Undo.RecordObject(transform, undoText);
			transform.position = position;
			transform.rotation = rotation;
			transform.localScale = scale;
		}
	}


	// FORM GRID
	void Grid(string undoText) {
		List<Transform> selections = new List<Transform>(Selection.transforms);
		Transform selected = selections[gridIndex];
		selections.RemoveAt(gridIndex);
		selections = selections.OrderBy(tr => Vector3.Distance(selected.position, tr.position)).ToList();
		selections.Insert(0, selected);
		Vector3 startPosition = selected.position;
		int axisPrimary = 0;
		int axisSecondary = 0;
		for (int i = 0; i < selections.Count; i++) {
			Vector3 position = selections[i].position;
			switch (gridPrimaryType) {
				case 0:
					switch (gridSecondaryType) {
						case 1:
							position = startPosition + (new Vector3(gridPrimaryPadding * axisPrimary, -(gridSecondaryPadding * axisSecondary), 0.0f));
							break;
						case 2:
							position = startPosition + (new Vector3(gridPrimaryPadding * axisPrimary, 0.0f, gridSecondaryPadding * axisSecondary));
							break;
					}
					break;

				case 1:
					switch (gridSecondaryType) {
						case 0:
							position = startPosition + (new Vector3(gridSecondaryPadding * axisSecondary, -(gridPrimaryPadding * axisPrimary), 0.0f));
							break;
						case 2:
							position = startPosition + (new Vector3(0.0f, -(gridPrimaryPadding * axisPrimary), gridSecondaryPadding * axisSecondary));
							break;
					}
					break;

				case 2:
					switch (gridSecondaryType) {
						case 0:
							position = startPosition + (new Vector3(gridSecondaryPadding * axisSecondary, 0.0f, gridPrimaryPadding * axisPrimary));
							break;
						case 1:
							position = startPosition + (new Vector3(0.0f, -(gridSecondaryPadding * axisSecondary), gridPrimaryPadding * axisPrimary));
							break;
					}
					break;
			}
			axisPrimary++;
			if (axisPrimary >= gridItems) {
				axisSecondary++;
				axisPrimary = 0;
			}
			Undo.RecordObject(selections[i], undoText);
			selections[i].position = position;
		}
	}


	// FORM CIRCLE
	void Circle(string undoText) {
		if (Selection.transforms.Length > 0) {
			Vector3 defaultPosition = Vector3.zero;
			List<Transform> selections = new List<Transform>(Selection.transforms);
			Transform selected = selections[circleIndex];
			selections.RemoveAt(circleIndex);
			selections = selections.OrderBy(tr => Vector3.Distance(selected.position, tr.position)).ToList();
			selections.Insert(0, selected);
			int count = Selection.transforms.Length;
			int index = 0;
			if (circleCenter) {
				defaultPosition = GetCenterPoint();
				if (circleOverlap) {
					count -= 1;
				}
			}
			else {
				defaultPosition = selected.position;
				if (circleOverlap) {
					count -= 2;
				}
				else {
					count -= 1;
				}
			}

			for (int i = 0; i < selections.Count; i++) {
				if (!circleCenter && selections[i] == selected) {
					continue;
				}
				float circleLength = Remap(circleArc, 0.0f, 360.0f, 0.0f, 2.0f);
				float angle = (index * Mathf.PI * circleLength / count) + (circleStart * Mathf.Deg2Rad);
				Vector3 position = defaultPosition;
				switch (circleAxis) {
					case 0:
						position += new Vector3(0.0f, Mathf.Cos(angle) * circleRadius, Mathf.Sin(angle) * circleRadius);
						break;
					case 1:
						position += new Vector3(Mathf.Sin(angle) * circleRadius, 0.0f, Mathf.Cos(angle) * circleRadius);
						break;
					case 2:
						position += new Vector3(Mathf.Sin(angle) * circleRadius, Mathf.Cos(angle) * circleRadius, 0.0f);
						break;
				}
				index++;
				Undo.RecordObject(selections[i], undoText);
				selections[i].position = position;
			}
		}
	}
	Vector3 GetCenterPoint() {
		Vector3 center = Vector3.zero;
		foreach (var transform in Selection.transforms) {
			center += transform.position;
		}
		center /= Selection.transforms.Length;
		return center;
	}
	float Remap(float value, float from1, float to1, float from2, float to2) {
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}
}