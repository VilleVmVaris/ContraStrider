using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeDash : MonoBehaviour {

	public int dashTicks;

	[HideInInspector]
	public bool dashing = false;
	[HideInInspector]
	public bool aiming = false;

	TimerManager timer;

	// Use this for initialization
	void Start () {
		timer = GameObject.Find("TimerManager").GetComponent<TimerManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartAiming() {
		Time.timeScale = 0.2f;
		Time.fixedDeltaTime = Time.timeScale * .2f;
		aiming = true;
	}

	public void DoDash() {
		dashing = true;
		Time.timeScale = 1f;
		timer.Once(StopDash, dashTicks);
	}

	public void StopDash() {
		dashing = false;
	}
		
}
