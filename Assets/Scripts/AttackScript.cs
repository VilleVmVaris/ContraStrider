using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {

	public int damageAmount;

	public bool chargeAttack;

	public int stunTicks;

	Player player;

	// Use this for initialization
	void Start() {
		player = GetComponentInParent<Player>();
	}
	
	// Update is called once per frame
	void Update() {
		
	}

	void OnTriggerStay2D(Collider2D collision) {
		if (collision.CompareTag("Enemy")) {
			var robot = collision.gameObject.GetComponent<EggRobot>(); //TODO: Generic enemy interface?
			if (!robot.IsNullOrDestroyed()) {
				if (robot.damageable && !robot.shielded) {
					var died = robot.TakeDamage(damageAmount);
					if (died && player.dash.dashing) {
						print("Dash kill!");
						player.dash.EndCoolDown();
					}
					robot.GetStunned(stunTicks);
				} else if (robot.damageable && robot.shielded && chargeAttack) {
					robot.DestroyShield();
					robot.GetStunned(stunTicks);
				}
			}
		}
	}

}
