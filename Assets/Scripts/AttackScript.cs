using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {

    BoxCollider2D boxcollider;
    public float maxAttackRange;

    Vector2 attackDirection;


    public LayerMask wallMask;




	// Use this for initialization
	void Start () {

        boxcollider = GetComponent<BoxCollider2D>();
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.K))
        {
            Attack();
        }
        
    }


    public void Attack()
    {


        if(Physics2D.Raycast(transform.position, Vector2.right, maxAttackRange, wallMask))
        {
            Debug.DrawRay(transform.position, Vector2.right, Color.blue);
        }


    }
}
