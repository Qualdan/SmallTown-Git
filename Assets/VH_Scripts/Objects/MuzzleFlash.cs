using UnityEngine;
using DG.Tweening;

namespace SmallTown {
public class MuzzleFlash : MonoBehaviour {

	// Private variables
	[SerializeField] private Light pointLight;


	// ************************************************************************************************************ CUSTOM FUNCTIONS: TARGET LINE

	public void Setup(Vector3 position) {
		gameObject.SetActive(true);
		transform.position = position;
		pointLight.DOIntensity(2.0f, 0.5f).OnComplete(HideLight);
	}

	void HideLight() {
		pointLight.DOIntensity(0.0f, 0.5f).OnComplete(Finished);
	}

	void Finished() {
		gameObject.SetActive(false);
	}

}
}