using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    int bulletDamage;
    float bulletSpeed;
    Vector2 bulletDirection;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(bulletDirection.normalized * Time.deltaTime * bulletSpeed);
	}
    public void Projectile(int damage, float speed, Vector2 direction) {
        bulletDamage = damage;
        bulletSpeed = speed;
        bulletDirection = direction;
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject go = collision.gameObject;
        if(go.tag == "Player") {
            print("Player hit");
            go.GetComponent<Damageable>().TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }
}
