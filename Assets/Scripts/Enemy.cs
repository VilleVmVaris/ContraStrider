using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, Damageable{
    Stats stats;
    GameObject player;
    public GameObject bullet;
    public Transform bulletPosition;
    public TimerManager timer;
    TimerManager.Timer t;
    
	void Start () {
        stats = GetComponent<Stats>();
        player = GameObject.FindGameObjectWithTag("Player");
        timer.AddTimer(ShootingTimer, 16, true);
    }
    
    void Update () {
        transform.position = new Vector2(transform.position.x, Mathf.Sin(Time.time * 7));
	}
    void ShootPlayer() {
        if (player != null) {
            var dir = player.transform.position - transform.position;
            float angle = Vector2.Angle(transform.position, player.transform.position);
            GameObject go = Instantiate(bullet, bulletPosition.position, Quaternion.identity);
            go.GetComponent<Bullet>().Projectile(stats.damage, stats.projectileSpeed, dir);
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
}
