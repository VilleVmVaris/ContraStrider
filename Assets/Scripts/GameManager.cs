﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    
	Serializer serializer;
	GUIManager gui;
	Player player;
    TimerManager timer;

	// Use this for initialization
	void Start() {
		serializer = GetComponentInChildren<Serializer>();
		gui = GameObject.Find("GameCanvas").GetComponent<GUIManager>();
		player = GameObject.Find("Player").GetComponent<Player>();
        timer = GetComponent<TimerManager>();

	}
	
	// Update is called once per frame
	void Update() {
		
	}

	public void EndGame() {
		gui.FadeToBlack(5f);
        timer.Once(ShowCredits, 10);
		// wait and load credits scene?
	}

	public void LoadGame() {
		serializer.LoadCheckPoint();
		player.Restart();
	}

    void ShowCredits()
    {
        SceneManager.LoadScene(2);
    }
}
