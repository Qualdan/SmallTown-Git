using UnityEngine;
using DG.Tweening;

namespace SmallTown {
public class EnemyWeapon : MonoBehaviour {

	// Private variables
	[SerializeField] private Rigidbody body;
	// [SerializeField] private TrailRenderer trailRenderer;

	// Public variables
	// public bool Active { get; private set; }


	// ************************************************************************************************************ CUSTOM FUNCTIONS: TARGET LINE

	public void Detach() {
		GameManager.Area.Level.Item.Reparent(transform);
		body.isKinematic = false;
		body.AddForce(Vector3.up * 10.0f, ForceMode.Impulse);
	}



	// public void Setup(Vector3 startPosition, Vector3 endPosition, EnemyController controller) {
	// 	Active = true;
	// 	graphics.SetActive(true);
	// 	trailRenderer.time = GameManager.Config.Item.BulletTrail;
	// 	transform.position = startPosition;
	// 	transform.LookAt(endPosition);
	// 	float duration = Vector3.Distance(startPosition, endPosition) / GameManager.Config.Item.BulletSpeed;
	// 	transform.DOMove(endPosition, duration).SetEase(Ease.Linear).OnComplete(()=> Finished(startPosition, endPosition, controller));
	// }

	// void Finished(Vector3 startPosition, Vector3 endPosition, EnemyController controller) {
	// 	controller?.Hit(startPosition, endPosition);
	// 	trailRenderer.Clear();
	// 	trailRenderer.time = 0.0f;
	// 	graphics.SetActive(false);
	// 	Active = false;
	// }
}
}