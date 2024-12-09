using UnityEngine;

namespace SmallTown {
[System.Serializable]
public class MinMax {

	// Variables
	[SerializeField] private float current = 0.0f;
	[SerializeField] private float minimum = 0.0f;
	[SerializeField] private float maximum = 0.0f;

	// Public variables
	public float Current { get { return current; } }
	public float Minimum { get { return minimum; } }
	public float Maximum { get { return maximum; } }


	// ************************************************************************************************************ CONSTRUCTORS

	public MinMax() {
		current = 0.0f;
		minimum = 0.0f;
		maximum = 0.0f;
	}

	public MinMax(MinMax minMax) {
		current = minMax.current;
		minimum = minMax.minimum;
		maximum = minMax.maximum;
	}

	public MinMax(float newCurrent, float newMinimum, float newMaximum) {
		current = newCurrent;
		minimum = newMinimum;
		maximum = newMaximum;
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Set(float value) {
		current = value;
		Clamp();
	}

	public void Change(float value) {
		current += value;
		Clamp();
	}

	public void Reset() {
		current = maximum;
		Clamp();
	}

	public void Clamp() {
		current = Mathf.Clamp(current, minimum, maximum);
	}
}
}