using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamageScript : MonoBehaviour {

    DamageSource damager;

	// Use this for initialization
	void Start () {

       damager = GetComponentInParent<DamageSource>();

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8 && collision.GetComponent<Player>() != null)
        {
            print("täällä se on");
            if(!damager.damageTargets.Contains(collision.gameObject))
            { 
            damager.damageTargets.Add(collision.gameObject);
            }
        }
    }
}
