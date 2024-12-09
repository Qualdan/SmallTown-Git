// "Copy Spritesheet" original by angelsolares on Unity Forum (25/02/2015)
// "Copy Spritesheet" modified by Rampe on Unity Forum (03/06/2015)
// "Copy components" by Markus_T on Unity Forum (14/12/2016)
// "Copier" by Ville Hankipohja (14/04/2020)

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Copier : EditorWindow {

	// GUI
	private GUIStyle headlineStyle;
	private GUIStyle warningStyle;
	private GUIStyle buttonStyle;
	private Color buttonSelected = new Color(0.5f, 0.5f, 0.5f, 1.0f);

	// Toolbar
	private int toolbarInt = 0;
	private string[] toolbarTitle = {"Components", "Spritesheet" };

	// Components
	private List<ComponentSelector> componentList = new List<ComponentSelector>();
	private GameObject componentFrom = null;
	private GameObject componentFromOld = null;
	private GameObject componentTo = null;
	private bool copyTag = false;
	private bool copyLayer = false;
	private bool copyStatic = false;
	private bool copyName = false;
	private bool copyActive = false;
	private bool removeFrom = false;
	private bool removeTo = false;

	// Spritesheet
	// private Texture2D spriteFrom = null;
	// private Texture2D spriteTo = null;


	// GUI
	[MenuItem ("Tools/Custom/Copier #&c", false, 101)]
	public static void ShowWindow () {
		Copier window = (Copier)EditorWindow.GetWindow(typeof(Copier), false, "Copier");
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
		EditorGUILayout.Space();
		toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarTitle, GUILayout.Height(30.0f));
		string emptyError = "";
		switch(toolbarInt) {

			// Copy components
			case 0:
				EditorGUILayout.LabelField("COPY COMPONENTS", headlineStyle);
				componentFrom = (GameObject)EditorGUILayout.ObjectField("Copy From", componentFrom, typeof(GameObject), true);
				componentTo = (GameObject)EditorGUILayout.ObjectField("Copy To", componentTo, typeof(GameObject), true);
				if (componentFrom == null && componentTo == null) {
					emptyError = "Both 'Copy From' and 'Copy To' are empty";
				}
				else if (componentFrom == null) {
					emptyError = "'Copy From' is empty";
				}
				else if (componentFrom == null) {
					emptyError = "'Copy To' is empty";
				}
				if (emptyError != "") {
					EditorGUILayout.Space();
					EditorGUILayout.LabelField(emptyError, warningStyle);
				}
				EditorGUILayout.Space();
				EditorGUI.BeginDisabledGroup(componentFrom == null || componentTo == null);
				if(HeadlineButton("COPY COMPONENTS")) {
					CopyComponents("Copied Components");
				}
				EditorGUI.EndDisabledGroup();
				Line();
				GUILayout.BeginHorizontal();
					PrefixLabel("Copy Also", "Copy also object's tag, layer, name active state or static toggle");
					if(Button("Tag", copyTag)) {
						copyTag = !copyTag;
					}
					if(Button("Layer", copyLayer)) {
						copyLayer = !copyLayer;
					}
					if(Button("Name", copyName)) {
						copyName = !copyName;
					}
					GUI.backgroundColor = Color.white;
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
					PrefixLabel(" ", "");
					if(Button("Active State", copyActive)) {
						copyActive = !copyActive;
					}
					if(Button("Static Toggle", copyStatic)) {
						copyStatic = !copyStatic;
					}
					GUI.backgroundColor = Color.white;
				GUILayout.EndHorizontal();
				Line();
				if (componentFromOld != componentFrom) {
					componentList.Clear();
					foreach (var component in componentFrom.GetComponents<Component>()) {
						componentList.Add(new ComponentSelector(component, false));
					}
					componentFromOld = componentFrom;
				}
				for (int i = 0; i < componentList.Count; i++) {
					string ignoreHead = " ";
					if (i == 0) {
						ignoreHead = "Don't Copy Component";
					}
					GUILayout.BeginHorizontal();
						PrefixLabel(ignoreHead, "Ignore this component");
						string componentName = TrimComponentName(componentList[i].component.ToString());
						if(Button(componentName, componentList[i].ignore)) {
							componentList[i].ignore = !componentList[i].ignore;
						}
						GUI.backgroundColor = Color.white;
					GUILayout.EndHorizontal();
				}

				Line();
				GUILayout.BeginHorizontal();
					PrefixLabel("Remove Components", "Remove all components from the selected objects (components on 'From' object are removed after copying, components on 'To' object are removed before copying)");
					if(Button("'From' Object", removeFrom)) {
						removeFrom = !removeFrom;
					}
					if(Button("'To' Object", removeTo)) {
						removeTo = !removeTo;
					}
				GUILayout.EndHorizontal();

				break;

			// // Copy spritesheet
			// case 1:
			// 	EditorGUILayout.LabelField("COPY SPRITESHEET PIVOT AND SLICES", headlineStyle);
			// 	string fromName = "Copy From";
			// 	if (spriteFrom != null) {
			// 		fromName += " '" + spriteFrom.name + "'";
			// 	}
			// 	string toName = "Copy To";
			// 	if (spriteTo != null) {
			// 		toName += " '" + spriteTo.name + "'";
			// 	}
			// 	spriteFrom = (Texture2D)EditorGUILayout.ObjectField(fromName, spriteFrom, typeof(Texture2D), true);
			// 	spriteTo = (Texture2D)EditorGUILayout.ObjectField(toName, spriteTo, typeof(Texture2D), true);
			// 	if (spriteFrom == null && spriteTo == null) {
			// 		emptyError = "Both 'Copy From' and 'Copy To' are empty";
			// 	}
			// 	else if (spriteFrom == null) {
			// 		emptyError = "'Copy From' is empty";
			// 	}
			// 	else if (spriteTo == null) {
			// 		emptyError = "'Copy To' is empty";
			// 	}
			// 	if (emptyError != "") {
			// 		EditorGUILayout.LabelField(emptyError, warningStyle);
			// 	}
			// 	EditorGUILayout.Space();
			// 	EditorGUI.BeginDisabledGroup(spriteFrom == null || spriteTo == null);
			// 		if(HeadlineButton("COPY PIVOT AND SLICES")) {
			// 			CopySpritesheet("Copied Spritesheet");
			// 		}
			// 	EditorGUI.EndDisabledGroup();
			// 	break;
		}
	}
	bool Button(string text, bool changeColor) {
		GUI.backgroundColor = changeColor ? buttonSelected : Color.white;
		return GUILayout.Button(text, GUILayout.Height(20));
	}
	bool HeadlineButton(string text) {
		return GUILayout.Button(text, buttonStyle, GUILayout.Height(30));
	}
	void PrefixLabel(string text, string tooltip) {
		EditorGUILayout.PrefixLabel(new GUIContent(text, tooltip));
	}
	void Line() {
		EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
	}
	string TrimComponentName(string name) {
		string newName = name;
		newName = newName.Replace(componentFrom.name, "");
		newName = newName.Replace("UnityEngine.", "");
		newName = newName.Replace(" ", "");
		newName = newName.Replace("(", "");
		newName = newName.Replace(")", "");
		return newName;
	}


	// COPY COMPONENTS
	void CopyComponents(string undoText) {
		if (componentFrom == null || componentTo == null) {
			return;
		}
		Undo.RecordObject(componentTo, undoText);
		if (removeTo) {
			foreach (var component in componentTo.GetComponents<Component>()) {
				if (component.GetType() != typeof(Transform)) {
					DestroyImmediate(component);
				}
			}
		}
		for (int i = 0; i < componentList.Count; i++) {
			if (!componentList[i].ignore) {
				if (componentList[i].component.GetType() == typeof(Transform)) {
					componentTo.transform.position = componentFrom.transform.position;
					componentTo.transform.rotation = componentFrom.transform.rotation;
					componentTo.transform.localScale = componentFrom.transform.localScale;
				}
				else {
					UnityEditorInternal.ComponentUtility.CopyComponent(componentList[i].component);
					UnityEditorInternal.ComponentUtility.PasteComponentAsNew(componentTo);
				}
			}
		}
		if (copyTag) {
			componentTo.tag = componentFrom.tag;
		}
		if (copyLayer) {
			componentTo.layer = componentFrom.layer;
		}
		if (copyName) {
			componentTo.name = componentFrom.name;
		}
		if (copyStatic) {
			componentTo.isStatic = componentFrom.isStatic;
		}
		if (copyActive) {
			componentTo.SetActive(copyActive);
		}
		if (removeFrom) {
			foreach (var component in componentFrom.GetComponents<Component>()) {
				if (component.GetType() != typeof(Transform)) {
					DestroyImmediate(component);
				}
			}
		}
	}


	// // COPY SPRITESHEET
	// void CopySpritesheet(string undoText) {
	// 	if (spriteFrom == null || spriteTo == null) {
	// 		return;
	// 	}
	// 	Undo.RecordObject(spriteTo, undoText);
	// 	string copyFromPath = AssetDatabase.GetAssetPath(spriteFrom);
	// 	TextureImporter ti1 = AssetImporter.GetAtPath(copyFromPath) as TextureImporter;
	// 	ti1.isReadable = true;
	// 	string copyToPath = AssetDatabase.GetAssetPath(spriteTo);
	// 	TextureImporter ti2 = AssetImporter.GetAtPath(copyToPath) as TextureImporter;
	// 	ti2.isReadable = true;
	// 	List <SpriteMetaData> newData = new List <SpriteMetaData>();
	// 	ti2.spriteImportMode = SpriteImportMode.Single;
	// 	ti2.spritesheet = newData.ToArray();
	// 	ti2.spriteImportMode = SpriteImportMode.Multiple;
	// 	for (int i = 0; i < ti1.spritesheet.Length; i++) {
	// 	 SpriteMetaData d = ti1.spritesheet[i];
	// 	 newData.Add(d);
	// 	}
	// 	ti2.spritesheet = newData.ToArray();
	// 	AssetDatabase.ImportAsset(copyToPath, ImportAssetOptions.ForceUpdate);
	// }
}
public class ComponentSelector {
	public Component component;
	public bool ignore;
	public ComponentSelector(Component newComponent, bool newIgnore) {
		component = newComponent;
		ignore = newIgnore;
	}
}