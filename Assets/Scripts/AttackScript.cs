using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {

	public int damageAmount;

	public bool chargeAttack;

	public float stunTime;

	Player player;

    public List<GameObject> hitTargets;

	// Use this for initialization
	void Start() {
		player = GetComponentInParent<Player>();
        hitTargets = new List<GameObject>();
	}



    // Update is called once per frame
    void Update() {
		
	}

	void OnTriggerStay2D(Collider2D collision) {
        /*
        if (collision.CompareTag("Enemy")) {
			var robot = collision.gameObject.GetComponent<EggRobot>(); //TODO: Generic enemy interface?
			if (!robot.IsNullOrDestroyed()) {
				if (robot.damageable && !robot.shielded) {
					var died = robot.TakeDamage(damageAmount);
					if (died && player.dash.dashing) {
						print("Dash kill!");
						player.dash.EndCoolDown();
					}
					robot.GetStunned(stunTime);
				} else if (robot.damageable && robot.shielded && chargeAttack) {
					robot.DestroyShield();
					robot.GetStunned(stunTime);
				}
			}
		}
        */
        
        if (collision.CompareTag("Enemy"))
        {
            var robot = collision.gameObject.GetComponent<EggRobot>();
            if(!robot.IsNullOrDestroyed())
            {
                if(robot.damageable)
                {
                   hitTargets.Add(collision.gameObject);

                    if(player.dash.dashing)
                    {
                        robot.GetStunned(stunTime);
                    }
                }
            }
            
        }

        if (!player.dash.dashing && hitTargets.Count != 0)
        {
            DealDamage();
        }
        if(!player.dash.dashing) { 
        gameObject.SetActive(false);
        }

    }

    public void DealDamage()
    {
        print("iik");
            foreach (var enemy in hitTargets)
            {
                if (enemy.GetComponent<EggRobot>() != null)
                {
                    var robot = enemy.GetComponent<EggRobot>();
                

                if (robot.shielded && chargeAttack)
                {
                    robot.DestroyShield();
                }
                else
                {

                    var died = robot.TakeDamage(damageAmount);


                    if (died && player.dash.dashing)
                    {
                        print("Dash kill!");
                        player.dash.EndCoolDown();
                    }

                }


                }

            }
        
        hitTargets.Clear();


        gameObject.SetActive(false);
    }
}


