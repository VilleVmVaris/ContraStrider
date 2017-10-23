using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour {

	[Tooltip("Parallax scroll speed relative to camera movement")]
	public float speedX;
	public float speedY;

	Transform cameraTransform;
	Vector3 previousCameraPosition;

	// TODO: options

	// Use this for initialization
	void Start () {
		cameraTransform = Camera.main.transform;
		previousCameraPosition = cameraTransform.position;
	}
	
	// Update is called once per frame
	void Update () {
		var delta = cameraTransform.position - previousCameraPosition;
		transform.position += Vector3.Scale(delta, new Vector3(speedX, speedY));
		previousCameraPosition = cameraTransform.position;
	}
}
