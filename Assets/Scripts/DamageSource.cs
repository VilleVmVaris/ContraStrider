using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour {

    public int damage;

    TimerManager timer;

    public int damageInterval;

    public List<GameObject> damageTargets;

    public List<GameObject> attackAreas;

    // Use this for initialization
    void Start () {

        damageTargets = new List<GameObject>();

        attackAreas = new List<GameObject>();

        foreach(Transform child in transform)
        {
            attackAreas.Add(child.gameObject);
        }

        timer = GameObject.Find("GameManager").GetComponent<TimerManager>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		
        if(damageTargets.Count != 0)
        {
            timer.Continuously(DealDamage, damageInterval);
        }

        damageTargets.Clear();

	}


    void DealDamage()
    {
        foreach(var player in damageTargets)
        {
            if(player.GetComponent<Player>().health > 0) { 
            player.GetComponent<Player>().TakeDamage(damage);
            }
        }
    }
}
