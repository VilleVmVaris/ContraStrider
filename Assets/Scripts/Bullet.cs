using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    int bulletDamage;
    float bulletSpeed;
    Vector2 bulletDirection;
    public Transform graphics;
    Transform player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        SetBulletRotation();
	}
	
	void Update () {
        transform.Translate(bulletDirection.normalized * Time.deltaTime * bulletSpeed);
	}
    public void Projectile(int damage, float speed, Vector2 direction, float destroyDelay) {
        bulletDamage = damage;
        bulletSpeed = speed;
        bulletDirection = direction;
        Destroy(gameObject, destroyDelay);
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        GameObject go = collision.gameObject;
        if (go.tag == "Player") {
            print("Player hit");
            go.GetComponent<Damageable>().TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }
    public static float AngleInRad(Vector3 vec1, Vector3 vec2) {
        return Mathf.Atan2(vec2.y - vec1.y, vec2.x - vec1.x);
    }
    public static float AngleInDeg(Vector3 vec1, Vector3 vec2) {
        return AngleInRad(vec1, vec2) * 180 / Mathf.PI;
    }
    void SetBulletRotation() {
        var angle = AngleInDeg(player.position, transform.position);
        graphics.Rotate(Vector3.forward, angle);
    }
}
