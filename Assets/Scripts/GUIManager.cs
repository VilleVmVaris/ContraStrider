using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

	public HealthBarScript HealthBar;
	public ScoreScript Score;
	public CheckpointScript Checkpoint;
	public BladeDashMeter BladeDash;
	public BossHealthBar BossHealthBar;
	public Image FadeImage;
    public Image PauseImage;
    public GameObject Resume;
    public GameObject Controls;
    public GameObject Quit;

	// Use this for initialization
	void Start() {
		
	}
	
	// Update is called once per frame
	void Update() {
		
	}


	public void SetFullHealth() {
		HealthBar.SetFullHealth();
	}

	public void SetHealth(int health) {
		HealthBar.SetHealthBar(health);
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

	public void ShowBossHealthBar(BossScript boss) {
		if (!BossHealthBar.bossHealthPanel.activeSelf) {
			BossHealthBar.bossHealthPanel.SetActive(true);
			BossHealthBar.boss = boss;	
		}
	}

	public void FadeToBlack(float duration) {
		FadeImage.enabled = true;
		FadeImage.canvasRenderer.SetAlpha(0f);
		FadeImage.CrossFadeAlpha(1.0f, duration, true);
	}

    public void ShowPause()
    {
        PauseImage.enabled = true;
        Resume.SetActive(true);
        Controls.SetActive(true);
        Quit.SetActive(true);

    }

    public void HidePause()
    {
        PauseImage.enabled = false;
        Resume.SetActive(false);
        Controls.SetActive(false);
        Quit.SetActive(false);
    }

}
