using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponFireMode { Burst, Sniper, Shotgun }

public class EnemyWeapon : MonoBehaviour {
	
	public GameObject bulletPrefab;
	public Transform bulletSpawner;
	[Header("Projectile Attributes")]
	public int damage;
	public float speed;
	public float lifetime;

    // TODO: Implement fire modes
    public WeaponFireMode weapon;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Shoot(Vector3 playerPosition) {
		var dir = playerPosition - bulletSpawner.position;
		if(weapon == WeaponFireMode.Burst) {
			GameObject bullet = Instantiate(bulletPrefab, bulletSpawner.position, Quaternion.identity);
			bullet.GetComponent<Bullet>().Projectile(damage, speed, dir, lifetime);
		}
		if(weapon == WeaponFireMode.Shotgun) {
            Vector2 shotty;
            shotty.y = -1;
            for(int i = 0; i < 3; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, bulletSpawner.position, Quaternion.identity);
                bullet.GetComponent<Bullet>().Projectile(damage, speed, new Vector3(dir.x, dir.y - shotty.y), lifetime);
                shotty.y += 1;
                
            }

		}
	}

}
