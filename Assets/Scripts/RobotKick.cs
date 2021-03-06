﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotKick : MonoBehaviour {

    public float force;
    public int damage;
    [HideInInspector]
    public float duration;
    [HideInInspector]
    public Vector2 direction;


	// Use this for initialization
	void Start () {
   
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8 && collision.GetComponent<Player>() != null)
        {
            if (!collision.gameObject.GetComponent<Player>().knockedBack && collision.gameObject.GetComponent<Player>().CheckCollisionStatus() && !collision.gameObject.GetComponent<Player>().dash.dashing)
            {
                print(duration);
                collision.GetComponent<Player>().KnockBack(direction, force, duration);
                collision.GetComponent<Player>().TakeDamage(damage);
            }
        }
    }

}
