using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SmallTown {
public class BuildingController : MonoBehaviour {

	// Private variables
	[SerializeField] private List<GameObject> floors = new List<GameObject>();
	private List<int> playerFloors = new List<int>();

	private Coroutine _delayRoutine;


	// ************************************************************************************************************ CUSTOM FUNCTIONS: TARGET LINE

	public void Entered(int floor) {
		playerFloors.Add(floor);
		// CheckFloors();
		Delay();
		GameManager.Camera.ForceView(true);
	}

	public void Exited(int floor) {
		playerFloors.Remove(floor);
		// CheckFloors();
		Delay();
		GameManager.Camera.ForceView(false);
	}

	void Delay() {
		if(_delayRoutine != null) {
			StopCoroutine(_delayRoutine);
		}
		_delayRoutine = StartCoroutine(CheckDelay());
	}

	IEnumerator CheckDelay() {
		yield return new WaitForSeconds(GameManager.Config.Effects.EaseOutDuration);
		yield return new WaitForEndOfFrame();
		for (int i = 0; i < floors.Count; i++) {
			floors[i].transform.DOKill();
		}
		CheckFloors();
	}


	void CheckFloors() {
		int highestFloor = -1;
		for (int i = 0; i < playerFloors.Count; i++) {
			if (playerFloors[i] > highestFloor) {
				highestFloor = playerFloors[i];
			}
		}

		// Debug.LogWarning("******* Highest floor is " + highestFloor);

		for (int i = 0; i < floors.Count; i++) {
			if (highestFloor >= 0) {
				bool show = i <= highestFloor;
				// floors[i].SetActive(show);
				ToggleFloor(floors[i], show);
				// ToggleShadowMode(floors[i], show);
			}
			else {
				// floors[i].SetActive(true);
				ToggleFloor(floors[i], true);
				// ToggleShadowMode(floors[i], true);
			}
		}
	}


	void ToggleFloor(GameObject floor, bool toggle) {
		if(toggle) {
			floor.SetActive(true);
		}
		Transform form = floor.transform;
		Vector3 scale = new Vector3(1.0f, toggle ? 1.0f : 0.0f, 1.0f);
		float duration = toggle ? GameManager.Config.Effects.EaseInDuration : GameManager.Config.Effects.EaseOutDuration;
		// float duration = GameManager.Config.Effects.EaseOutDuration;
		Ease ease = toggle ? GameManager.Config.Effects.EaseIn : GameManager.Config.Effects.EaseOut;

		// int kills = form.DOKill();
		// Debug.LogWarning("Kills " + kills);
		// DOTween.KillAll();
		DOTween.To(()=> form.localScale, x=> form.localScale = x, scale, duration).SetEase(ease).OnComplete(()=> OnComplete(floor, toggle));

	}

	void OnComplete(GameObject floor, bool toggle) {

		// Debug.LogWarning("Setting floor '" + floor + "' to " + toggle);

		floor.SetActive(toggle);
	}
}
}