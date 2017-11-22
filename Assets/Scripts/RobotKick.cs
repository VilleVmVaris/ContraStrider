using System.Collections;
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
        print("kickscript direction is " + direction);
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8 && collision.GetComponent<Player>() != null)
        {
            if(!collision.gameObject.GetComponent<Player>().knockedBack)
            collision.GetComponent<Player>().KnockBack(direction, force, duration);
        }
    }

}
