using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {
    public GameObject healthMeter;
    public int score;
    Text text;

	void Start () {
        score = 0;
		text.text = ("score: " + score);
	}
	

	void Update () {
		
	}
}
