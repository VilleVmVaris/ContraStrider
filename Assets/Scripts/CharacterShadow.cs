using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShadow : MonoBehaviour {

	public float shadowDistance;

	SpriteRenderer sr;
	Transform player;
	Vector3 maxScale;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer>();
		player = transform.root;
		maxScale = transform.localScale;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		RaycastHit2D hit = Physics2D.Raycast(player.position, Vector2.down, shadowDistance, Helpers.ObstacleLayerMask);
		if (hit.collider != null) {
			sr.enabled = true;
			// Position
			transform.position = hit.point;
			// Scale
			var t = Mathf.InverseLerp(0, shadowDistance, hit.distance);
			transform.localScale = Vector3.Lerp(maxScale, Vector3.zero, t);
		} else {
			sr.enabled = false;
		}
	}
}
