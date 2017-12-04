using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour {

	public BossScript boss;
	public Image bar;
	public GameObject bossHealthPanel;
	float fullHealth;

	// Use this for initialization
	void Start () {
		fullHealth = boss.health;
	}
	
	// Update is called once per frame
	void Update () {
		if (!boss.IsNullOrDestroyed() && boss.health >= 0) {
			bar.fillAmount = boss.health / fullHealth;
			if (boss.health <= 0) {
				bossHealthPanel.SetActive(false);
			}
		}
	}
}
