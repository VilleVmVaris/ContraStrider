using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : MonoBehaviour {

	public GameObject teapot;

	BoxCollider2D boxCollider;

	// Use this for initialization
	void Start () {
		boxCollider = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag("Player")) {
			teapot.SetActive(false);
			boxCollider.enabled = false;
		}
	}
}
