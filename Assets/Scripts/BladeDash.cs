﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeDash : MonoBehaviour {

	public float speed;
	public int dashTicks;
	public GameObject dashArrow;
	public int coolDown;
	[HideInInspector]
	public bool onCooldown;

	[HideInInspector]
	public bool dashing = false;
	[HideInInspector]
	public bool aiming = false;
	[HideInInspector]
	public Vector2 direction;

	TimerManager timer;
	Controller2D controller;
	Animator animator;
	GUIManager gui;
    GameObject dashAttack;
	Player player;
	AudioManager sounds;

	// Use this for initialization
	void Start() {
		timer = GameObject.Find("GameManager").GetComponent<TimerManager>();
		controller = GetComponent<Controller2D>();
		animator = GetComponentInChildren<Animator>();
		dashArrow.SetActive(false);
        dashAttack = GetComponent<Player>().dashAttack;
		gui = GameObject.Find("GameCanvas").GetComponent<GUIManager>();
		player = GetComponentInParent<Player>();
		sounds = GameObject.Find("Audio").GetComponent<AudioManager>();
	}

	// Update is called once per frame
	void Update() {

	}

	public void StartAiming(Vector2 input) {
		if (!onCooldown) {
			var aimDirection = input.normalized;
			if (aimDirection == Vector2.zero) {
				aimDirection.x = controller.collisions.faceDir;
			}
			direction = aimDirection;
			Time.timeScale = 0.2f;
			Time.fixedDeltaTime = Time.timeScale * .2f;
			aiming = true;
			foreach(AnimatorControllerParameter parameter in animator.parameters) {            
				animator.SetBool(parameter.name, false);            
			}
			animator.SetBool("ninjastance", true);
			dashArrow.SetActive(true);
		}
	}

	public void Aim(Vector2 input) {
		if (!onCooldown) {

			// TODO: Aim in 8 directions

			var aimDirection = input.normalized;
			if (aimDirection == Vector2.zero) {
				aimDirection.x = controller.collisions.faceDir;
			}
			direction = aimDirection;
			dashArrow.transform.localPosition = aimDirection * 1.5f;
			// Rotate aiming arrow towards outside
			var dir = dashArrow.transform.position - transform.position;
			var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
			dashArrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
		}
	}

	public void DoDash() {
		if (!onCooldown) {
			dashArrow.SetActive(false);
			aiming = false;
			dashing = true;
			onCooldown = true;
			Time.timeScale = 1f;
			timer.Once(StopDash, dashTicks);
			timer.Once(EndCoolDown, coolDown);
			gui.ReFillDashCharge(coolDown);
			controller.dashingThroughEnemy = true;
		}
	}

	public void StopDash() {
        dashAttack.GetComponent<AttackScript>().DealDamage();
		dashing = false;
		direction = Vector2.zero;
		controller.dashingThroughEnemy = false;
		player.EndDashAttackEffect();
	}

	public void EndCoolDown() {
		gui.SetFullDashCharge();
		sounds.dashCharged.Play();
		onCooldown = false;
	}

}
