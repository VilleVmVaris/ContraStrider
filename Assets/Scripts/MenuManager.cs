﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	public void Play() {
		SceneManager.LoadScene("Level_1_nu");
	}

	public void PlayTutorial() {
		SceneManager.LoadScene("Level_0");
	}

	public void Quit() {
		Application.Quit();
	}
}
