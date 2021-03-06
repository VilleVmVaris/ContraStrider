﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour {

	Player player;
	GameManager gm;


	// Use this for initialization
	void Start() {
		player = GetComponent<Player>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	// Update is called once per frame
	void Update() {

		if(Input.GetButtonDown("Start")) {
			if(gm.state != GameManager.Gamestate.Paused) { 
				gm.Pause();
			} else {
				gm.Unpause();
			}
		}

		if(gm.state == GameManager.Gamestate.Paused) {
			return; // Do not take player input during pause
		}

		Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		if(player.dash.aiming) {
			player.dash.Aim(directionalInput);
		} else {
			player.SetDirectionalInput(directionalInput);
		}

		if(Input.GetButtonDown("Jump")) {
			player.JumpInputDown();
		}

		if(Input.GetButtonUp("Jump")) {
			player.JumpInputUp();
		}

		if(Input.GetButtonDown("Fire1")) {
			player.Attack(directionalInput);
		}

		if(Input.GetButton("Fire1")) {
			player.timeToCharge += Time.deltaTime;

			if(player.timeToCharge >= player.timeBeforeCharge) {
				player.StartChargingSword();
			}
		}

		if(Input.GetButtonUp("Fire1")) {
			if(player.chargingSword) {
				player.ChargedAttack(directionalInput);

			} else {
				player.ResetCharge();
			}
		}

		if(Input.GetButtonDown("Fire2") && !player.dash.dashing) {
			if(!player.dash.aiming) {
				player.dash.StartAiming(directionalInput);
			}
            
		}
		if(Input.GetButtonUp("Fire2") && !player.dash.dashing) {
			player.BladeDashAttack(directionalInput);
			player.dash.DoDash();
		}

		if(directionalInput.y <= -.8f) {
			player.Crouch();
		}

		if(player.crouching && directionalInput.y > -.8f) {
			player.StandUp();
		}


	}
}


