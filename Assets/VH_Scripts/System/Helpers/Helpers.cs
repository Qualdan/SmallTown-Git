using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

namespace SmallTown {
public static class Helpers {


	// ************************************************************************************************************ CUSTOM FUNCTIONS: SCENES




	// ************************************************************************************************************ CUSTOM FUNCTIONS: NAVIGATION

	public static bool CanSee(Vector3 target) {
		return !Physics.Linecast(GameManager.Player.CameraTarget, target, ~GameManager.Config.General.CharacterMask, QueryTriggerInteraction.Ignore);
	}

	public static bool CanReach(Vector3 target, NavMeshAgent agent) {
		NavMeshPath path = new NavMeshPath();
		if(agent.CalculatePath(target, path) && path.status == NavMeshPathStatus.PathComplete) {
			return true;
		}
		return false;
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: POSITIONING

    // public static Vector2 CalculateTooltipPosition(RectTransform parentRT, RectTransform popupRT)
    // {
    //     var canvasRT = (RectTransform)GameManager.UIManager.popupCanvas;
    //     var corners = new Vector3[4];
    //     parentRT.GetWorldCorners(corners);
    //     var anchor = (corners[2] + corners[3]) / 2f;
    //     var screenAnchor = RectTransformUtility.WorldToScreenPoint(null, anchor);
    //     var localAnchor = new Vector2((screenAnchor.x / Screen.width) * canvasRT.rect.width, (screenAnchor.y / Screen.height) * canvasRT.rect.height);

    //     var size = popupRT.rect.size;
    //     float minX = size.x * popupRT.pivot.x;
    //     float maxX = canvasRT.rect.width - (size.x * (1f - popupRT.pivot.x));
    //     float minY = size.y * popupRT.pivot.y;
    //     float maxY = canvasRT.rect.height - (size.y * (1f - popupRT.pivot.y));
    //     float screenEdgeMargin = 8f;
    //     localAnchor.x = Mathf.Clamp(localAnchor.x, minX + screenEdgeMargin, maxX - screenEdgeMargin);
    //     localAnchor.y = Mathf.Clamp(localAnchor.y, minY + screenEdgeMargin, maxY - screenEdgeMargin);
    //     return localAnchor;
    // }




	public static Vector3 WorldToUI(Canvas parentCanvas, Vector3 worldPos) {
		Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
		Vector2 movePos = Vector2.zero;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
		return parentCanvas.transform.TransformPoint(movePos);
	}

	public static bool PlayerDistance(Transform target, float distance) {
		return Vector3.Distance(GameManager.Player.Position, target.position) <= distance;
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: POST PROCESSING

	public static void TogglePostProcessing(PostProcessVolume volume, GameStates checkState, GameStates currentState) {
		volume.weight = currentState == checkState ? 1.0f : 0.0f;
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: CALCULATIONS

	public static float Remap(float value, float min1, float max1, float min2, float max2) {
		return (value - min1) / (max1 - min1) * (max2 - min2) + min2;
	}

	public static float ClampAngle(float angle, float min, float max){
		angle = angle % 360;
		if((angle >= -360.0f) && (angle <= 360.0f)){
			if(angle < -360.0f){
				angle += 360.0f;
			}
			if(angle > 360.0f){
				angle -= 360.0f;
			}
		}
		return Mathf.Clamp(angle, min, max);
	}

	public static Vector3 AngleLerp(Vector3 StartAngle, Vector3 FinishAngle, float t) {
		float xLerp = Mathf.LerpAngle(StartAngle.x, FinishAngle.x, t);
		float yLerp = Mathf.LerpAngle(StartAngle.y, FinishAngle.y, t);
		float zLerp = Mathf.LerpAngle(StartAngle.z, FinishAngle.z, t);
		return new Vector3(xLerp, yLerp, zLerp);
	}

	public static Vector3 SmoothedVector(Vector3 start, Vector3 end, float lerp) {
		float x = Mathf.SmoothStep(start.x, end.x, lerp);
		float y = Mathf.SmoothStep(start.y, end.y, lerp);
		float z = Mathf.SmoothStep(start.z, end.z, lerp);
		return new Vector3(x, y, z);
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: UI

	public static bool IsOnGUI() {
		return EventSystem.current.IsPointerOverGameObject();
	}

	public static Color Alpha(Color color, float alpha) {
		color.a = alpha;
		return color;
	}

	public static string ConvertColor(Color color) {
		return ColorUtility.ToHtmlStringRGBA(color);
	}

	public static string TextColor(string text, Color color) {
		return "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + text + "</color>";
	}





	// ************************************************************************************************************ CUSTOM FUNCTIONS: RAYCASTING

	// public static bool CheckDirection(Vector3 origin, Vector3 direction, float distance, float radius = 1.0f) {
	// 	RaycastHit[] hits = Physics.SphereCastAll(origin, radius, direction.normalized, distance, ~GameManager.Config.Player.mask, GameManager.Config.General.ignoreTriggers);
	// 	if (hits.Length > 0) {
	// 		if (GameManager.Config.Debug.logging) {
	// 			foreach(RaycastHit hit in hits) {
	// 				Helpers.Log("Direction check", "Towards " + direction + ")", hit.transform.name + " (tagged as " + hit.transform.tag + ")");
	// 			}
	// 		}
	// 		return true;
	// 	}
	// 	return false;
	// }

	// public static float CheckDistance(Vector3 origin, Vector3 direction) {
	// 	RaycastHit[] hits = Physics.RaycastAll(origin, direction.normalized, Mathf.Infinity, ~GameManager.Config.Player.mask, GameManager.Config.General.ignoreTriggers);
	// 	if (hits.Length > 0) {
	// 		if (GameManager.Config.Debug.logging) {
	// 			foreach(RaycastHit hit in hits) {
	// 				Helpers.Log("Distance check", "Towards " + direction + ")", hit.transform.name + " (tagged as " + hit.transform.tag + ")");
	// 			}
	// 		}
	// 		float savedDistance = Mathf.Infinity;
	// 		float distance = 0.0f;
	// 		for (int i = 0; i < hits.Length; i++) {
	// 			distance = Vector3.Distance(origin, hits[i].point);
	// 			if (savedDistance > distance) {
	// 				savedDistance = distance;
	// 			}
	// 		}
	// 		return savedDistance;
	// 	}
	// 	return -1;
	// }


	// ************************************************************************************************************ CUSTOM FUNCTIONS: RANDOM

	// public static bool GetRandomPosition(Vector3 center, float range, out Vector3 result) {
	// 	for (int i = 0; i < 30; i++) {
	// 		Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
	// 		NavMeshHit hit;
	// 		if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) {
	// 			result = hit.position;
	// 			return true;
	// 		}
	// 	}
	// 	result = Vector3.zero;
	// 	return false;
	// }

	public static bool Random(float compare, float max = 1.0f, float min = 0.0f) {
		float value = UnityEngine.Random.Range(min, max);
		return value >= compare;
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS: LOGGING

	public static void Log(string sender, string message, string value = "", LogStates logState = LogStates.Normal) {
		if (!CheckLogging(logState)) { return; }
		Debug.Log(ColorText(sender, GameManager.Config.Debug.ColorNormal) + message + ColorText(value, GameManager.Config.Debug.ColorValue));
	}

	public static void LogWarning(string sender, string message, string value = "", LogStates logState = LogStates.Normal) {
		if (!CheckLogging(logState)) { return; }
		Debug.LogWarning(ColorText(sender, GameManager.Config.Debug.ColorWarning) + message + ColorText(value, GameManager.Config.Debug.ColorValue));
	}

	public static void LogError(string sender, string message, string value = "", LogStates logState = LogStates.Normal) {
		if (!CheckLogging(logState)) { return; }
		Debug.LogError(ColorText(sender, GameManager.Config.Debug.ColorError) + message + ColorText(value, GameManager.Config.Debug.ColorValue));
	}

	public static void Initialized(string sender) {
		if (CheckLogging(LogStates.Normal)) {
			Debug.Log(ColorText(sender, GameManager.Config.Debug.ColorNormal) + "Has initialized" + ColorText(Time.realtimeSinceStartup.ToString("F2"), GameManager.Config.Debug.ColorValue));
		}
	}

	public static bool CheckLogging(LogStates state) {
		return (int)GameManager.Config.Debug.Logging >= (int)state;
	}

	static string ColorText(string text, Color color) {
		return " <color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + text + "</color> ";
	}
}
}