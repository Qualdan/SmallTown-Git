using UnityEngine;
using UnityEngine.EventSystems;

namespace SmallTown {
public class EnemyTargeting : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	// Private variables
	[SerializeField] private EnemyController controller;


	// ************************************************************************************************************ UNITY FUNCTIONS

	public void OnPointerEnter(PointerEventData eventData) {
		GameManager.Player.Actions.TargetEnemy(controller);
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData) {
		GameManager.Player.Actions.ResetEnemy();
	}
}
}