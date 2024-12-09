using UnityEngine;
using DG.Tweening;

namespace SmallTown {
public class Bullet : MonoBehaviour {

	// Private variables
	[SerializeField] private GameObject graphics;
	[SerializeField] private TrailRenderer trailRenderer;

	// Public variables
	public bool Active { get; private set; }


	// ************************************************************************************************************ CUSTOM FUNCTIONS: TARGET LINE

	public void Setup(CharacterHit hit) {
		Active = true;
		transform.position = hit.StartPosition;
		transform.LookAt(hit.EndPosition);
		trailRenderer.Clear();
		trailRenderer.time = GameManager.Config.Item.BulletTrail;
		graphics.SetActive(true);
		float duration = Vector3.Distance(hit.StartPosition, hit.EndPosition) / GameManager.Config.Item.BulletSpeed;
		transform.DOMove(hit.EndPosition, duration).SetEase(Ease.Linear).OnComplete(()=> Finished(hit));
	}

	void Finished(CharacterHit hit) {
		hit.Controller?.Hit(hit);
		trailRenderer.Clear();
		graphics.SetActive(false);
		Active = false;
	}
}
}