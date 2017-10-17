using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar2 : MonoBehaviour {
    float healthBarFull;
    public float maxHealth = 50f;
    float health;
    float healthBarHeight;

    void Awake() {
        healthBarHeight = GetComponent<RectTransform>().rect.height;
        healthBarFull = /*GetComponent<RectTransform>().rect.width*/ 500;
        health = /*healthBarFull*/0;
        SetHealthBar(0);
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
        // health -= damage;
        if (health < healthBarFull) { 
        health += (healthBarFull / maxHealth) * damage;
        //transform.localScale = new Vector3(health , 1, 1);
        GetComponent<RectTransform>().sizeDelta = new Vector2(health, healthBarHeight);
        }
    }
}