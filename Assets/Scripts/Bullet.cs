using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    int bulletDamage;
    float bulletSpeed;
    Vector2 bulletDirection;
    public Transform graphics;
    Transform player;
    bool deflected;
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
        if (go.layer == 8) {
            if(go.tag != "Attack" && go.GetComponent<Player>() != null) { 
            
            go.GetComponent<Damageable>().TakeDamage(bulletDamage);
            Destroy(gameObject);
            }

        } else if(go.layer == 11)
        {
            if (deflected)
            {
                if(go.GetComponent<EggRobot>() != null) {

                    if(!go.GetComponent<EggRobot>().shielded) { 

                    go.GetComponent<EggRobot>().TakeDamage(1);
                    }

                    else
                    {
                        Destroy(gameObject);
                    }

                } else if(go.GetComponent<BossScript>() != null)
                {
                    go.GetComponent<BossScript>().TakeDamage(2);
                }
            }
        }

        else if(go.layer != 11) {
        
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

    public void GetDeflected(Vector2 direction)
    {
        deflected = true;

        bulletDirection = direction;
        
        bulletSpeed = bulletSpeed * 2;
    }

    public bool SeeIfDeflected()
    {
        return deflected;
    }


}
