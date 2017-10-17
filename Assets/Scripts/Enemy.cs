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

	// Use this for initialization
	void Start () {
        stats = GetComponent<Stats>();
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector2(transform.position.x, Mathf.Sin(Time.time * 7));
        if (Input.GetKeyDown(KeyCode.Space)) {
            timer.AddTimer(ShootingTimer, 16, true);
        }
	}
    void ShootPlayer() {
        var dir = player.transform.position - transform.position;
        float angle = Vector2.Angle(transform.position, player.transform.position);
        GameObject go = Instantiate(bullet, bulletPosition.position, Quaternion.identity);
        go.GetComponent<Bullet>().Projectile(stats.damage, stats.projectileSpeed, dir);
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
