﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{

    Player player;


    // Use this for initialization
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (player.dash.aiming)
        {
            player.dash.Aim(directionalInput);
        }
        else
        {
            player.SetDirectionalInput(directionalInput);
        }

        if (Input.GetButtonDown("Jump"))
        {
            player.JumpInputDown();
        }

        if (Input.GetButtonUp("Jump"))
        {
            player.JumpInputUp();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            player.Attack(directionalInput);
        }

        if (Input.GetButton("Fire1"))
        {
            player.timeToCharge += Time.deltaTime;

            if (player.timeToCharge >= player.timeBeforeCharge)
            {
                player.StartChargingSword();
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (player.chargingSword)
            {
                player.ChargedAttack(directionalInput);

            }
            else
            {
                player.ResetCharge();
            }
        }

        if (Input.GetButtonDown("Fire2") && !player.dash.dashing)
        {
            if (!player.dash.aiming)
            {
                player.dash.StartAiming(directionalInput);
            }
            
        }
		if (Input.GetButtonUp("Fire2") && !player.dash.dashing) {
			player.BladeDashAttack(directionalInput);
			player.dash.DoDash();
		}

        if (directionalInput.y <= -.8f)
        {
            player.Crouch();
        }

        if (player.crouching && directionalInput.y > -.8f)
        {
            player.StandUp();
        }

		if (Input.GetButtonDown("Start")) {
			// Just reload the scene for testing during development
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
    }
}


