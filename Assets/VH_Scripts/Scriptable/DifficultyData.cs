using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

namespace SmallTown {
[CreateAssetMenu(fileName = "New Difficulty", menuName = "SmallTown/Difficulty")]
public class DifficultyData : ScriptableObject {

	[Tooltip("Difficylty level")]
	public Difficulties Level;

	[Header("Enemy")]
	[Tooltip("How fast targeting circle will contract")]
	public float TargetingSpeed;
	[Tooltip("How many bullets need to be fired for there to be a chance to hit")]
	public float HitRatio;
	[Tooltip("What is the actual chance that enemy will hit player")]
	public float HitChance;
}
}