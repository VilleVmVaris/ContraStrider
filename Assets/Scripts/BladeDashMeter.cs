using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BladeDashMeter : MonoBehaviour {

	public Image fill;
	public GameObject fullCharge;

	TimerManager tm;
	float timer;
	float time;

	// Use this for initialization
	void Start() {
		tm = GameObject.Find("GameManager").GetComponent<TimerManager>();
	}
		
	// Update is called once per frame
	void Update() {
		if (timer > 0) {
			timer -= Time.deltaTime;
			fill.fillAmount = 1f - Mathf.InverseLerp(0, time, timer);
		}
	}

	public void FillDash(int ticks) {
		fullCharge.SetActive(false);
		time = tm.tickInterval * ticks;
		timer = time;
	}

	public void FullCharge() {
		time = 0;
		timer = 0;
		fullCharge.SetActive(true);
	}
}
