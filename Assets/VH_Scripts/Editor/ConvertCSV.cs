using System;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace SmallTown {
public class ConvertCSV {

	// Variables
	// private static int _index;


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	// static string[] GetLines(string datapath) {
	// 	 return File.ReadAllLines(Application.dataPath + datapath);
	// }

	// static string Text(string[] lines, bool reset = false) {
	// 	return lines[Index(reset)];
	// }

	// static int Value(string[] lines, bool reset = false) {
	// 	string text = lines[Index(reset)];
	// 	if (text == "") {
	// 		return 0;
	// 	}
	// 	return int.Parse(text);
	// }

	// static int Index(bool reset) {
	// 	if (reset) {
	// 		_index = 0;
	// 	}
	// 	else {
	// 		_index++;
	// 	}
	// 	return _index;
	// }


	// // ************************************************************************************************************ CUSTOM FUNCTIONS: WEAPONS

	// [MenuItem("Tools/Taival/Generate Weapons", false, -50)]
	// public static void GenerateWeapons() {
	// 	string[] lines = GetLines("/VH_Config/Editor/Weapons.csv");
	// 	ItemWeapon weapon = null;
	// 	bool create = false;
	// 	for (int i = 1; i < lines.Length; i++) {
	// 		string[] split = lines[i].Split(',');
	// 		if (System.IO.File.Exists(Application.dataPath + "/VH_Config/Items/Weapons/" + split[0] + ".asset")) {
	// 			weapon = (ItemWeapon)AssetDatabase.LoadAssetAtPath("Assets/VH_Config/Items/Weapons/" + split[0] + ".asset", typeof(ItemWeapon));
	// 		}
	// 		else {
	// 			weapon = ScriptableObject.CreateInstance<ItemWeapon>();
	// 			create = true;
	// 		}
	// 		weapon.ItemType = ItemTypes.Weapon;
	// 		if (weapon.ID == "") {
	// 			weapon.ID = Guid.NewGuid().ToString();
	// 		}
	// 		weapon.LocalizationKey = "Weapons/" + Text(split, true);
	// 		weapon.StackLimit = Value(split);
	// 		weapon.Weight = Value(split);
	// 		weapon.Size = new Vector2Int(Value(split), Value(split));
	// 		weapon.WeaponType = GetWeaponType(Text(split));
	// 		weapon.Damage = Value(split);
	// 		weapon.Range = Value(split);
	// 		weapon.StaminaPerHit = Value(split);
	// 		weapon.ReloadTime = Value(split);
	// 		weapon.Noise = Value(split);
	// 		weapon.Firerate = Value(split);
	// 		weapon.SingleShot = Text(split) != string.Empty;
	// 		weapon.BurstShot = Text(split) != string.Empty;
	// 		weapon.AutoShot = Text(split) != string.Empty;
	// 		if(create) {
	// 			AssetDatabase.CreateAsset(weapon, "Assets/VH_Config/Items/Weapons/" + split[0] + ".asset");
	// 		}
	// 		EditorUtility.SetDirty(weapon);
	// 	}
	// 	AssetDatabase.SaveAssets();
	// 	Debug.Log("Generated weapons");
	// }

	// static WeaponTypes GetWeaponType(string type) {
	// 	foreach(WeaponTypes weapon in Enum.GetValues(typeof(WeaponTypes))) {
	// 		if (weapon.ToString().ToLower() == type.ToLower()) {
	// 			return weapon;
	// 		}
	// 	}
	// 	return WeaponTypes.None;
	// }


	// // ************************************************************************************************************ CUSTOM FUNCTIONS: WEAPON EXTENSIONS

	// [MenuItem("Tools/Taival/Generate Weapon Extensions", false, -49)]
	// public static void GenerateWeaponExtensions() {
	// 	string[] lines = GetLines("/VH_Config/Editor/WeaponsExtensions.csv");
	// 	ItemWeaponPart weaponPart = null;
	// 	bool create = false;
	// 	for (int i = 1; i < lines.Length; i++) {
	// 		string[] split = lines[i].Split(',');
	// 		if (System.IO.File.Exists(Application.dataPath + "/VH_Config/Items/WeaponExtensions/" + split[0] + ".asset")) {
	// 			weaponPart = (ItemWeaponPart)AssetDatabase.LoadAssetAtPath("Assets/VH_Config/Items/WeaponExtensions/" + split[0] + ".asset", typeof(ItemWeaponPart));
	// 		}
	// 		else {
	// 			weaponPart = ScriptableObject.CreateInstance<ItemWeaponPart>();
	// 			create = true;
	// 		}
	// 		weaponPart.ItemType = ItemTypes.WeaponExtension;
	// 		if (weaponPart.ID == "") {
	// 			weaponPart.ID = Guid.NewGuid().ToString();
	// 		}
	// 		weaponPart.LocalizationKey = "WeaponExtensions/" + Text(split, true);
	// 		weaponPart.StackLimit = Value(split);
	// 		weaponPart.Weight = Value(split);
	// 		weaponPart.Size = new Vector2Int(Value(split), Value(split));
	// 		weaponPart.Type = GetWeaponExtensionsType(Text(split));
	// 		weaponPart.Damage = Value(split);
	// 		weaponPart.NoiseReduction = Value(split);
	// 		weaponPart.ZoomAmount = Value(split);
	// 		weaponPart.ZoomSpeed = Value(split);
	// 		weaponPart.AmmoCount = Value(split);
	// 		weaponPart.ReloadTime = Value(split);
	// 		if(create) {
	// 			AssetDatabase.CreateAsset(weaponPart, "Assets/VH_Config/Items/WeaponExtensions/" + split[0] + ".asset");
	// 		}
	// 		EditorUtility.SetDirty(weaponPart);
	// 	}
	// 	AssetDatabase.SaveAssets();
	// 	Debug.Log("Generated Weapon Extensions");
	// }

	// static WeaponPartTypes GetWeaponExtensionsType(string type) {
	// 	foreach(WeaponPartTypes weaponExtension in Enum.GetValues(typeof(WeaponPartTypes))) {
	// 		if (weaponExtension.ToString().ToLower() == type.ToLower()) {
	// 			return weaponExtension;
	// 		}
	// 	}
	// 	return WeaponPartTypes.None;
	// }


	// // ************************************************************************************************************ CUSTOM FUNCTIONS: CLOTHES

	// [MenuItem("Tools/Taival/Generate Clothes", false, -48)]
	// public static void GenerateClothes() {
	// 	string[] lines = GetLines("/VH_Config/Editor/Clothes.csv");
	// 	ItemClothes clothes = null;
	// 	bool create = false;
	// 	for (int i = 1; i < lines.Length; i++) {
	// 		string[] split = lines[i].Split(',');
	// 		if (System.IO.File.Exists(Application.dataPath + "/VH_Config/Items/Clothes/" + split[0] + ".asset")) {
	// 			clothes = (ItemClothes)AssetDatabase.LoadAssetAtPath("Assets/VH_Config/Items/Clothes/" + split[0] + ".asset", typeof(ItemClothes));
	// 		}
	// 		else {
	// 			clothes = ScriptableObject.CreateInstance<ItemClothes>();
	// 			create = true;
	// 		}
	// 		clothes.ItemType = ItemTypes.Clothes;
	// 		if (clothes.ID == "") {
	// 			clothes.ID = Guid.NewGuid().ToString();
	// 		}
	// 		clothes.LocalizationKey = "Clothes/" + Text(split, true);
	// 		clothes.StackLimit = Value(split);
	// 		clothes.Weight = Value(split);
	// 		clothes.Size = new Vector2Int(Value(split), Value(split));
	// 		clothes.Type = GetClothesType(Text(split));
	// 		clothes.Slots = new Vector2Int(Value(split), Value(split));
	// 		clothes.Heat = Value(split);
	// 		clothes.Armour = Value(split);
	// 		if(create) {
	// 			AssetDatabase.CreateAsset(clothes, "Assets/VH_Config/Items/Clothes/" + split[0] + ".asset");
	// 		}
	// 		EditorUtility.SetDirty(clothes);
	// 	}
	// 	AssetDatabase.SaveAssets();
	// 	Debug.Log("Generated clothes");
	// }

	// static ClothesTypes GetClothesType(string type) {
	// 	foreach(ClothesTypes clothes in Enum.GetValues(typeof(ClothesTypes))) {
	// 		if (clothes.ToString().ToLower() == type.ToLower()) {
	// 			return clothes;
	// 		}
	// 	}
	// 	return ClothesTypes.None;
	// }


	// // ************************************************************************************************************ CUSTOM FUNCTIONS: CONSUMABLES

	// [MenuItem("Tools/Taival/Generate Consumables", false, -47)]
	// public static void GenerateConsumables() {
	// 	string[] lines = GetLines("/VH_Config/Editor/Consumables.csv");
	// 	ItemConsumable consumable = null;
	// 	bool create = false;
	// 	for (int i = 1; i < lines.Length; i++) {
	// 		string[] split = lines[i].Split(',');
	// 		if (System.IO.File.Exists(Application.dataPath + "/VH_Config/Items/Consumables/" + split[0] + ".asset")) {
	// 			consumable = (ItemConsumable)AssetDatabase.LoadAssetAtPath("Assets/VH_Config/Items/Consumables/" + split[0] + ".asset", typeof(ItemConsumable));
	// 		}
	// 		else {
	// 			consumable = ScriptableObject.CreateInstance<ItemConsumable>();
	// 			create = true;
	// 		}
	// 		consumable.ItemType = ItemTypes.Consumable;
	// 		if (consumable.ID == "") {
	// 			consumable.ID = Guid.NewGuid().ToString();
	// 		}
	// 		consumable.LocalizationKey = "Consumables/" + Text(split, true);
	// 		consumable.StackLimit = Value(split);
	// 		consumable.Weight = Value(split);
	// 		consumable.Size = new Vector2Int(Value(split), Value(split));
	// 		consumable.Health = Value(split);
	// 		consumable.Stamina = Value(split);
	// 		consumable.Food = Value(split);
	// 		consumable.Water = Value(split);
	// 		if(create) {
	// 			AssetDatabase.CreateAsset(consumable, "Assets/VH_Config/Items/Consumables/" + split[0] + ".asset");
	// 		}
	// 		EditorUtility.SetDirty(consumable);
	// 	}
	// 	AssetDatabase.SaveAssets();
	// 	Debug.Log("Generated consumables");
	// }
}
}