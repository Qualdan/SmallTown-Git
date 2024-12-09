using System.Collections.Generic;
using UnityEngine;

namespace SmallTown {
[CreateAssetMenu(fileName = "New Enemy", menuName = "SmallTown/Enemy")]
public class EnemyData : ScriptableObject {

	public EnemyFactions Faction;
	public string Key;
	public int Health;
	public float MovementSpeed;
	public bool LookingForPlayer;
	public float DetectionDistance;
	public float DetectionAngle;
}
}