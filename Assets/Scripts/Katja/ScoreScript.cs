using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {
    Text text;
    int score;
    public int scoreFactor;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        score = 0;
        scoreFactor = 1;
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Insert)) {
            score += 5 * scoreFactor;
        }
        text.text = ("Score: " + score);
    }
}
