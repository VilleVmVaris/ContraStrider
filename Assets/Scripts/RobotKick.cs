using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotKick : MonoBehaviour {

    public float force;
    Vector2 knockDir;
    public float directionX;
    public float directionY;

	// Use this for initialization
	void Start () {
        knockDir = new Vector2(directionX, directionY);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            collision.GetComponent<Player>().KnockBack(knockDir, force);
        }
    }

}
