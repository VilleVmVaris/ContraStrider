using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Image bar;
    public float health;
    public float fullHealth;

    void Start () {
        health = fullHealth;
    }
	

	void Update () {
        //***Testing***
        if (Input.GetKeyDown(KeyCode.Delete)) {
            health -= 10;
            SetHealthBar(health);
        }
        //***Testing***
    }
    public void SetHealthBar(float healthNow) {
        if (healthNow >= 0) {
            bar.fillAmount = healthNow / fullHealth;
            if (healthNow <= 0) {
                print("GAME OVER");
            }
        }
    }
}
