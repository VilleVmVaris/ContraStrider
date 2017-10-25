using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Player))]
public class PlayerInput : MonoBehaviour {

    Player player;

	// Use this for initialization
	void Start () {
        player = GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        player.SetDirectionalInput(directionalInput);


        if(Input.GetKeyDown(KeyCode.Space))
        {
            player.JumpInputDown();
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            player.JumpInputUp();
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            player.Attack(directionalInput);
        }

		if (Input.GetKeyDown(KeyCode.LeftShift)) {
			if (!player.dash.aiming) {
				player.dash.StartAiming();	
			} else {
				player.dash.DoDash();
			}

		}
    }



}
