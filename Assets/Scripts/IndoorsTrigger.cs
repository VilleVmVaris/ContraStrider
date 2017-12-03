using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorsTrigger : MonoBehaviour {

	CameraFollow mainCamera;
	Direction enterDirection;

	enum Direction {
		Right,
		Left,
		Top,
		Bottom
	}

	// Use this for initialization
	void Start () {
		mainCamera = Camera.main.GetComponent<CameraFollow>();
	}

	void OnTriggerEnter2D(Collider2D c) {
		if (c.CompareTag("Player")) {
			Vector3 direction = c.transform.position - transform.position;


			var angle = Vector3.Angle(direction, Vector3.up);

			print("Enter angle " + angle);

			if (Vector3.Dot(transform.right, direction) > 0) {
				enterDirection = Direction.Right;
			} 
			if (Vector3.Dot(transform.right, direction) < 0) {
				enterDirection = Direction.Left;
			}
		}
	}


	void OnTriggerExit2D(Collider2D c) {
		if (c.CompareTag("Player")) {
			Vector3 direction = c.transform.position - transform.position;
			if (Vector3.Dot (transform.right, direction) > 0 && enterDirection == Direction.Left) {
				mainCamera.ToggleIndoorsMode();
			} 
			if (Vector3.Dot (transform.right, direction) < 0 && enterDirection == Direction.Right) {
				mainCamera.ToggleIndoorsMode();
			}	
		}
	}

}
