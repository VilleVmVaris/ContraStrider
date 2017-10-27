using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUP : MonoBehaviour {
    // Lisätään GameManageriin
    public List<string> powerUpS = new List<string>();
    public HealthBarScript HBS;
    public ScoreScript SS;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == 20) {
            var power = other.gameObject.name;
            powerUpS.Add("" + power);
            Destroy(other.gameObject);
            SetPowerUp();
        }
    }
    void SetPowerUp() {
        // Case: HealthKit
        //      HealthMeter_Panel_Health_HealthBar.health = FullHealth
        foreach (string pw in powerUpS) {
            if (pw == ("HealthKit")) {
                HBS.SetFullHealth();
            }
            if (pw == ("DoubleScore")) {
                SS.scoreFactor = 2;
            }
        }

        // Case: DoubleScore
        //      ScoreScript.scoreFactor; // Time?

        // Case: Checkpoint?
        // .......
    }
}
