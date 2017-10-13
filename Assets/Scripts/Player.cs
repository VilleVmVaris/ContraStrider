using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, Damageable {
    Stats stats;
	// Use this for initialization
	void Start () {
        stats = GetComponent<Stats>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void TakeDamage(int damage) {
        stats.health -= damage;
        if(stats.health == 0) {
            print("Player died");
            Die();
        }
    }
    void Die() {
        Destroy(gameObject);
    }
}
