using System;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTown {
[CreateAssetMenu(fileName = "New Config", menuName = "SmallTown/System/Config")]
public class ConfigData : ScriptableObject {

	// Public variables
	public GeneralConfig General;
	public PlayerConfig Player;
	public EnemyConfig Enemy;
	public CameraConfig Camera;
	public AreaConfig Area;
	public EffectsConfig Effects;
	public ItemConfig Item;
	public UiConfig UI;
	public DebugConfig Debug;


	// ************************************************************************************************************ CUSTOM STRUCTS:

	[Serializable]
	public struct GeneralConfig {
		public string Version;
		public GameStates StartupState;
		public LayerMask CharacterMask;
		public LayerMask HiddenMask;
		public float PauseSpeed;
	}

	[Serializable]
	public struct PlayerConfig {
		public LayerMask Mask;
		public float ActivationDistance;

		[Header("Marker")]
		public float MarkerFadeDuration;
		public float MarkerHideDistance;
		public float DenyFadeDuration;

		[Header("Ragdoll")]
		public RagdollValues RagdollMelee;
		public RagdollValues RagdollFirearms;

		[Header("Speed")]
		public float SneakingSpeed;
		public float WalkingSpeed;
		public float RunningSpeed;

		[Header("Targeting")]
		public int LineSegments;
		public float LineAngle;
	}

	[Serializable]
	public struct EnemyConfig {
		public LayerMask Mask;
		public LayerMask TargetingMask;
		public MinMax TargetingCircle;
		public Vector3 LineOffset;
		public Vector3 WaypointOffset;

		[Header("Ragdoll")]
		public RagdollValues RagdollMelee;
		public RagdollValues RagdollFirearms;
	}

	[Serializable]
	public struct CameraConfig {
		public LayerMask DefaultMask;
		public LayerMask MapviewMask;
		public LayerMask SeethroughDefaultMask;
		public LayerMask SeethroughMapviewMask;
		public float FollowSpeed;

		[Header("Views")]
		public List<Vector3> ViewList;
		public Vector3 MapView;
		public float ViewSpeed;
	}

	[Serializable]
	public struct AreaConfig {
		public List<AreaScenes> AreaScenes;
		public List<FastTravelAreas> FastTravelAreas;
		public float FastTravelDelay;
	}

	[Serializable]
	public struct EffectsConfig {

		[Header("Easing")]
		public DG.Tweening.Ease EaseIn;
		public DG.Tweening.Ease EaseOut;
		public float EaseInDuration;
		public float EaseOutDuration;

		[Header("Post Processing")]
		public float HenrisRoomDuration;
		public float HostileAreaDuration;
	}

	[Serializable]
	public struct ItemConfig {
		public float DoorMovementDuration;

		[Header("Highlight")]
		public float HighlightShow;
		public float HighlightHide;

		[Header("Bullet")]
		public float BulletSpeed;
		public float BulletTrail;
	}

	[Serializable]
	public struct UiConfig {
		public float ChangePanelDuration;
		public float MessageFadeDuration;

		[Header("Colors")]
		public Color HostileTextColor;
		public Color HenrisRoomTextColor;
		public Color LockedColor;

		[Header("Tooltip")]
		public Vector2 TooltipOffset;
		public float TooltipDuration;

		[Header("Credits")]
		public float CreditsSpeed;
		public float CreditsStartup;
	}

	[Serializable]
	public struct DebugConfig {
		public EditorStates EditorState;
		public bool DeveloperMode;
		// public bool DontGenerateLevels;
		// public bool ForceNamesFinnish;

		[Header("Logging")]
		public LogStates Logging;
		public Color ColorNormal;
		public Color ColorWarning;
		public Color ColorError;
		public Color ColorValue;
	}
}
[Serializable]
public class RagdollValues {
	public float Multiplier;
	public float Upwards;
}
[Serializable]
public class FastTravelAreas {
	public string AreaKey;
	public Sprite AreaSprite;
	public List<string> FastTravelPoints;
}
[Serializable]
public class AreaScenes {
	public string AreaKey;
	public string AreaName;
}
}