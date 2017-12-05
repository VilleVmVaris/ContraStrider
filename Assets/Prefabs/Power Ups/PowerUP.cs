using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUP : MonoBehaviour {
	
	 public GUIManager gui;

	void Start() {
		//gui = GameObject.Find("GameCanvas").GetComponent<GUIManager>();
	}

	void OnTriggerEnter2D(Collider2D other) {

		if (other.gameObject.layer == 20) {
			string power = other.gameObject.name;
			// powerUpS.Add("" + power);
			//Destroy(other.gameObject);
			SetPowerUp(power);
		}

		if (other.gameObject.layer == 21) {
			string power = other.gameObject.name;
			SetPowerUp(power);

		}
	}


	void SetPowerUp(string power) {
        
		if (power == ("HealthKit")) {
			gui.SetFullHealth();
		}

		if (power == ("DoubleScore")) {
			gui.DoubleScoreFactor();
		}

		// TODO: Checkpoint logic under game manager maybe?
		if (power == ("Checkpoint")) {
			//PlayerPrefs.SetFloat("PlayerX", transform.position.x);
			//PlayerPrefs.SetFloat("PlayerY", transform.position.y);
			// PlayerPrefs.SetFloat("PlayerZ", transform.position.z);
			// What else need to save?
		}

	}
}
