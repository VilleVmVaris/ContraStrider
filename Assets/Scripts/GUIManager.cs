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
	public GameObject PausePanel;

	GameManager gm;

	// Use this for initialization
	void Start() {
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
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

	public void FadeToBlack() {
		FadeImage.enabled = true;
		FadeImage.canvasRenderer.SetAlpha(0f);
		FadeImage.CrossFadeAlpha(alpha: 1.0f, duration: 5f, ignoreTimeScale: true);
	}

    public void ShowPause()
    {
		PausePanel.SetActive(true);

    }

    public void HidePause()
    {
		PausePanel.SetActive(false);
    }

	public void Restart() {
		gm.Restart();
	}

}
