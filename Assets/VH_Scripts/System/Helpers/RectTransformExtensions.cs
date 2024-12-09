using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace SmallTown {
public static class RectTransformExtensions {


	// ************************************************************************************************************ EXTENSIONS

	public static Vector2 MatchPosition(this RectTransform to, RectTransform from) {
		Vector2 localPoint;
		Vector2 fromPivotDerivedOffset = new Vector2(from.rect.width * from.pivot.x + from.rect.xMin, from.rect.height * from.pivot.y + from.rect.yMin);
		Vector2 screenP = RectTransformUtility.WorldToScreenPoint(null, from.position);
		screenP += fromPivotDerivedOffset;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(to, screenP, null, out localPoint);
		Vector2 pivotDerivedOffset = new Vector2(to.rect.width * to.pivot.x + to.rect.xMin, to.rect.height * to.pivot.y + to.rect.yMin);
		return to.anchoredPosition + localPoint - pivotDerivedOffset;
	}
}
}