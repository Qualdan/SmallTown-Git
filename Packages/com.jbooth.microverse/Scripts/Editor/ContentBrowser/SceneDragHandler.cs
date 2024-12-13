using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static JBooth.MicroVerseCore.Browser.ContentBrowser;

#if __MICROVERSE_ROADS__
using UnityEngine.Splines;
#endif

namespace JBooth.MicroVerseCore.Browser
{
    public class SceneDragHandler
    {
        /// <summary>
        /// Identifier for the generic data of a drag and drop operation
        /// </summary>
        public static string EnabledCollidersId = "Enabled Colliders";

        /// <summary>
        /// Identifier whether the draggable is a MicroVerse object or not.
        /// Had to be introduced in case the content browser remained open the entire session.
        /// Otherwise it would handle all objects that are dragged into the scene and eg remove them after the drop
        /// </summary>
        public static string IsMicroverseDraggableId = "MicroVerse Draggable";

        /// <summary>
        /// Whether shift was pressed during the start of the drag operation or not
        /// </summary>
        public static string WasShiftPressed = "Shift Pressed";

        /// <summary>
        /// Whether control was pressed during the start of the drag operation or not
        /// </summary>
        public static string WasControlPressed = "Control Pressed";

        private ContentBrowser browser;

        public SceneDragHandler(ContentBrowser browser)
        {
            this.browser = browser;
        }

        public void OnEnable()
        {
            SceneView.beforeSceneGui += this.OnSceneGUI;
        }

        public void OnDisable()
        {
            SceneView.beforeSceneGui -= this.OnSceneGUI;
        }

        private void OnSceneGUI(SceneView obj)
        {
            HandleDragAndDropEvents();
        }

        public void OnDragStart( PresetItem preset)
        {

            DragAndDrop.PrepareStartDrag();
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            DragAndDrop.SetGenericData("preset", preset);
            DragAndDrop.SetGenericData(IsMicroverseDraggableId, true);

            // record if shift was pressed at the start of the drag operation
            bool wasShiftPressed = Event.current.shift;
            DragAndDrop.SetGenericData(WasShiftPressed, wasShiftPressed);

            // record if control was pressed at the start of the drag operation
            bool wasControlPressed = Event.current.control;
            DragAndDrop.SetGenericData(WasControlPressed, wasControlPressed);

            DragAndDrop.StartDrag("Dragging MyData");

        }

        public void HandleDragAndDropEvents()
        {
            if (Event.current.type != EventType.DragUpdated && Event.current.type != EventType.DragPerform)
                return;

            if (EditorWindow.mouseOverWindow == browser)
                return;

            object isMicroVerseDraggable = DragAndDrop.GetGenericData( IsMicroverseDraggableId);

            bool valid = isMicroVerseDraggable != null && isMicroVerseDraggable is bool && (bool)isMicroVerseDraggable == true;

            if (!valid)
                return;

            object wasShiftPressedObject = DragAndDrop.GetGenericData(WasShiftPressed);
            object wasControlPressedObject = DragAndDrop.GetGenericData(WasControlPressed);

            bool wasShiftPressed = wasShiftPressedObject != null && wasShiftPressedObject is bool && (bool)wasShiftPressedObject == true;
            bool wasControlPressed = wasControlPressedObject != null && wasControlPressedObject is bool && (bool)wasControlPressedObject == true;

            PresetItem preset = (PresetItem)DragAndDrop.GetGenericData("preset");
            if (preset != null)
            {
                DragAndDrop.SetGenericData("preset", null);

                GameObject draggable = CreateInstance(preset, wasShiftPressed, wasControlPressed);

                DragAndDrop.objectReferences = new GameObject[] { draggable };
                DragAndDrop.paths = null;

                // disable colliders, we don't want to raycast against self; store as generic data for re-enabling later
                Collider[] enabledColliders = draggable.GetComponentsInChildren<Collider>().Where(x => x.enabled == true).ToArray();
                foreach (Collider collider in enabledColliders)
                {
                    collider.enabled = false;
                }
                DragAndDrop.SetGenericData( EnabledCollidersId, enabledColliders);

            }


            if (Event.current.type == EventType.DragUpdated)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    // if( !instance.activeInHierarchy)
                    //    instance.SetActive(true);

                    Vector3 point = hit.point;

                    if (DragAndDrop.objectReferences.Length == 1)
                    {
                        if (DragAndDrop.objectReferences[0] is GameObject)
                        {
                            GameObject go = DragAndDrop.objectReferences[0] as GameObject;
                            if (browser.GetSelectedTab() == Tab.Height)
                            {
                                point.y = 0; // height stamps always at 0
                            }
                            else if (browser.GetSelectedTab() == Tab.Roads)
                            {
#if __MICROVERSE_ROADS__
                                point.y += browser.heightOffset;
#endif
                            }
                            go.transform.position = point;
                        }
                    }
                }
            }
            else if (Event.current.type == EventType.DragPerform)
            {

                DragAndDrop.SetGenericData(IsMicroverseDraggableId, false);
                DragAndDrop.SetGenericData(WasShiftPressed, false);
                DragAndDrop.SetGenericData(WasControlPressed, false);

                DragAndDrop.AcceptDrag();

                if (DragAndDrop.objectReferences.Length == 1)
                {
                    // re-enable colliders which were diabled before the drag operation
                    Collider[] enabledColliders = (Collider[])DragAndDrop.GetGenericData(EnabledCollidersId);
                    if (enabledColliders != null)
                    {
                        foreach (Collider collider in enabledColliders)
                        {
                            collider.enabled = true;
                        }
                    }

                    // reference to new object
                    GameObject go = DragAndDrop.objectReferences[0] as GameObject;

                    bool centerTerrain = wasControlPressed;
                    if (centerTerrain)
                    {
                        Terrain[] terrains = MicroVerse.instance.GetComponentsInChildren<Terrain>();

                        Bounds worldBounds = TerrainUtil.ComputeTerrainBounds(terrains);

                        // position
                        float y = worldBounds.center.y - worldBounds.size.y / 2f;

                        if (browser.GetSelectedTab() == Tab.Roads)
                        {
#if __MICROVERSE_ROADS__
                            y += browser.heightOffset;
#endif
                        }

                        go.transform.transform.position = new Vector3(worldBounds.center.x, y, worldBounds.center.z);
                    }

                    // select new object
                    Selection.activeObject = go;
                }

                // cleanup drag
                DragFinished();

                DragAndDrop.objectReferences = new Object[0];
                DragAndDrop.visualMode = DragAndDropVisualMode.None;
                DragAndDrop.SetGenericData(EnabledCollidersId, null);

                Event.current.Use();

            }
            /* note: doesn't seem to work, DragExited is also invoked after DragPerform
            // eg escape pressed
            else if (Event.current.type == EventType.DragExited)
            {
                if (DragAndDrop.objectReferences.Length == 1)
                {
                    GameObject go = DragAndDrop.objectReferences[0] as GameObject;
                    GameObject.DestroyImmediate(go);
                }

                // cleanup drag
                DragAndDrop.objectReferences = new Object[0];
                DragAndDrop.visualMode = DragAndDropVisualMode.None;

                Event.current.Use();

                DragFinished();
            }
            */
        }

        private GameObject CreateInstance(PresetItem preset, bool wasShiftPressed, bool wasControlPressed)
        {
            GameObject instance = null;

            if ( preset == null)
            {
                return null;
            }

            var tab = browser.GetSelectedTab();
            var selected = preset.content;

            if (selected.prefab != null)
            {
                // TODO: If instantiated as a prefab, using the painter causes a hang
                // PrefabUtility.InstantiatePrefab(selected.prefab) as GameObject;
                // Update: Unpacking the prefab solves the problem. Keeping this comment here in case unforeseen issues arise
                // Original code was: instance = GameObject.Instantiate(selected.prefab);
                instance = PrefabUtility.InstantiatePrefab(selected.prefab) as GameObject;
                if (ContentBrowser.tab != Tab.Roads)
                {
                    PrefabUtility.UnpackPrefabInstance(instance, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                }
                instance.name = selected.prefab.name;
                if (selected.childPrefab != null)
                {
                    var harness = PrefabUtility.InstantiatePrefab(selected.prefab) as GameObject;
                    PrefabUtility.UnpackPrefabInstance(harness, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                    harness.transform.SetParent(instance.transform, false);
                }

                if (tab == Tab.Height)
                {
                    instance.transform.localScale = browser.heightStampDefaultScale;
                }
                else if (tab == Tab.Roads)
                {

                }
                else if (tab == Tab.Texture)
                {
                    instance.transform.localScale = browser.textureStampDefaultScale;
                }
                else if (tab == Tab.Vegetation)
                {
                    instance.transform.localScale = browser.vegetationStampDefaultScale;
#if __MICROVERSE_VEGETATION__
                    var trees = instance.GetComponentsInChildren<TreeStamp>();
                    var details = instance.GetComponentsInChildren<DetailStamp>();
                    foreach (var t in trees)
                    {
                        t.seed = (uint)Random.Range(0, 99);
                    }
                    foreach (var d in details)
                    {
                        d.prototype.noiseSeed = (int)Random.Range(0, 99);
                    }
#endif
                }
            }
            else if (selected.stamp != null && tab == Tab.Height)
            {
                var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(new GUID(selected.stamp)));
                if (tex != null)
                {
                    instance = new GameObject(tex.name + " (Height Stamp)");
                    HeightStamp heightStamp = instance.AddComponent<HeightStamp>();

                    heightStamp.stamp = tex;
                    heightStamp.mode = HeightStamp.CombineMode.Add;

                    heightStamp.falloff.filterType = (FalloffFilter.FilterType)browser.filterTypeDefault;
                    heightStamp.falloff.falloffRange = new Vector2(0.8f, 1f);

                    // overrides
                    bool autoScaleTerrain = wasShiftPressed;
                    if (autoScaleTerrain)
                    {
                        Terrain[] terrains = MicroVerse.instance.GetComponentsInChildren<Terrain>();

                        Bounds worldBounds = TerrainUtil.ComputeTerrainBounds(terrains);

                        // scale
                        float x = worldBounds.size.x;
                        float y = worldBounds.size.y;
                        float z = worldBounds.size.z;

                        // if y is dynamic and depends on the current bounds
                        // however if it's very low or 0 in case it's the first terrain we use
                        // a heuristic to calculate a resonable height. the values are just arbitrary
                        float threshold = 10f;
                        if (y < threshold)
                        {
                            if (Terrain.activeTerrain)
                            {
                                y = TerrainUtil.ComputeTerrainSize(Terrain.activeTerrain).y * 0.1f;
                            }
                            else
                            {
                                y = threshold;
                            }
                        }

                        instance.transform.localScale = new Vector3(x, y, z);
                    }
                    else
                    {
                        instance.transform.localScale = browser.heightStampDefaultScale;
                    }
                }
            }
            if (instance != null)
            {
                // hide instance initially, we don't want it visible at 0/0/0
                // instance.SetActive(false);

                // overrides
                // shift at start of drag operation: force falloff type global
                bool forceFalloffTypeGlobal = wasShiftPressed;
                if (forceFalloffTypeGlobal)
                {
                    FalloffOverride falloffOverride = instance.GetComponent<FalloffOverride>();
                    if (falloffOverride && falloffOverride.enabled)
                    {
                        falloffOverride.filter.filterType = FalloffFilter.FilterType.Global;
                    }
                    else
                    {
                        Stamp[] stamps = instance.GetComponentsInChildren<Stamp>();
                        foreach (Stamp stamp in stamps)
                        {
                            if (stamp.GetFilterSet() == null)
                                continue;

                            stamp.GetFilterSet().falloffFilter.filterType = FalloffFilter.FilterType.Global;
                        }
                    }
                }
                if (tab == Tab.Roads)
                {
#if __MICROVERSE_ROADS__
                    RoadSystem[] roadSystems = null;
                    if (MicroVerse.instance == null)
                    {
                        roadSystems = GameObject.FindObjectsOfType<RoadSystem>();
                    }
                    else
                    {
                        roadSystems = MicroVerse.instance.GetComponentsInChildren<RoadSystem>();
                    }
                    RoadSystem rs = null;
                    foreach (var crs in roadSystems)
                    {
                        if (!string.IsNullOrEmpty(crs.contentID) && crs.contentID == preset.collection.id)
                        {
                            rs = crs;
                            break;
                        }
                    }
                    if (rs == null)
                    {
                        GameObject go = new GameObject("Road System");
                        rs = go.AddComponent<RoadSystem>();
                        rs.contentID = preset.collection.id;
                        if (MicroVerse.instance != null)
                        {
                            rs.transform.SetParent(MicroVerse.instance.transform);
                        }
                        rs.transform.localPosition = Vector3.zero;
                        rs.transform.localScale = Vector3.one;
                        rs.transform.localRotation = Quaternion.identity;
                    }
                    instance.transform.SetParent(rs.transform);
                    rs.UpdateMaterialOverrides();
#endif
                }
                else
                {
                    instance.transform.SetParent(MicroVerse.instance?.transform);
                }
                MicroVerse.instance?.Invalidate(null); // TODO: Do better?
            }

            return instance;
        }


        private void DragFinished()
        {
#if __MICROVERSE_ROADS__
            GameObject go = DragAndDrop.objectReferences[0] as GameObject;
            if (go != null)
            {
                SplineRelativeTransform srt = go.GetComponent<SplineRelativeTransform>();
                if (srt != null)
                {
                    RoadSystem[] rss = GameObject.FindObjectsOfType<RoadSystem>();
                    if (rss != null)
                    {
                        RoadSystem roadSystem = null;
                        Road closest = null;
                        float closestDist = 9999999;
                        foreach (var rs in rss)
                        {
                            Road[] roads = rs.GetComponentsInChildren<Road>();
                            foreach (var road in roads)
                            {
                                if (road.splineContainer != null && road.splineContainer.Splines.Count > 0)
                                {
                                    var spline = road.splineContainer.Spline;
                                    var dist = SplineUtility.GetNearestPoint(spline, road.splineContainer.transform.worldToLocalMatrix.MultiplyPoint( go.transform.position), out _, out _);
                                    if (dist < closestDist)
                                    {
                                        closest = road;
                                        closestDist = dist;
                                        roadSystem = rs;
                                    }
                                }
                            }
                        }

                        if (closest != null && closestDist < 30)
                        {
                            srt.splineContainer = closest.splineContainer;
                            srt.transform.SetParent(roadSystem.transform, true);
                            srt.CaptureOffset();
                            srt.enabled = true;
                        }
                    }
                }
            }
#endif

            MicroVerse.instance?.Invalidate(null); // TODO : Do Better?
        }
    }
}