using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropToGround : MonoBehaviour {

	public bool AddOffset; // HAX: Not necessary if one bothers to tweak proper collision boxes...

	static readonly float SPEED = 8f;

	bool stop = false;

	// Use this for initialization
	void Start() {
		
	}

	// Update is called once per frame
	void FixedUpdate() {
		if (!stop) {
			RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, Helpers.ObstacleLayerMask);
			if (hit.collider != null) {
				// Set final position
				transform.position = AddOffset ? hit.point + Vector2.up : hit.point;
				stop = true;
			} else {
				transform.Translate(Vector3.down * SPEED * Time.deltaTime);
			}	
		}
	}

}
