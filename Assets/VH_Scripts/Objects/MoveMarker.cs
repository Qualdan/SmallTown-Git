using UnityEngine;
using DG.Tweening;

namespace SmallTown {
public class MoveMarker : MonoBehaviour {

	// Private variables
	[SerializeField] private SpriteRenderer markerSprite;
	[SerializeField] private SpriteRenderer denySprite;


	// ************************************************************************************************************ UNITY FUNCTIONS

	void Update() {
		RotateMarker();
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: MARKER

	void RotateMarker() {
		if(!GameManager.HasInstance || !GameManager.Player.Exists) { return; }
		markerSprite.transform.Rotate(Vector3.forward, (int)GameManager.Player.Controller.PlayerState * 50.0f * Time.deltaTime);
	}

	public void Show(Vector3 position) {
		markerSprite.DOComplete();
		markerSprite.DOFade(1.0f, GameManager.Config.Player.MarkerFadeDuration);
		markerSprite.transform.position = position;
	}

	public void Hide() {
		markerSprite.DOComplete();
		markerSprite.DOFade(0.0f, GameManager.Config.Player.MarkerFadeDuration);
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: POINTER

	public void Deny(Vector3 position) {
		denySprite.transform.position = position;
		denySprite.DOKill();
		denySprite.DOFade(1.0f, GameManager.Config.Player.DenyFadeDuration).OnComplete(DenyHide);
	}

	public void DenyHide() {
		denySprite.DOFade(0.0f, GameManager.Config.Player.DenyFadeDuration);
	}
}
}