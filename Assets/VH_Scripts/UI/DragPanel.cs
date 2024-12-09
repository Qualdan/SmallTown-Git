using UnityEngine;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour, IDragHandler {

	// Private variables
	[SerializeField] private Transform moveTarget;


	// ************************************************************************************************************ UNITY FUNCTIONS

	public void OnDrag(PointerEventData eventData) {
		// this.transform.position += (Vector3)eventData.delta;
		moveTarget.position += (Vector3)eventData.delta;
	}
}