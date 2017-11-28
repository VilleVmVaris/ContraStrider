﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour, Damageable
{
    BoxCollider2D collider;
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

    [HideInInspector]
    public bool crouching;
    bool midAir;
    bool justJumped;
    bool canDoubleJump;

    float origColliderX;
    float origColliderY;
    float colliderOffSet;
    float crouchColliderY;

    public float attackDurationTicks;

    public GameObject groundAttackObject;
    public GameObject chargeAttackObject;
	public GameObject ninjaSprite;
	public GameObject dashAttack;
	public ParticleSystem attackEffect;
	public Material slash;
	public Material chargeSlash;
	public ParticleSystem stepSandCloud;
	ParticleSystemRenderer attackEffectRenerer;

	SpriteMeshInstance[] sprites;
	Color originalColor;
	Color hitFlashColor = new Color(.9f, .3f, .3f, 1f);

	public GameObject swordChargeEffect;
    bool swordCharged;

    [HideInInspector]
    public bool chargingSword;

    public float chargeTime;
    float charged;
    public float timeBeforeCharge;

    [HideInInspector]
    public float timeToCharge;

    Vector3 velocity;
    float gravity = -20;

    public float moveSpeed = 6;
    float velocityXSmoothing;
	public float maxWalkSpeed = 4;

    Controller2D controller;

    Vector2 directionalInput;

    float maxJumpVelocity = 8;
    float minJumpVelocity;

	Animator animator;
    TimerManager timer;
	GUIManager gui;

    [HideInInspector]
    public BladeDash dash;

    [HideInInspector]
    public bool knockedBack;

    float knockForce;

    Vector2 knockDirection;

    float knockDuration;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        dash = GetComponent<BladeDash>();

        print("Gravity: " + gravity + " Jump velocity: " + maxJumpVelocity);

        timer = GameObject.Find("GameManager").GetComponent<TimerManager>();
		gui = GameObject.Find("GameCanvas").GetComponent<GUIManager>();
        collider = GetComponent<BoxCollider2D>();

        origColliderX = collider.size.x;
        origColliderY = collider.size.y;
        crouchColliderY = collider.size.y / 2;
        colliderOffSet = -collider.size.y / 4;
    
		animator = GetComponentInChildren<Animator>();

		attackEffectRenerer = attackEffect.GetComponent<ParticleSystemRenderer>();
		sprites = ninjaSprite.GetComponentsInChildren<SpriteMeshInstance>();
		originalColor = sprites[0].color;
		swordChargeEffect.SetActive(false);
		stepSandCloud.Stop();
	}

    // Update is called once per frame
    void Update()
    {
		//see if character has moved since last frame
		if (dash.dashing) {
			if (controller.collisions.left || controller.collisions.right) {
				dash.StopDash();
				EndAttackEffect();
			}
		}

        if (health <= 0) {
			return; // TODO: Proper handling of death 
		}
		if (dash.aiming) {
			velocity = Vector2.zero;
		}
		
        CalculateVelocity();
        HandleWallSliding();
		RotateY();

        if(CheckCollisionStatus()) {
            canDoubleJump = true;
		} else {
			PlayInAirAnimation();
		}

        if (dash.dashing)
        {
            controller.Move(dash.direction * dash.speed * Time.deltaTime, directionalInput);

        } else if(knockedBack)
        {
            print("knockdirection " + knockDirection);
            controller.Move(knockDirection * knockForce * Time.deltaTime, directionalInput);
            timer.Once(StopKnockBack, knockDuration);
        }

        else
        {
            controller.Move(velocity * Time.deltaTime, directionalInput);
        }

        //This stops gravity from accumulating if the controllers detects collisions above or below or while dashing
        if (controller.collisions.above || controller.collisions.below || dash.dashing)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0;

				if (velocity.x > 0.5f || velocity.x < -0.5f) {
					animator.SetBool("jumpair", false);	
				} else {
					animator.SetBool("jumpair", false);	
					animator.SetBool("jumpdown", true);	
				}
            }

        }

		if (chargingSword) {
			charged += Time.deltaTime;
			if (charged >= chargeTime) {
				swordCharged = true;
				swordChargeEffect.SetActive(true);
			}
		}

		if ((velocity.x > 0.15f || velocity.x < -0.15f ) 
			&& controller.collisions.below && !wallSliding && !dash.dashing) {
			if (velocity.x > maxWalkSpeed || velocity.x < -maxWalkSpeed ) {
				animator.SetBool("ninjarun", true);
				animator.SetBool("ninjawalk", false);
			} else {
				animator.SetBool("ninjawalk", true);
				animator.SetBool("ninjarun", false);
			}
			if (!stepSandCloud.isPlaying) {
				stepSandCloud.Play();	
			}
		} else {
			stepSandCloud.Stop();
			animator.SetBool("ninjarun", false);
			animator.SetBool("ninjawalk", false);	
		}


    }
    public void Attack(Vector2 input)
    {

		if (!dash.dashing && !dash.aiming && !knockedBack) {
			if ((input.x == 0 && input.y == 0) || crouching) {
				input.x = controller.collisions.faceDir;
				input.y = 0;
			}

			int attackDir = (int)(Vector2.Angle(input, Vector3.up));

			if (input.x < 0) {
				attackDir = 360 - attackDir;
			}

			attackDir = (attackDir + 22) / 45 * 45 % 360 - 90;
			 
			PlaySlashEffect(attackDir);

			groundAttackObject.transform.rotation = Quaternion.Euler(0, 0, -attackDir);
			groundAttackObject.SetActive(true);
			timer.Once(EndAttackEffect, attackDurationTicks);
		}
    }

    public void ChargedAttack(Vector2 input)
    {
        if(!dash.dashing && !dash.aiming && !knockedBack) { 

        if (swordCharged)
        {
            if ((input.x == 0 && input.y == 0) || crouching)
            {
                input.x = controller.collisions.faceDir;
                input.y = 0;
            }
            int attackDir = (int)(Vector2.Angle(input, Vector3.up));

            if (input.x < 0)
            {
                attackDir = 360 - attackDir;
            }

            attackDir = (attackDir + 22) / 45 * 45 % 360 - 90;

			PlaySlashEffect(attackDir, true);

            chargeAttackObject.transform.rotation = Quaternion.Euler(0, 0, -attackDir);

			chargeAttackObject.SetActive(true);
			swordChargeEffect.SetActive(false);

            timer.Once(EndAttackEffect, attackDurationTicks);

            ResetCharge();

        }
        else
        {
            ResetCharge();
        }

    }
    }

	void PlaySlashEffect(int attackDir, bool chargeAttack = false) {
		var psMain = attackEffect.GetComponent<ParticleSystem>().main;
		if (chargeAttack) {
			psMain.startSizeMultiplier = 9f;
			attackEffectRenerer.material = chargeSlash;
		} else {
			psMain.startSizeMultiplier = 5.5f;
			attackEffectRenerer.material = slash;
		}

		// Hardcoded rotations, cuz i couldn't be arsed
		if (attackDir == 0 || attackDir == 180) {
			if (velocity.x > maxWalkSpeed || velocity.x < -maxWalkSpeed || wallSliding) {
				animator.SetBool("ninjasword", true);
			} else {
				animator.SetBool("ninjaswordSTAND", true);
			}
			attackEffect.transform.rotation = Quaternion.Euler(75f, 0f, 0f);
		} else if (attackDir == -90) {
			animator.SetBool("ninjaswordUP", true);
			if (ninjaSprite.transform.rotation.y > 0) {
				//left
				attackEffect.transform.rotation = Quaternion.Euler(2f, -74.9f, -89.4f);
			} else {
				// right
				attackEffect.transform.rotation = Quaternion.Euler(-0.1f, 75f, 90f);	
			}
		} else if (attackDir == -45 || attackDir == 225) {
			animator.SetBool("ninjaswordUpcorner", true);
			if (attackDir == -45) {
				// right
				attackEffect.transform.rotation = Quaternion.Euler(56.3f, 62.1f, 66.2f);
			} else {
				// left
				attackEffect.transform.rotation = Quaternion.Euler(38.1f, -70.7f, -77.8f);	
			}

		} else if (attackDir == 45 || attackDir == 135) {
			animator.SetBool("ninjaswordDOWN", true);
			if (attackDir == 45) {
				// right
				attackEffect.transform.rotation = Quaternion.Euler(39.8f, -70.2f, -77f);
			} else {
				// left
				attackEffect.transform.rotation = Quaternion.Euler(43.6f, 69f, 75.1f);	
			}
		}	 
		attackEffect.Play();
	}

    public bool CheckCollisionStatus()
    {
        if (!controller.collisions.below && !controller.collisions.right && !controller.collisions.left)
        {
            return false;
        }
        else return true;
    }

	void PlayInAirAnimation() {
		// If player walks off a cliff
		if (!animator.GetCurrentAnimatorStateInfo(0).IsName("jumpup") &&
			!animator.GetCurrentAnimatorStateInfo(0).IsName("jumpair") &&
			!animator.GetCurrentAnimatorStateInfo(0).IsName("jumpdown") && 
			!animator.IsInTransition(0) 
			&& !dash.dashing && !dash.aiming && !wallSliding) {
			animator.SetBool("jumpdown", false);	
			animator.SetBool("jumpair", true);
		}
	}

    void EnableCrouch()
    {
        justJumped = false;
    }

    public void StartChargingSword()
    {
        chargingSword = true;

    }

    public void ResetCharge()
    {
        timeToCharge = 0;
        charged = 0;
        chargingSword = false;
        swordCharged = false;

    }

	public void BladeDashAttack(Vector2 input) {
        if(!dash.onCooldown) { 
		if (input == Vector2.zero) {
			input.x = controller.collisions.faceDir;
		}

		int attackDir = (int)(Vector2.Angle(input, Vector3.up));

		if (input.x < 0) {
			attackDir = 360 - attackDir;
		}

		attackDir = (attackDir + 22) / 45 * 45 % 360 - 90;
		dashAttack.transform.rotation = Quaternion.Euler(0, 0, -attackDir);
		dashAttack.SetActive(true);
		animator.SetBool("ninjadash", true);
		animator.SetBool("ninjastance", false);
		timer.Once(EndAttackEffect, dash.dashTicks);
	}
    }

    public void EndAttackEffect()
    {
        if (groundAttackObject.activeSelf)
        {

            groundAttackObject.SetActive(false);

        }
        else if (chargeAttackObject.activeSelf)
        {
            chargeAttackObject.SetActive(false);
		} else if (dashAttack.activeSelf && !dash.dashing) {
			dashAttack.SetActive(false);
			animator.SetBool("ninjadash", false);
		}
    }

    void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;

        //Smoothen change from initial velocity to target velocity, by default making it slower while airborne but faster while grounded
        //Will want to test this and see what feels better
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        velocity.y += gravity * Time.deltaTime;
    }

    void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;

        //Check for collisions on sides and below and make sure velocity.y is negative to enable wall slide
        wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0 && controller.collisions.collisionLayer == 9)
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

		animator.SetBool("ninjaslidedown", wallSliding);
    }

    public void Crouch()
    {
        if (controller.collisions.below && crouching == false && !justJumped)
        {

            crouching = true;

            var crouchCollider = new Vector2(collider.size.x, crouchColliderY);

            var offset = colliderOffSet;

            collider.size = crouchCollider;

            collider.offset = new Vector2(0, offset);

        }

		animator.SetBool("ninjacrouch", crouching);
    }

    public void StandUp()
    {
        if (crouching == true)
        {
            var origSize = new Vector2(origColliderX, origColliderY);
            collider.size = origSize;
            collider.offset = new Vector2(0, 0);
            crouching = false;

        }
		animator.SetBool("ninjacrouch", crouching);
    }

    public void SetDirectionalInput(Vector2 input)
    {

        if (crouching)
        {
            input.x = 0;
        }

        directionalInput = input;
    }

    public void JumpInputDown()
    {
        if (wallSliding)
        {

            //Wall-hop is direction of the wall is equal to the input, as in player is holding towards the wall
            //Might want to change later so that input doesn't have to be exactly 1?
            //Will want to test out different values
            if (Mathf.Abs(wallDirX - directionalInput.x) < 0.5f)
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
            }
            else
            {
				animator.SetBool("jumpup", true);
                if (controller.canFallThrough)
                {
                    controller.collisions.fallingThroughPlatform = true;
                    Invoke("ResetFallingThroughPlatform", .15f);
                    Invoke("StandUp", .15f);

                }
                else
                {
                    StandUp();
                    velocity.y = maxJumpVelocity;
                    justJumped = true;
                    Invoke("EnableCrouch", .1f);
                }
            }

        } else if (!CheckCollisionStatus() && canDoubleJump)
        {
            velocity.y = maxJumpVelocity;
            canDoubleJump = false;
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

	// Rotates player sprite to movement direction
	void RotateY() { 

		if (velocity.x < -0.1f || (wallSliding && controller.collisions.right) ) {
			ninjaSprite.transform.rotation = Helpers.FlipYRotation;
			var effect = attackEffect.main;
			effect.startRotationY = Mathf.Deg2Rad * 180f;
			// HAX: Animation positioning
			ninjaSprite.transform.localPosition = new Vector3(0.725f, 0f, 0f);	
		} else if (velocity.x > 0.1f || (wallSliding && controller.collisions.left) ) {
			ninjaSprite.transform.rotation = Quaternion.identity;
			var effect = attackEffect.main;
			effect.startRotationY = 0f;
			// HAX: Animation positioning 
			ninjaSprite.transform.localPosition = new Vector3(-0.725f, 0f, 0f);
		}

	}

	public bool TakeDamage(int damage)
    {
        health -= damage;
		gui.SetHealth(health);
		foreach (var sprite in sprites) {
			sprite.color = hitFlashColor;
		}
		timer.Once(FlashSpriteColorBack, .1f);
        if (health <= 0) {
            print("Player died");
            Die();
			return true;
		} else {
			return false;
		} 

    }

	void FlashSpriteColorBack() {
		foreach (var sprite in sprites) {
			sprite.color = originalColor;
		}
	}

    public void KnockBack(Vector2 direction, float force, float duration)
    {
        knockDirection = direction;
        knockDuration = duration;
        knockForce = force;
        knockedBack = true;
    }

    void StopKnockBack()
    {
        knockedBack = false;
    }

    void Die()
    {
		animator.SetBool("ninjadeath", true);
    }
}


