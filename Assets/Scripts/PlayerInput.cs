using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
            else
            {
				player.BladeDashAttack(directionalInput);
                player.dash.DoDash();
            }
        }

        if (directionalInput.y < -0.8)
        {
            player.Crouch();
        }

        if (directionalInput.y > -0.8)
        {
            player.StandUp();
        }

    }
}


