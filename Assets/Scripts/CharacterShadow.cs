using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShadow : MonoBehaviour {

	public float shadowDistance;

	SpriteRenderer sr;
	Transform player;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer>();
		player = transform.root;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		RaycastHit2D hit = Physics2D.Raycast(player.position, Vector2.down, shadowDistance, Helpers.ObstacleLayerMask);
		if (hit.collider != null) {
			sr.enabled = true;
			transform.position = hit.point;
// 			//TODO: Fade and/or scale?
//			Color color = sr.material.color;
//			color.a = ??
//			sr.material.color = color;
		} else {
			sr.enabled = false;
		}
	}
}
