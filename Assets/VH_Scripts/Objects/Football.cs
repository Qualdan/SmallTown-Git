using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTown {
public class Football : MonoBehaviour {

	// Private variables
	[SerializeField] private Rigidbody body;


	// ************************************************************************************************************ UNITY FUNCTIONS

	void OnCollisionEnter(Collision collision) {
		if (collision.transform.CompareTag("Player")) {
			MoveBall();
		}
	}


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	void MoveBall() {
		if(GameManager.Player.Moving) {
			Vector3 direction = transform.position - GameManager.Player.Position;
			float speed = GameManager.Player.State == PlayerStates.Running ? 20.0f : 10.0f;
			body.AddForce(direction * speed, ForceMode.Impulse);
		}
	}
}
}