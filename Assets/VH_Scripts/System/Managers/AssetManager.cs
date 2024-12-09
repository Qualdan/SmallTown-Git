using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SmallTown {
public class AssetManager : MonoBehaviour, IManager {

	// Variables
	// private List<AreaObject> _areaTypes = new List<AreaObject>();
	// private List<Item> _items = new List<Item>();
	private int initializeCount = 0;
	private int initializeNeed = 2;

	// Public variables
	public bool Initialized { get; private set; }


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {
		// Addressables.InitializeAsync().Completed += AssetsLoaded;
	}

	void Initialize() {
		initializeCount++;
		if(initializeCount >= initializeNeed) {
			Initialized = true;
			Helpers.Initialized(GetType().Name);
		}
	}

	void AssetsLoaded(AsyncOperationHandle<IResourceLocator> obj) {
		Helpers.Log(GetType().Name, "Loaded asset", obj.DebugName);
		// Addressables.LoadAssetsAsync<AreaObject>("AreaScriptable", obj => { _areaTypes.Add(obj); }).Completed += OnAreaTypesLoaded;
		// Addressables.LoadAssetsAsync<Item>("ItemScriptable", obj => { _items.Add(obj); }).Completed += OnItemsLoaded;
		// Addressables.LoadAssetsAsync<Item>("ItemScriptable", obj => { _items.Add(obj); }).Completed += OnItemsLoaded;
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: AREAS

	// private void OnAreaTypesLoaded(AsyncOperationHandle<IList<AreaObject>> handle) {
	// 	if (handle.Status == AsyncOperationStatus.Succeeded) {
	// 		Addressables.Release<IList<AreaObject>>(handle);
	// 		Helpers.Log(GetType().Name, "Loaded area types", handle.Status.ToString());
	// 	}
	// 	else if (handle.Status == AsyncOperationStatus.Failed) {
	// 		Helpers.LogError(GetType().Name, "Couldn't load area types", handle.Status.ToString());
	// 	}
	// 	Initialize();
	// }

	// public AreaObject GetAreaType(AreaTypes area) {
	// 	for (int i = 0; i < _areaTypes.Count; i++) {
	// 		if(_areaTypes[i].Area == area) {
	// 			Helpers.Log(GetType().Name, "Returned area type", area.ToString(), LogStates.Extended);
	// 			return _areaTypes[i];
	// 		}
	// 	}
	// 	Helpers.LogError(GetType().Name, "Couldn't return area type", area.ToString());
	// 	return null;
	// }


	// ************************************************************************************************************ CUSTOM FUNCTIONS: Items

	// private void OnItemsLoaded(AsyncOperationHandle<IList<Item>> handle) {
	// 	if (handle.Status == AsyncOperationStatus.Succeeded) {
	// 		Addressables.Release<IList<Item>>(handle);
	// 		Helpers.Log(GetType().Name, "Loaded items", handle.Status.ToString());
	// 	}
	// 	else if (handle.Status == AsyncOperationStatus.Failed) {
	// 		Helpers.LogError(GetType().Name, "Couldn't load items", handle.Status.ToString());
	// 	}
	// 	Initialize();
	// }

	// public Item GetItem(string id) {
	// 	for (int i = 0; i < _items.Count; i++) {
	// 		if (_items[i].ID == id) {
	// 			return _items[i];
	// 		}
	// 	}
	// 	Helpers.LogError(GetType().Name, "Couldn't return area type", id.ToString());
	// 	return null;
	// }



}
}