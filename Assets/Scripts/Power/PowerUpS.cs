using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PowerUpS : MonoBehaviour {
    public HealthBar healthBar;
	// Use this for initialization
	void Start () {
        SaveData data = new SaveData() { health = healthBar.fullHealth, powerUpS = new List<string>() };
        data.powerUpS.Add("heal");
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
