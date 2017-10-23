﻿using System.Collections;
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

    public EnemyType enemyType;
    public EnemyWeapon enemyWeapon;
    
	void Start () {
        stats = GetComponent<Stats>();
        player = GameObject.FindGameObjectWithTag("Player");
        startPosition = transform.position;
        timer.AddTimer(ShootingTimer, stats.burstInterval, true);
    }
    
    void Update () {
        Move();
        RotateY();
        //if (Input.anyKeyDown && stats.health > 0) {
        //    TakeDamage(1);
        //}
    }
    void ShootPlayer() {
        if (player != null && canShoot && stats.health > 0) {
            var dir = player.transform.position - bulletPosition.position;
            anim.Play("munaammus");
            if(enemyWeapon == EnemyWeapon.Burst) {
                GameObject go = Instantiate(bullet, bulletPosition.position, Quaternion.identity);
                go.GetComponent<Bullet>().Projectile(stats.damage, stats.projectileSpeed, dir, stats.bulletDestroyDelay);
            }
            if(enemyWeapon == EnemyWeapon.Shotgun) {
                
            }
        }
    }
    void ShootingTimer() {
        if (player != null) {
            for (int i = 0; i <= stats.burstAmount; i++) {
                timer.AddTimer(ShootPlayer, i * 2);
            }
        }
    }
    void Move() {
        if (stats.health > 0) {
            canShoot = CheckDistanceX(transform.position, player.transform.position, stats.shootingDistance);
            Vector2 offset = transform.position - player.transform.position;
            Vector2 startPosOffset = transform.position - startPosition;
            if (enemyType == EnemyType.Grounded) {
                if (CheckDistanceX(startPosition, player.transform.position, stats.chaseDistance)) {
                    if (canShoot && Mathf.Abs(offset.x) > stats.shootingDistance / 2) {
                        transform.Translate(Vector2.left * Time.deltaTime * stats.moveSpeed / 2);
                        anim.SetBool("munaanimation", true);
                    } else if (canShoot) {
                        anim.SetBool("munaanimation", false);
                    } else {
                        transform.Translate(Vector2.left * Time.deltaTime * stats.moveSpeed);
                        anim.SetBool("munaanimation", true);
                    }
                } else if (Mathf.Abs(startPosOffset.x) > stats.chaseDistance / 3 && !canShoot) {
                    transform.Translate(Vector2.right * Time.deltaTime * stats.moveSpeed);
                    anim.SetBool("munaanimation", false);
                }
            }
        }
    }

    public void TakeDamage(int damage) {
        stats.health -= damage;
        anim.SetTrigger("munaosuma");
        if(stats.health <= 0) {
            Die();
        }
    }
    
    void RotateY() { // Rotates enemy to face player
        if(player != null && stats.health > 0) {
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
        int i = Random.Range(0, deathAnimations.Length);
        anim.SetTrigger(deathAnimations[i].name);
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
