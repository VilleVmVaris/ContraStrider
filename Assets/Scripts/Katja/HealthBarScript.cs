using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour {

	// TODO: Player script should send health amount at start?

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

        bar.fillAmount = player.health / fullHealth;
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
        // PWU.powerUpS.Remove("HealthKit");
        // Debug.Log("health Full");
    }
}
