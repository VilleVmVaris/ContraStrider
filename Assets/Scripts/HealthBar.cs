using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    float healthBarFull;
    public float maxHealth;
    float health;
    float healthBarHeight;

    void Awake() {
        healthBarHeight = GetComponent<RectTransform>().rect.height;
        healthBarFull = GetComponent<RectTransform>().rect.width;
        health = 0;
        SetHealthBar(health);
    }

    void Update() {
        //***Testing***
        if (Input.GetKeyDown(KeyCode.Delete)) {
            SetHealthBar(1);
        }
        //***Testing***
        if (health >= healthBarFull) {
            print("Game Over!!!");
        }

    }
    public void SetHealthBar(float damage) {
        if (health < healthBarFull) { 
        health += (healthBarFull / maxHealth) * damage;
        GetComponent<RectTransform>().sizeDelta = new Vector2(health, healthBarHeight);
        }
    }
}