using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeDash : MonoBehaviour {

	public float speed;
	public int dashTicks;
	public GameObject dashArrow;

	[HideInInspector]
	public bool dashing = false;
	[HideInInspector]
	public bool aiming = false;
	[HideInInspector]
	public Vector2 direction;

	TimerManager timer;
	Controller2D controller;

	// Use this for initialization
	void Start () {
		timer = GameObject.Find("GameManager").GetComponent<TimerManager>();
		controller = GetComponent<Controller2D>();
		dashArrow.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartAiming(Vector2 input) {
		var aimDirection = input.normalized;
		if (aimDirection == Vector2.zero) {
			direction.x = controller.collisions.faceDir;
		}
		direction = aimDirection;
		Time.timeScale = 0.2f;
		Time.fixedDeltaTime = Time.timeScale * .2f;
		aiming = true;
		dashArrow.SetActive(true);
	}

	public void Aim(Vector2 input) {

		// TODO: Aim in 8 directions

		var aimDirection = input.normalized;
		if (aimDirection == Vector2.zero) {
			return;
		}
		direction = aimDirection;
		dashArrow.transform.localPosition = aimDirection;
		// Rotate aiming arrow towards outside
		var dir = dashArrow.transform.position - transform.position;
		var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
		dashArrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.back); 
	}

	public void DoDash() {
		dashArrow.SetActive(false);
		aiming = false;
		dashing = true;
		Time.timeScale = 1f;
		timer.Once(StopDash, dashTicks);
	}

	public void StopDash() {
		dashing = false;
	}
		
}
