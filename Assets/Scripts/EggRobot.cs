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

	public bool damageable = true;

	[Header("Weapon Use")]
	public float shootingDistance;
	public int burstAmount;
	public int burstInterval;

    public bool shielded;

	[Header("Effects")]
	public Animator coreAnimator;
	public List<AnimationClip> deathAnimations;
	public float fadeTime;
	public ParticleSystem hitSpark;
	public GameObject shieldSprite;
	public Animator shieldAnimator;
	SpriteMeshInstance[] sprites;


	GameObject player;
	TimerManager timer;
	Controller2D controller;
	EnemyWeapon weapon;

	readonly float gravity = -20f;

	Vector2 velocity;
	float moveDirection;
	bool canShoot;
	System.Guid rotateTimer;

	// Use this for initialization
	void Start() {
		controller = GetComponent<Controller2D>();
		player = GameObject.FindGameObjectWithTag("Player");
		weapon = GetComponent<EnemyWeapon>();
		sprites = GetComponentsInChildren<SpriteMeshInstance>();
		timer = GameObject.Find("GameManager").GetComponent<TimerManager>();
		timer.Continuously(ShootingTimer, burstInterval);
		rotateTimer = timer.Continuously(RotateY, 3);
		if (shielded) {
			shieldSprite.SetActive(true);
		} else {
			shieldSprite.SetActive(false);
		}
	}

	// Update is called once per frame
	void Update() {
		if (health != 0) { // Alive
			CalculateActions(); 
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
		// Moving
		if (Vector3.Distance(player.transform.position, transform.position) < chaseDistance && CanMove()) {
			velocity.x = moveDirection * moveSpeed;
			coreAnimator.SetBool("munaanimation", true);
		} else {
			coreAnimator.SetBool("munaanimation", false);
			velocity.x = 0f;
		}
        //Stops flying robots from being affected by gravity 
        if (type != RobotType.Flying)
        {
            velocity.y += gravity * Time.deltaTime;
        }
		// Shooting
		if (Vector3.Distance(player.transform.position, transform.position) < shootingDistance) {
			canShoot = true;
		} else {
			canShoot = false;
		}
	}

	void RotateY() { // Rotates enemy sprite to face player
		if (player != null) {
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
		damageable = true;
	}

	#region Damageable implementation

	public bool TakeDamage(int damage) {
		print("osuma");
		health -= damage;
		hitSpark.Play();
		coreAnimator.SetTrigger("munaosuma");
		if (health <= 0) {
			Die();
			return true;
		} else {
			return false;
		}
	}

	#endregion

    public void GetStunned(int invulTicks)
    {
        damageable = false;
        timer.Once(ChangeDamageable, invulTicks);
    }

    public void DestroyShield()
    {
        shielded = false;
		shieldSprite.transform.parent = null;
		shieldAnimator.SetBool("kuoretfly", true);
    }

	void Die() {
		velocity.x = 0;
		canShoot = false;
		timer.RemoveTimer(rotateTimer);
		int i = Random.Range(0, deathAnimations.Count);
		coreAnimator.SetTrigger(deathAnimations[i].name);
		GetComponent<Collider2D>().enabled = false;
		Destroy(gameObject, deathAnimations[i].length + fadeTime);


		StartCoroutine(FadeOut(1, 0, fadeTime, deathAnimations[i].length)); // Fading out sprites with coroutine for now
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
