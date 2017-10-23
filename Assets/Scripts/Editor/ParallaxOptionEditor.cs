using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ParallaxOptions))]
public class ParallaxOptionEditor : Editor {

	ParallaxOptions parallaxOptions;

	// Use this for initialization
	void Awake () {
		parallaxOptions = target as ParallaxOptions;
	}
	
	public override void OnInspectorGUI () {
		base.OnInspectorGUI();
		if (GUILayout.Button("Save Position")) {
			parallaxOptions.SaveCameraPosition();
			EditorUtility.SetDirty(parallaxOptions);
		}
		if (GUILayout.Button("Load Position")) {
			parallaxOptions.LoadCameraPosition();
		}
	}
}
