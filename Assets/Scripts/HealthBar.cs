using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    float healthBarFull;
    float health;
    float healthBarHeight;

    void Awake() {
        healthBarHeight = GetComponent<RectTransform>().rect.height;
        healthBarFull = GetComponent<RectTransform>().rect.width;
        health = healthBarFull;
    }

    void Update() {
        //***Testing***
        if (Input.GetKeyDown(KeyCode.Delete)) {
            SetHealthBar(1);
        }
        //***Testing***
        if (health <= 0) {
            print("Game Over!!!");
        }

    }
    public void SetHealthBar(float damage) {
        health -= (healthBarFull / 50) * damage;
        GetComponent<RectTransform>().sizeDelta = new Vector2(health, healthBarHeight);
    }
}