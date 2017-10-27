using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour {

    public Image bar;
    public float health = 0;
    public float fullHealth = 100;

    void Start () {
        SetFullHealth();
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
    public void SetFullHealth() {
        health = fullHealth;
        SetHealthBar(health);
    }
}
