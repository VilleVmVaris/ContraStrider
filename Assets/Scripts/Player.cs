using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour, Damageable {

	public int health;

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
    bool wallSliding;
    int wallDirX;

    public GameObject groundAttackObject;

    Vector3 velocity;
    float gravity = -20;

    public float moveSpeed = 6;
    float velocityXSmoothing;

    Controller2D controller;

    Vector2 directionalInput;

    float maxJumpVelocity = 8;
    float minJumpVelocity;

	// Use this for initialization
	void Start () {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        print("Gravity: " + gravity + " Jump velocity: " + maxJumpVelocity);
	}

    // Update is called once per frame
    void Update () {
        CalculateVelocity();
        HandleWallSliding();

        controller.Move(velocity * Time.deltaTime, directionalInput);

        //This stops gravity from accumulating if the controllers detects collisions above or below
        if (controller.collisions.above || controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0;
            }
        }
    }
    public void Attack(Vector2 input)
    {
        if (input.x == 0 && input.y == 0)
        {
            input.x = controller.collisions.faceDir;
        }
        int attackDir = (int)(Vector2.Angle(input, Vector3.up));

        if (input.x < 0)
        {
            attackDir = 360 - attackDir;
        }

        attackDir = (attackDir + 22) / 45 * 45 % 360 - 90;

        groundAttackObject.transform.rotation = Quaternion.Euler(0, 0, -attackDir);

        groundAttackObject.SetActive(true);

    }
    void CalculateVelocity()
    {

        float targetVelocityX = directionalInput.x * moveSpeed;

        //Smoothen change from initial velocity to target velocity, by default making it slower while airborne but faster while grounded
        //Will want to test this and see what feels better
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        velocity.y += gravity * Time.deltaTime;
    }

    void HandleWallSliding ()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;

        //Check for collisions on sides and below and make sure velocity.y is negative to enable wall slide
        wallSliding = false;
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
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    public void JumpInputDown()
    {
        if (wallSliding)
        {

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
        if (controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x))
                { // not jumping against max slope
                    velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
                    velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
                }
            } else
            {
                if (controller.canFallThrough)
                {
                    controller.collisions.fallingThroughPlatform = true;
                    Invoke("ResetFallingThroughPlatform", .5f);

                }
                else
                {
                    velocity.y = maxJumpVelocity;
                }
            }

        }
        
    }

    public void JumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }

    void ResetFallingThroughPlatform()
    {
        controller.collisions.fallingThroughPlatform = false;
    }

    public void TakeDamage(int damage) {
        health -= damage;
        if(health <= 0) {
            print("Player died");
            Die();
        }
    }
    void Die() {
        Destroy(gameObject);
    }
}
