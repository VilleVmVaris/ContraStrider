using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {
    Text text;
    int score;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        score = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Insert)) {
            score += 5;
        }
        text.text = ("Score: " + score);
    }
}
