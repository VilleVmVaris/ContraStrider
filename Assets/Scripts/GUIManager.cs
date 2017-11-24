using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour {

	public HealthBarScript HealthBar;
	public ScoreScript Score;
	public CheckpointScript Checkpoint;
	public BladeDashMeter BladeDash;

	// Use this for initialization
	void Start() {
		
	}
	
	// Update is called once per frame
	void Update() {
		
	}


	public void SetFullHealth() {
		HealthBar.SetFullHealth();
	}

	public void DoubleScoreFactor() {
		Score.scoreFactor = 2; //(Timelimit?)
	}

	public void ReFillDashCharge(int ticks) {
		BladeDash.FillDash(ticks);
	}

	public void SetFullDashCharge() {
		BladeDash.FullCharge();
	}

}
