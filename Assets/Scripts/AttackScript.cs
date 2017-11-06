using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {

	public int damageAmount;

	public bool chargeAttack;

	public int stunTicks;

	// Use this for initialization
	void Start() {
		
	}
	
	// Update is called once per frame
	void Update() {
		
	}

	void OnTriggerStay2D(Collider2D collision) {
		if (collision.CompareTag("Enemy")) {
			var robot = collision.gameObject.GetComponent<EggRobot>(); //TODO: Generic enemy interface?
			if (!robot.IsNullOrDestroyed()) {
				if (robot.damageable && !robot.shielded) {
					robot.TakeDamage(damageAmount);
					robot.GetStunned(stunTicks);
				} else if (robot.damageable && robot.shielded) {
					robot.DestroyShield();
				}
			}
		}
	}

}
