using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {

    public int damageAmount;

    public bool chargeAttack;

    public int stunTicks;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11 && collision.gameObject.GetComponent<EggRobot>().damageable && !collision.gameObject.GetComponent<EggRobot>().shielded)
        {
            collision.GetComponent<EggRobot>().TakeDamage(damageAmount);
            collision.GetComponent<EggRobot>().GetStunned(stunTicks);

        } else if(collision.gameObject.layer == 11 && collision.gameObject.GetComponent<EggRobot>().damageable && collision.gameObject.GetComponent<EggRobot>().shielded)
        {
            collision.gameObject.GetComponent<EggRobot>().DestroyShield();
        }
    }
}
