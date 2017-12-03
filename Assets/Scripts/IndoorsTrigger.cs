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
			var contactPoint = GetComponent<Collider2D>().bounds.ClosestPoint(c.transform.position);
			var contactNormal = (c.transform.position - contactPoint).normalized;

			if (Vector3.Dot(transform.right, contactNormal) > 0) {
				enterDirection = Direction.Right;
			} 
			if (Vector3.Dot(transform.right, contactNormal) < 0) {
				enterDirection = Direction.Left;
			}
			if (Vector3.Dot(transform.up, contactNormal) > 0) {
				enterDirection = Direction.Top;
			} 
			if (Vector3.Dot(transform.up, contactNormal) < 0) {
				enterDirection = Direction.Bottom;
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

			// TODO: Top/Bottom toggles

			if (enterDirection == Direction.Top) {
				print("exit top");
			}
		}
	}

}
