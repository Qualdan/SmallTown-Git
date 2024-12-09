using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

namespace SmallTown {
[CreateAssetMenu(fileName = "New Weapon", menuName = "SmallTown/Weapon")]
public class WeaponData : ScriptableObject {

	public WeaponTypes Type;
	public float PlayerAttackSpeed;
	public float EnemyAttackSpeed;
	public GameObject physicalObject;
}
}