using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShadow : MonoBehaviour {

	public float shadowDistance;

	SpriteRenderer sr;
	Vector3 groundPosition;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer>();
		groundPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, shadowDistance, Helpers.ObstacleLayerMask);
		if (hit.collider != null) {
			sr.enabled = true;

// 			//TODO: Fade and position shadow
//			Color color = sr.material.color;
//			color.a = ??
//			sr.material.color = color;
		} else {
			sr.enabled = false;
		}
	}
}
