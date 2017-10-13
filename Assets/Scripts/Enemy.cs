using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, Damageable{
    Stats stats;
    GameObject player;
    public GameObject bullet;
    public Transform bulletPosition;
	// Use this for initialization
	void Start () {
        stats = GetComponent<Stats>();
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector2(transform.position.x, Mathf.Sin(Time.time * 7));
        if (Input.GetKeyDown(KeyCode.Space)) {
            ShootPlayer();
        }
	}
    void ShootPlayer() {
        var dir = player.transform.position - transform.position;
        GameObject go = Instantiate(bullet, bulletPosition.position, Quaternion.identity);
        go.GetComponent<Bullet>().Projectile(stats.damage, stats.projectileSpeed, dir);
    }
    public void TakeDamage(int damage) {
        stats.health -= damage;
    }
}
