using UnityEngine;
using UnityEngine.UI;


namespace SmallTown {
public class MultiImageTargets : MonoBehaviour {

	[SerializeField] private Graphic[] targetGraphics;
	public Graphic[] GetTargetGraphics => targetGraphics;

}
}