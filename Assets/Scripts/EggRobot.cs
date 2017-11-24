using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class EggRobot : MonoBehaviour, Damageable {
	public enum RobotType {Normal, Flying};

    public RobotType type;

	public GameObject robotSprite;
	public int health;

	[Header("Movement")]
	public float moveSpeed;
	public float chaseDistance;
    public float distanceAllowance;
	public float floatingAmplitude;
	public float floatingSpeed;

    public bool damageable = true;

	[Header("Weapon Use")]
	public float shootingDistance;
    public float kickDistance;
    public float kickDuration;
    public float kickDelay;
    public GameObject kickObject;
	public int burstAmount;
	public int burstInterval;
    public float kickCooldown;
    bool kickOnCooldown;

    public bool shielded;

	[Header("Effects")]
	public Animator coreAnimator;
	public List<AnimationClip> deathAnimations;
	public float fadeTime;
	public ParticleSystem hitSpark;
	public GameObject shieldSprite;
	public Animator shieldAnimator;
	public GameObject jetpackSprite;
	SpriteMeshInstance[] sprites;
	Color originalColor;
	Color flashColor;

	GameObject player;
	TimerManager timer;
	Controller2D controller;
	EnemyWeapon weapon;

	readonly float gravity = -20f;

	Vector2 velocity;
	float moveDirection;
	bool canShoot;
	System.Guid rotateTimer;
    bool kicking;

    Vector2 startPoint;

	// Use this for initialization
	void Start() {
		controller = GetComponent<Controller2D>();
		player = GameObject.FindGameObjectWithTag("Player");
		weapon = GetComponent<EnemyWeapon>();
		sprites = GetComponentsInChildren<SpriteMeshInstance>();
		timer = GameObject.Find("GameManager").GetComponent<TimerManager>();
		timer.Continuously(ShootingTimer, burstInterval);
		rotateTimer = timer.Continuously(RotateY, 3);
		shieldSprite.SetActive(shielded);
		jetpackSprite.SetActive(type == RobotType.Flying);
		originalColor = sprites[0].color;
		flashColor = Color.white;
		flashColor.a = 0.5f;

        startPoint = transform.position;
	}

	// Update is called once per frame
	void Update() {
		if (health > 0) { // Alive
			CalculateActions(); 
		}

		if (type == RobotType.Normal) {
			// Apply gravity to grounded enemies
			velocity.y += gravity * Time.deltaTime;
		} else if (type == RobotType.Flying) { 
			// Float flying robots up and down
			velocity.y = floatingAmplitude * Mathf.Sin(floatingSpeed * Time.time);
		}

		controller.Move(velocity * Time.deltaTime, false); // TODO: Platforms?

		//This stops gravity from accumulating if the controllers detects collisions above or below
		if (controller.collisions.above || controller.collisions.below) {
			if (controller.collisions.slidingDownMaxSlope) {
				velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
			} else {
				velocity.y = 0;
			}
		}
	}

	void CalculateActions() {
        print("liikelaskelma "+Vector3.Distance(transform.position, startPoint)+" " + moveDirection + " " + moveSpeed);
		// Moving
		if (Vector3.Distance(player.transform.position, transform.position) < chaseDistance && CanMove() && !kicking) {
            // Limit how far robots can move from their start position
			if ((Vector3.Distance(transform.position, startPoint) < distanceAllowance || 
				(Vector3.Distance(transform.position + new Vector3(moveDirection, 0, 0), startPoint) < Vector3.Distance(transform.position, startPoint)))) 
            {
                velocity.x = moveDirection * moveSpeed;

                if (type == RobotType.Normal)
                {
                    coreAnimator.SetBool("munaanimation", true);
                }
                else
                {
                    coreAnimator.SetBool("munafly", true);
                }
            }
		} else {
			velocity.x = 0;
			coreAnimator.SetBool("munaanimation", false);
		}

		// Shooting
		if (Vector3.Distance(player.transform.position, transform.position) < shootingDistance && !kicking) {
			canShoot = true;
		} else {
			canShoot = false;
		}

        //Kicking
        if(Vector3.Distance(player.transform.position, transform.position) < kickDistance && !kicking && !kickOnCooldown)
        {
            if (player.transform.position.x < transform.position.x && !kicking)
            {
                kickObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                kickObject.GetComponent<RobotKick>().direction = new Vector2(-1, 0);

            }
            else if(player.transform.position.x > transform.position.x && !kicking)
            {
                kickObject.transform.rotation = Quaternion.Euler(0, 0, 180);
                kickObject.GetComponent<RobotKick>().direction = new Vector2(1, 0);
            }

			if (!coreAnimator.GetCurrentAnimatorStateInfo(0).IsName("munaammus")) {
				kicking = true;
				coreAnimator.SetBool("munapotku", true);
				timer.Once(ActivateKick, kickDelay);
			}
        }
	}

	void RotateY() { // Rotates enemy sprite to face player
		if (player != null && !kicking) {
			if (transform.position.x < player.transform.position.x) {
				robotSprite.transform.rotation = new Quaternion(0, 180f, 0, 0);
			} else {
				robotSprite.transform.rotation = Quaternion.identity;
			}
			moveDirection = transform.position.x < player.transform.position.x ? Vector2.right.x : Vector2.left.x;
		}
	}
		
	bool CanMove() {
        // Stop while shooting or taking damage
        return !coreAnimator.GetCurrentAnimatorStateInfo(0).IsName("munaammus")
            && !coreAnimator.GetCurrentAnimatorStateInfo(0).IsName("munaosuma")
            && !coreAnimator.GetCurrentAnimatorStateInfo(0).IsName("munapotkuanimaatio")
            && !coreAnimator.IsInTransition(0); 
	}

	void ShootPlayer() {
		if (player != null && canShoot) {
			coreAnimator.SetBool("munaammus", true);
			weapon.Shoot(player.transform.position);
		}
	}

	void ShootingTimer() {
		if (player != null && canShoot) {
			for (int i = 0; i <= burstAmount; i++) {
				timer.Once(ShootPlayer, i + 1); 
			}
		}
	}

	void ChangeDamageable() {
		foreach (var sprite in sprites) {
			sprite.color = originalColor;
		}
		damageable = true;
	}

	#region Damageable implementation

	public bool TakeDamage(int damage) {
		print("osuma");
		health -= damage;
		hitSpark.Play();
		coreAnimator.SetTrigger("munaosuma");
		foreach (var sprite in sprites) {
			sprite.color = flashColor;
		}
		timer.Once(ChangeDamageable, .1f);
		if (health <= 0) {
			Die();
			return true;
		} else {
			return false;
		}
	}

	#endregion

	public void GetStunned(float invulTime)
    {
        damageable = false;
		timer.Once(ChangeDamageable, invulTime);
    }

    public void DestroyShield()
    {
        shielded = false;
		shieldSprite.transform.parent = null;
		shieldAnimator.SetBool("kuoretfly", true);
        print("kilpimeni");
    }

    public void ActivateKick()
    {
        
        kickObject.GetComponent<RobotKick>().duration = kickDuration;
        kickObject.SetActive(true);
        timer.Once(DeactivateKick, kickDuration);

    }

    public void DeactivateKick()
    {
        kicking = false;
        kickObject.SetActive(false);
        kickOnCooldown = true;
        timer.Once(EndKickCooldown, kickCooldown);
    }

    void EndKickCooldown()
    {
        kickOnCooldown = false;
    }

	void Die() {
		velocity.x = 0f;
        DeactivateKick();
        gameObject.layer = 26;
        canShoot = false;
		timer.RemoveTimer(rotateTimer);
		jetpackSprite.SetActive(false);
		var dieLength = 0f;
        if (type == RobotType.Normal) {
			int i = Random.Range(0, deathAnimations.Count);
			coreAnimator.SetTrigger(deathAnimations[i].name);
			dieLength = deathAnimations[i].length;
		} else {
			coreAnimator.SetTrigger("munaflydeath");
			dieLength = 0f;
		}
		// FIXME: Disabling collider drops robot into the ground while in death animation
		//        Do we need to do this?
		//GetComponent<Collider2D>().enabled = false;
		Destroy(gameObject, dieLength + fadeTime);


		StartCoroutine(FadeOut(1, 0, fadeTime, dieLength)); // Fading out sprites with coroutine for now
	}

	IEnumerator FadeOut(float startIntensity, float endIntensity, float time, float waitTime) {
		float t = 0f;
		Color original = sprites[0].color;
		yield return new WaitForSeconds(waitTime);
		while (t < time) {
			t += Time.deltaTime;
			foreach (var sprite in sprites) {
				sprite.color = new Color(original.r, original.g, original.b, Mathf.Lerp(startIntensity, endIntensity, t / time));
			}
			yield return null;
		}
	}
}
