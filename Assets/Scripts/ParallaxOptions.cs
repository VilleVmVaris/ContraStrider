using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxOptions : MonoBehaviour {

	public bool moveParallaxLayers;

	[SerializeField]
	[HideInInspector]
	Vector3 storedPosition;

	public void SaveCameraPosition() {
		storedPosition = transform.position;
	}

	public void LoadCameraPosition() {
		transform.position = storedPosition;
	}

}
