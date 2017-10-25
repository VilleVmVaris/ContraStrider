using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class EggRobot : MonoBehaviour, Damageable {
	
	public GameObject robotSprite;
	public int health;

	[Header("Movement")]
	public float moveSpeed;
	public float chaseDistance;

	[Header("Weapon Use")]
	public float shootingDistance;
	public int burstAmount;
	public int burstInterval;

	[Header("Effects")]
	public List<AnimationClip> deathAnimations;
	public float fadeTime;
	SpriteMeshInstance[] sprites;

	GameObject player;
	TimerManager timer;
	Animator animator;
	Controller2D controller;
	EnemyWeapon weapon;

	readonly float gravity = -20f;

	Vector2 velocity;
	bool canShoot;

	// Use this for initialization
	void Start() {
		controller = GetComponent<Controller2D>();
		player = GameObject.FindGameObjectWithTag("Player");
		weapon = GetComponent<EnemyWeapon>();
		animator = GetComponentInChildren<Animator>();
		sprites = GetComponentsInChildren<SpriteMeshInstance>();
		timer = GameObject.Find("TimerManager").GetComponent<TimerManager>();
		timer.Continuously(ShootingTimer, burstInterval);
	}

	// Update is called once per frame
	void Update() {
		RotateY();
		CalculateActions();

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
		if (Vector3.Distance(player.transform.position, transform.position) < chaseDistance && !IsShooting()) {
			var moveDirection = transform.position.x < player.transform.position.x ? Vector2.right.x : Vector2.left.x ;
			velocity.x = moveDirection * moveSpeed;
			animator.SetBool("munaanimation", true);
		} else {
			animator.SetBool("munaanimation", false);
			velocity.x = 0f;
		}
		velocity.y += gravity * Time.deltaTime;
		// Shooting
		if (Vector3.Distance(player.transform.position, transform.position) < shootingDistance) {
			canShoot = true;
		} else {
			canShoot = false;
		}
	}

	void RotateY() { // Rotates enemy sprite to face player
		if(player != null) {
			if (transform.position.x < player.transform.position.x) {
				robotSprite.transform.rotation = new Quaternion(0, 180f, 0, 0);
			} else {
				robotSprite.transform.rotation = Quaternion.identity;
			}
		}
	}

	bool IsShooting() {
		return animator.GetBool("munaammus");
	}

	void ShootPlayer() {
		if (player != null && canShoot) {
			animator.SetBool("munaammus", true);
			weapon.Shoot(player.transform.position);
		}
	}

	void ShootingTimer() {
		if (player != null && canShoot) {
			for (int i = 0; i <= burstAmount; i++) {
				timer.Once(ShootPlayer, i * 2);
			}
		}
	}

	#region Damageable implementation

	public void TakeDamage(int damage) {
        print("osuma");
		health -= damage;
		animator.SetTrigger("munaosuma");
		if(health <= 0) {
			Die();
		}
	}

	#endregion

	void Die() {
		int i = Random.Range(0, deathAnimations.Count);
		animator.SetTrigger(deathAnimations[i].name);
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
