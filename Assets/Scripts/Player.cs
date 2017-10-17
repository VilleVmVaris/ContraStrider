using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, Damageable {


    Stats stats;

    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    public float accelerationTimeAirborne = .2f;
    public float accelerationTimeGrounded = .1f;

    Vector3 velocity;
    float gravity = -20;

    public float moveSpeed = 6;
    float velocityXSmoothing;

    Controller2D controller;



    float jumpVelocity = 8;
	// Use this for initialization
	void Start () {
        stats = GetComponent<Stats>();
        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        print("Gravity: " + gravity + " Jump velocity: " + jumpVelocity);
	}
	
	// Update is called once per frame
	void Update () {

        //This stops gravity from accumulating if the controllers detects collisions above or below
        if(controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if(Input.GetKey(KeyCode.Space) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        float targetVelocityX = input.x * moveSpeed;

        //Smoothen change from initial velocity to target velocity, by default making it slower while airborne but faster while grounded
        //Will want to test this and see what feels better
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
		
	}
    public void TakeDamage(int damage) {
        stats.health -= damage;
        if(stats.health <= 0) {
            print("Player died");
            Die();
        }
    }
    void Die() {
        Destroy(gameObject);
    }
}
