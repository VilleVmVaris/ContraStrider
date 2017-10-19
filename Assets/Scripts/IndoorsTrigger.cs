using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorsTrigger : MonoBehaviour {

	CameraFollow mainCamera;

	// Use this for initialization
	void Start () {
		mainCamera = Camera.main.GetComponent<CameraFollow>();
	}

	void OnTriggerEnter2D(Collider2D c) {
		if (c.CompareTag("Player")) {
			mainCamera.ToggleIndoorsMode();	
		}
	}
}
