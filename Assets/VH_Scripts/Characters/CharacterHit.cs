using UnityEngine;

namespace SmallTown {
public class CharacterHit {

	// Public variables
	public ICharacter Controller;
	public GameObject Source;
	public WeaponTypes WeaponType;
	public Vector3 Direction { get { return EndPosition - StartPosition; } }
	public Vector3 StartPosition;
	public Vector3 EndPosition;


	// ************************************************************************************************************ CONSTRUCTORS

	public CharacterHit(ICharacter newController, GameObject newSource, WeaponTypes newWeaponType, Vector3 newStartPosition, Vector3 newEndPosition) {
		Controller = newController;
		Source = newSource;
		WeaponType = newWeaponType;
		StartPosition = newStartPosition;
		EndPosition = newEndPosition;
	}
}
}