using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { Grounded, Flying }

public class Enemy : MonoBehaviour, Damageable{
    Stats stats;
    GameObject player;

    public GameObject bullet;
    public Transform bulletPosition;
    public TimerManager timer;

    bool canShoot;

    public EnemyType enemyBehaviour;
    
	void Start () {
        stats = GetComponent<Stats>();
        player = GameObject.FindGameObjectWithTag("Player");
        timer.AddTimer(ShootingTimer, stats.burstInterval, true);
    }
    
    void Update () {
        
        RotateY();
        canShoot = CheckDistanceX(transform.position, player.transform.position, stats.shootingDistance);
    }
    void ShootPlayer() {
        if (player != null && canShoot) {
            var dir = player.transform.position - bulletPosition.position;
            float angle = Vector2.Angle(transform.position, player.transform.position);
            GameObject go = Instantiate(bullet, bulletPosition.position, Quaternion.identity);
            go.GetComponent<Bullet>().Projectile(stats.damage, stats.projectileSpeed, dir);
            Destroy(go, stats.destroyDelay);
        }
    }
    
    public void TakeDamage(int damage) {
        stats.health -= damage;
    }
    void ShootingTimer() {
        if (player != null) {
            timer.AddTimer(ShootPlayer, 2);
            timer.AddTimer(ShootPlayer, 4);
            timer.AddTimer(ShootPlayer, 6);
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
}
