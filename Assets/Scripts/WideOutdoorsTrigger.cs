using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WideOutdoorsTrigger : MonoBehaviour {

	CameraFollow mainCamera;
	Direction enterDirection;

	enum Direction {
		Right,
		Left
	}

	// Use this for initialization
	void Start () {
		mainCamera = Camera.main.GetComponent<CameraFollow>();
	}

	void OnTriggerEnter2D(Collider2D c) {
		if (c.CompareTag("Player")) {
			Vector3 direction = c.transform.position - transform.position;
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
				mainCamera.ToggleWideOutdoorsMode();
			} 
			if (Vector3.Dot (transform.right, direction) < 0 && enterDirection == Direction.Right) {
				mainCamera.ToggleWideOutdoorsMode();
			}	
		}
	}

}