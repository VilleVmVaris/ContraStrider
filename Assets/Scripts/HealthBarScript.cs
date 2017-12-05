using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour {

    public PowerUP PWU;
    public Image bar;
    public Player player;
    public float health;
    public float fullHealth;

    void Start () {
        fullHealth = player.health;
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
		health = healthNow;
        if (healthNow >= 0) {
			bar.fillAmount = health / fullHealth;
            if (healthNow <= 0) {
                print("GAME OVER");
            }
        }
    }
    public void SetFullHealth() {
        health = fullHealth;
        SetHealthBar(health);
        // PWU.powerUpS.Remove("HealthKit");
        // Debug.Log("health Full");
    }
}
