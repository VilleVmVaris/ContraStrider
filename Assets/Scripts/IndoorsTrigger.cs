using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorsTrigger : MonoBehaviour {

	CameraFollow mainCamera;
	Direction enterDirection;
	BoxCollider2D boxCollider2D;

	enum Direction {
		Right,
		Left,
		Top,
		Bottom
	}

	// Use this for initialization
	void Start() {
		mainCamera = Camera.main.GetComponent<CameraFollow>();
		boxCollider2D = GetComponent<BoxCollider2D>();
	}

	void OnTriggerEnter2D(Collider2D c) {
		if (c.CompareTag("Player")) {
			var contactPoint = boxCollider2D.bounds.ClosestPoint(c.transform.position);
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

			var contactPoint = boxCollider2D.bounds.ClosestPoint(c.transform.position);
			var contactNormal = (c.transform.position - contactPoint).normalized;

			if (Vector3.Dot(transform.right, contactNormal) > 0 && enterDirection == Direction.Left) {
				mainCamera.ToggleIndoorsMode();
			} 
			if (Vector3.Dot(transform.right, contactNormal) < 0 && enterDirection == Direction.Right) {
				mainCamera.ToggleIndoorsMode();
			}
			if (Vector3.Dot(transform.up, contactNormal) > 0 && enterDirection == Direction.Bottom) {
				mainCamera.ToggleIndoorsMode();
			} 
			if (Vector3.Dot(transform.up, contactNormal) < 0 && enterDirection == Direction.Top) {
				mainCamera.ToggleIndoorsMode();
			}
		}
	}

}
