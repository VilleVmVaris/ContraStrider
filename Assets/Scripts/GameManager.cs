using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    
    public enum Gamestate {Running, Paused};

    public Gamestate state;

	Serializer serializer;
	GUIManager gui;
	Player player;
    TimerManager timer;
    GameObject[] activators;

    public GameObject boss;

	// Use this for initialization
	void Start() {
		serializer = GetComponentInChildren<Serializer>();
		gui = GameObject.Find("GameCanvas").GetComponent<GUIManager>();
		player = GameObject.Find("Player").GetComponent<Player>();
        timer = GetComponent<TimerManager>();
        activators = GameObject.FindGameObjectsWithTag("Spawner");
		Unpause();
	}
	
	// Update is called once per frame
	void Update() {
		
	}

	public void EndGame() {
		gui.FadeToBlack(5f);
        timer.Once(ShowCredits, 30);
		// wait and load credits scene?
	}

	public void LoadGame() {
        foreach (GameObject go in activators)
        {
            go.SetActive(true);
            go.GetComponent<WaveActivator>().wave.GetComponent<WaveSpawner>().OnReload();
            go.GetComponent<WaveActivator>().spawned = false;
        }
        serializer.LoadCheckPoint();
		player.Restart();
        gui.SetHealth(player.health);
        if(boss.activeSelf)
        {
            boss.GetComponent<BossScript>().Reset();
        }

	}

	public void Restart() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

    public void Pause()
    {
        state = Gamestate.Paused;
        gui.ShowPause();
        if (!Mathf.Approximately(Time.timeScale, 0f))
        {
            Time.timeScale = 0;
        }
    }

    public void Unpause()
    {
        state = Gamestate.Running;
        gui.HidePause();
        Time.timeScale = 1;
    }

    void ShowCredits()
    {
        SceneManager.LoadScene(2);
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }
}
