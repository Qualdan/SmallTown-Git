using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace SmallTown {
public class TooltipPanel : MonoBehaviour {

	// Objects
	[SerializeField] private CanvasGroup canvasGroup;
	[SerializeField] private RectTransform tooltip;
	[SerializeField] private TextMeshProUGUI titleText;
	[SerializeField] private TextMeshProUGUI detailsText;
	[SerializeField] private GameObject separator;


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup(string title, string details, Vector3 position) {
		Vector3 targetPosition = Helpers.WorldToUI(GameManager.Gui.Hud.Canvas, position);
		bool left = targetPosition.x < Screen.width  / 2.0f;
		bool bottom = targetPosition.y < Screen.height / 2.0f;
		float x = left ? 0.0f : 1.0f;
		float y = bottom ? 0.0f : 1.0f;
		Vector3 offset = GameManager.Config.UI.TooltipOffset;
		if (!left) {
			offset.x = -offset.x;
		}
		if (!bottom) {
			offset.y = -offset.y;
		}
		tooltip.pivot = new Vector2(x, y);
		tooltip.position = targetPosition + offset;

		if(!string.IsNullOrEmpty(title)) {
			titleText.SetText(title);
			titleText.gameObject.SetActive(true);
			// separator.SetActive(true);
		}
		else {
			titleText.gameObject.SetActive(false);
			// separator.SetActive(false);
		}
		detailsText.SetText(details);

		canvasGroup.DOComplete();
		canvasGroup.DOFade(1.0f, GameManager.Config.UI.TooltipDuration);
	}

	public void Hide() {
		canvasGroup.DOComplete();
		canvasGroup.DOFade(0.0f, GameManager.Config.UI.TooltipDuration);
	}
}
}