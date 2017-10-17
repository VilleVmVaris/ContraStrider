using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, Damageable {
    Stats stats;

    Vector3 velocity;
    float gravity = -20;

    float moveSpeed = 6;

    Controller2D controller;
	// Use this for initialization
	void Start () {
        stats = GetComponent<Stats>();
        controller = GetComponent<Controller2D>();
	}
	
	// Update is called once per frame
	void Update () {

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        velocity.x = input.x * moveSpeed;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
		
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
