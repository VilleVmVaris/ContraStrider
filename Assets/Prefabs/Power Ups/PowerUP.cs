using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUP : MonoBehaviour {
    // Lisätään GameManageriin
    public List<string> powerUpS = new List<string>();
    public HealthBarScript HBS;
    public ScoreScript SS;
    public CheckpointScript CPS;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == 20) {
            var power = other.gameObject.name;
            powerUpS.Add("" + power);
            Destroy(other.gameObject);
            SetPowerUp();
        } else if (other.gameObject.layer == 21) {

        }
    }
    void SetPowerUp() {

        foreach (string pw in powerUpS) {
            if (pw == ("HealthKit")) {
                HBS.SetFullHealth();
//                int i = powerUpS.FindIndex("HealthKit");
//                powerUpS.RemoveAt(i);
            }

            if (pw == ("DoubleScore")) {
                SS.scoreFactor = 2; //(Timelimit?)
            }
        }
    }
}
