using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour, Damageable {


    Stats stats;

    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    public float accelerationTimeAirborne = .2f;
    public float accelerationTimeGrounded = .1f;
    public float wallSlideSpeedMax = 3;
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    Vector3 velocity;
    float gravity = -20;

    public float moveSpeed = 6;
    float velocityXSmoothing;

    Controller2D controller;

    Vector2 directionalInput;

    float maxJumpVelocity = 8;
    float minJumpVelocity;

    bool wallSliding;

	// Use this for initialization
	void Start () {
        stats = GetComponent<Stats>();
        controller = GetComponent<Controller2D>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        print("Gravity: " + gravity + " Jump velocity: " + maxJumpVelocity);
	}

    public void SetDirectionalInput(Vector2 input)
    {

    }
	
	// Update is called once per frame
	void Update () {
        print(controller.canFallThrough);
        

        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int wallDirX = (controller.collisions.left) ? -1 : 1;

        float targetVelocityX = directionalInput.x * moveSpeed;

        //Smoothen change from initial velocity to target velocity, by default making it slower while airborne but faster while grounded
        //Will want to test this and see what feels better
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);


        //Check for collisions on sides and below and make sure velocity.y is negative to enable wall slide
        bool wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (directionalInput.x != wallDirX && directionalInput.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            } else
            {
                timeToWallUnstick = wallStickTime;
            }
        }


        if(Input.GetKey(KeyCode.Space))
        {
            if(wallSliding)
            {

                if(directionalInput.y == -1 && controller.collisions.below)
                {

                }

                //Wall-hop is direction of the wall is equal to the input, as in player is holding towards the wall
                //Might want to change later so that input doesn't have to be exactly 1?
                //Will want to test out different values
                if (wallDirX == directionalInput.x)
                {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if (directionalInput.x == 0)
                {
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }
                else
                {
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }

            }
            if (controller.collisions.below && !controller.canFallThrough)
            {
                velocity.y = maxJumpVelocity;

            } else if (controller.canFallThrough)
            {
                controller.collisions.fallingThroughPlatform = true;
                Invoke("ResetFallingThroughPlatform", .5f);

            }
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, directionalInput);

        //This stops gravity from accumulating if the controllers detects collisions above or below
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }


    }

    void ResetFallingThroughPlatform()
    {
        controller.collisions.fallingThroughPlatform = false;
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
