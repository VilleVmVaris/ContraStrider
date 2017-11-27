using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeDashWaveEffect : MonoBehaviour {

	public float speed;

	static readonly Vector3 START_SIZE = new Vector3(.1f, .1f, 1f);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// TODO: play with noise texture offset?

		var scale = transform.localScale;
		var newScale = scale.x + speed * Time.deltaTime;
		scale = new Vector3(newScale, newScale);
		if (scale.magnitude > 6f) {
			scale = START_SIZE;
		}
		transform.localScale = scale;
	}
}
