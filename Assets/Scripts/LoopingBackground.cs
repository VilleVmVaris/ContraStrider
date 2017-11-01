using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoopingBackground : MonoBehaviour {

	public List<Transform> sprites;
	float spriteWidth;
	public float warpThreshold = 1f;
	
	// Use this for initialization
	void Start() {
		foreach (var t in transform) {
			sprites.Add((Transform)t);
		}
		sprites = sprites.OrderBy(x => x.position.x).ToList();
		var sr = sprites[0].GetComponent<SpriteRenderer>();
		spriteWidth = sr.bounds.size.x; // assume all are same size
		//spriteWidth -= 0.1f;
	}
	
	// Update is called once per frame
	void Update() {
		var cam = Camera.main;
		var camP = cam.transform.position;
		var halfCW = cam.orthographicSize * cam.aspect;
		Debug.DrawLine(Vector3.up + camP, Vector3.up + camP + Vector3.right * halfCW);
		// right case
		// if camera's right edge close enough to rightmost sprite's right edge
		if (sprites[sprites.Count - 1].position.x + spriteWidth * 0.5f - (camP.x + halfCW) < warpThreshold) {
			sprites[0].position = sprites[sprites.Count - 1].position + Vector3.right * spriteWidth;
			var newRight = sprites[0];
			sprites.RemoveAt(0);
			sprites.Add(newRight);
		}
		// left case
		// if camera's left edge close enough to leftmost sprite's left edge
		if (camP.x - halfCW - (sprites[0].position.x - spriteWidth * 0.5f) < warpThreshold) {
			sprites[sprites.Count - 1].position -= Vector3.right * spriteWidth * sprites.Count;
			var newLeft = sprites[sprites.Count - 1];
			sprites.RemoveAt(sprites.Count - 1);
			sprites.Insert(0, newLeft);
		}
	}
}
