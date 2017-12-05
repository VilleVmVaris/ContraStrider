using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	
	[Header("Robot")]
	public AudioSource jetpack;
	public AudioSource explosion;

	Transform player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		// TODO: Make this a bit more generic?
		var playJetpack = false;
		var enemies = Physics2D.OverlapCircleAll(player.position, 15f, Helpers.EnemyLayerMask);
		for (int i = 0; i < enemies.Length; i++) {
			var enemy = enemies[i].GetComponent<EggRobot>();
			if (!enemy.IsNullOrDestroyed() && enemy.type == EggRobot.RobotType.Flying) {
				playJetpack = true;		
			}
		}
		if (playJetpack && !jetpack.isPlaying) {
			jetpack.Play();
		} else if (!playJetpack && jetpack.isPlaying) {
			jetpack.Stop();
		}
	}

}
