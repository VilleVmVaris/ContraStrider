﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorsTrigger : MonoBehaviour {

	public bool isIndoors;

	CameraFollow mainCamera;
	Direction enterDirection;
	BoxCollider2D boxCollider2D;
	Player player;


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
		player = GameObject.Find("Player").GetComponent<Player>();
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

			// *** fukfuk
			if (isIndoors) {
				mainCamera.SetIndoors();
				player.SetStepDustEffect(false);
			} else {
				mainCamera.SetOutdoors();
				player.SetStepDustEffect(true);
			}
			return;
			// ***
//
//			var contactPoint = boxCollider2D.bounds.ClosestPoint(c.transform.position);
//			var contactNormal = (c.transform.position - contactPoint).normalized;
//
//			if (Vector3.Dot(transform.right, contactNormal) > 0 && enterDirection == Direction.Left) {
//				mainCamera.ToggleIndoorsMode();
//				player.ToggleStepDustEffect();
//			} 
//			if (Vector3.Dot(transform.right, contactNormal) < 0 && enterDirection == Direction.Right) {
//				mainCamera.ToggleIndoorsMode();
//				player.ToggleStepDustEffect();
//			}
//			if (Vector3.Dot(transform.up, contactNormal) > 0 && enterDirection == Direction.Bottom) {
//				mainCamera.ToggleIndoorsMode();
//				player.ToggleStepDustEffect();
//			} 
//			if (Vector3.Dot(transform.up, contactNormal) < 0 && enterDirection == Direction.Top) {
//				mainCamera.ToggleIndoorsMode();
//				player.ToggleStepDustEffect();
//			}
		}
	}

}
