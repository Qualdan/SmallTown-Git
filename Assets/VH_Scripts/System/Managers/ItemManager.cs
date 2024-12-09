using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SmallTown {
public class ItemManager : MonoBehaviour, IManager {

	// Private variables
	[SerializeField] private Transform instantiatedParent;
	[SerializeField] private Transform reParent;
	[SerializeField] private Bullet bulletPrefab;
	[SerializeField] private MuzzleFlash muzzleFlashPrefab;
	private List<PickableItem> _pickableItems = new List<PickableItem>();
	private List<ItemHighlighter> _itemHighlighters = new List<ItemHighlighter>();
	private List<Bullet> _bullets = new List<Bullet>();
	private List<MuzzleFlash> _muzzleFlashes = new List<MuzzleFlash>();

	// Public variables
	public bool Initialized { get; private set; }


	// ************************************************************************************************************ UNITY FUNCTIONS

	void OnDestroy() {
		if(GameManager.HasInstance) {
			GameManager.Input.Actions.Player.Highlight.performed -= ctx => ItemHighlight(true);
			GameManager.Input.Actions.Player.Highlight.canceled -= ctx => ItemHighlight(false);
			GameManager.Input.Actions.Camera.Highlight.performed -= ctx => ItemHighlight(true);
			GameManager.Input.Actions.Camera.Highlight.canceled -= ctx => ItemHighlight(false);
		}
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {
		GameManager.Input.Actions.Player.Highlight.performed += ctx => ItemHighlight(true);
		GameManager.Input.Actions.Player.Highlight.canceled += ctx => ItemHighlight(false);
		GameManager.Input.Actions.Camera.Highlight.performed += ctx => ItemHighlight(true);
		GameManager.Input.Actions.Camera.Highlight.canceled += ctx => ItemHighlight(false);

		GatherItems();

		Initialized = true;
		Helpers.Initialized(GetType().Name);
	}


	void GatherItems() {
		PickableItem[] foundItems = FindObjectsOfType<PickableItem>();
		_pickableItems.AddRange(foundItems);

		ItemHighlighter[] foundHighlighters = FindObjectsOfType<ItemHighlighter>();
		_itemHighlighters.AddRange(foundHighlighters);

		for (int i = 0; i < _itemHighlighters.Count; i++) {
			_itemHighlighters[i].Setup();
		}
	}

	void ItemHighlight(bool toggle) {
		for (int i = 0; i < _itemHighlighters.Count; i++) {
			_itemHighlighters[i].Toggle(toggle);
		}
	}

	public void Reparent(Transform item) {
		item.SetParent(reParent);
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: BULLETS

	public void ShootBullet(CharacterHit hit) {
		// if(hit.Controller != null) {
		// 	Debug.LogWarning("************************ >>> HIT ENEMY");
		// }

		Bullet bullet = GetBullet();
		if(bullet != null) {
			bullet.Setup(hit);
		}
		else {
			Debug.LogError("Couldn't get bullet!");
		}
	}


	Bullet GetBullet() {
		for (int i = 0; i < _bullets.Count; i++) {
			if(!_bullets[i].Active) {
				Helpers.Log(GetType().Name, "GetBullet()", "Returned previous bullet", LogStates.Extended);
				return _bullets[i];
			}
		}
		Helpers.Log(GetType().Name, "GetBullet()", "Instantiated new bullet", LogStates.Extended);
		Bullet bullet = Instantiate(bulletPrefab, instantiatedParent);
		_bullets.Add(bullet);
		return bullet;
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: MUZZLE FLASH

	public void ShowMuzzleFlash(Vector3 position) {
		MuzzleFlash muzzleFlash = GetMuzzleFlash();
		if(muzzleFlash != null) {
			muzzleFlash.Setup(position);
		}
		else {
			Debug.LogError("Couldn't get muzzle flash!");
		}
	}

	MuzzleFlash GetMuzzleFlash() {
		for (int i = 0; i < _muzzleFlashes.Count; i++) {
			if(!_muzzleFlashes[i].gameObject.activeSelf) {
				Helpers.Log(GetType().Name, "GetMuzzleFlash()", "Returned previous muzzle flash", LogStates.Extended);
				return _muzzleFlashes[i];
			}
		}
		Helpers.Log(GetType().Name, "GetMuzzleFlash()", "Instantiated muzzle flash", LogStates.Extended);
		MuzzleFlash muzzleFlash = Instantiate(muzzleFlashPrefab, instantiatedParent);
		_muzzleFlashes.Add(muzzleFlash);
		return muzzleFlash;
	}




	// // ************************************************************************************************************ CUSTOM FUNCTIONS: PICKABLE ITEMS

	// public void AddItem(PickableItem item) {
	// 	if (!_pickableItems.Contains(item)) {
	// 		_pickableItems.Add(item);
	// 	}
	// }

	// public void RemoveItem(PickableItem item) {
	// 	if (_pickableItems.Contains(item)) {
	// 		_pickableItems.Remove(item);
	// 	}
	// }



	// // ************************************************************************************************************ CUSTOM FUNCTIONS: ITEM HIGHLIGHTERS

	// public void AddItemHighlighter(ItemHighlighter item) {
	// 	if (!_itemHighlighters.Contains(item)) {
	// 		_itemHighlighters.Add(item);
	// 	}
	// }

	// public void RemoveItemHighlighter(ItemHighlighter item) {
	// 	if (_itemHighlighters.Contains(item)) {
	// 		_itemHighlighters.Remove(item);
	// 	}
	// }

}
}