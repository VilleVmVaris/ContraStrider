using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    Serializer serializer;
	// Use this for initialization
	void Start () {
        serializer = GetComponentInChildren<Serializer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void EndGame()
    {
        //Fade to black and load credits scene?
    }

    public void LoadGame()
    {
        serializer.LoadCheckPoint();
    }
}
