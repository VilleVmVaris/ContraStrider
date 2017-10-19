using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public enum EnemyType { Grounded, Flying }
public enum EnemyWeapon { Burst, Sniper, Shotgun }

public class Enemy : MonoBehaviour, Damageable {
    Stats stats;
    GameObject player;
    Vector3 startPosition;

    public GameObject bullet;
    public Transform bulletPosition;
    public TimerManager timer;
    public Animator anim;
    public AnimationClip[] deathAnimations;
    public SpriteMeshInstance[] sprites;

    bool canShoot;
    float fadeTime = 5f;
    int deathAnimation;

    public EnemyType enemyBehaviour;
    public EnemyWeapon weaponType;
    
	void Start () {
        stats = GetComponent<Stats>();
        player = GameObject.FindGameObjectWithTag("Player");
        startPosition = transform.position;
        timer.AddTimer(ShootingTimer, stats.burstInterval, true);
    }
    
    void Update () {
        if(stats.health > 0) {
            Move();
            RotateY();
        }
        if (Input.anyKeyDown && stats.health > 0) {
            TakeDamage(1);
        }
    }
    void ShootPlayer() {
        if (player != null && canShoot && stats.health > 0) {
            //anim.Play("munaammus");
            anim.SetTrigger("munaammus");
            var dir = player.transform.position - bulletPosition.position;
            GameObject go = Instantiate(bullet, bulletPosition.position, Quaternion.identity);
            go.GetComponent<Bullet>().Projectile(stats.damage, stats.projectileSpeed, dir, stats.bulletDestroyDelay);
        }
    }
    
    void Move() {
        canShoot = CheckDistanceX(transform.position, player.transform.position, stats.shootingDistance);
        Vector2 offset = transform.position - player.transform.position;
        Vector2 startPosOffset = transform.position - startPosition;
        if (enemyBehaviour == EnemyType.Flying) {
            if (CheckDistanceX(startPosition, player.transform.position, stats.chaseDistance)) {
                if (canShoot && Mathf.Abs(offset.x) > stats.shootingDistance / 2) {
                    transform.Translate(Vector2.left * Time.deltaTime * stats.moveSpeed / 2);
                    anim.SetBool("munaanimation", true);
                } else if (canShoot){
                    anim.SetBool("munaanimation", false);
                } else if (!canShoot) {
                    transform.Translate(Vector2.left * Time.deltaTime * stats.moveSpeed);
                    anim.SetBool("munaanimation", true);
                }
            } else if (Mathf.Abs(startPosOffset.x) > stats.chaseDistance / 2 && !canShoot) {
                anim.SetBool("Firing", false);
                transform.Translate(Vector2.right * Time.deltaTime * stats.moveSpeed);
                anim.SetBool("munaanimation", false);
            }
        }
    }

    public void TakeDamage(int damage) {
        stats.health -= damage;
        //anim.Play("munaosuma");
        anim.SetTrigger("munaosuma");
        if(stats.health <= 0) {
            Die();
        }
    }
    void ShootingTimer() {
        if (player != null) {
            for(int i = 0; i <= stats.burstAmount; i++) {
                timer.AddTimer(ShootPlayer, i * 2);
            }
        }  
    }
    void RotateY() {
        if(player != null) {
            if (transform.position.x < player.transform.position.x) {
                transform.rotation = new Quaternion(0, 180f, 0, 0);
            } else {
                transform.rotation = Quaternion.identity;
            }
        }
    }
    bool CheckDistanceX(Vector2 vec1, Vector2 vec2, float distance) {
        var offset = vec1 - vec2;
        return Mathf.Abs(offset.x) < distance ? true : false;
    }
    void Die() {
        deathAnimation = Random.Range(0, deathAnimations.Length);
        anim.SetTrigger(deathAnimations[deathAnimation].name);
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, deathAnimations[deathAnimation].length + fadeTime);
        StartCoroutine(FadeOut(1, 0, fadeTime)); // Fading out sprites with coroutine for now
    }
    IEnumerator FadeOut(float startIntensity, float endIntensity, float time) {
        float t = 0f;
        Color original = sprites[0].color;
        yield return new WaitForSeconds(deathAnimations[deathAnimation].length);
        while (t < time) {
            t += Time.deltaTime;
            foreach (var sprite in sprites) {
                sprite.color = new Color(original.r, original.g, original.b, Mathf.Lerp(startIntensity, endIntensity, t / 2));
            }
            yield return null;
        }
    }
}
