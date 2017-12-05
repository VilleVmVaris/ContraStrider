using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	[Header("Ninja")]
	public List<AudioSource> slashes;
	public List<AudioSource> footsteps;
	public AudioSource chargeSlash;
	public AudioSource hit;
	public AudioSource dashCharged;

	[Header("Robot")]
	public List<AudioSource> robotsteps;
	public AudioSource jetpack;
	public AudioSource explosion;
	public AudioSource shot;
	public AudioSource shieldBreaks;

	Transform player;
	float footstepTimer;
	float robotstepTimer;
	bool flyingClose = false;
	bool walkingClose = false;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (footstepTimer >= 0f) {
			footstepTimer -= Time.deltaTime;
		}		
		if (robotstepTimer >= 0f) {
			robotstepTimer -= Time.deltaTime;
		}

		// TODO: Make this a bit more generic?
		flyingClose = false;
		walkingClose = false;
		var enemies = Physics2D.OverlapCircleAll(player.position, 15f, Helpers.EnemyLayerMask);
		for (int i = 0; i < enemies.Length; i++) {
			var enemy = enemies[i].GetComponent<EggRobot>();
			if (!enemy.IsNullOrDestroyed() && enemy.type == EggRobot.RobotType.Flying) {
				flyingClose = true;		
			} else if(!enemy.IsNullOrDestroyed() && enemy.type == EggRobot.RobotType.Normal) {
				walkingClose = true;
			}
		}
		if (flyingClose && !jetpack.isPlaying) {
			jetpack.Play();
		} else if (!flyingClose && jetpack.isPlaying) {
			jetpack.Stop();
		}
	}

	public void Slash() {
		slashes[Random.Range(0, slashes.Count)].Play();
	}

	public void NinjaStep() {
		if (footstepTimer < 0f) {
			footsteps[Random.Range(0, footsteps.Count)].Play();
			footstepTimer =+ .1f;
		}

	}

	public void RobotStep() {
		if (walkingClose && robotstepTimer < 0f) {
			robotsteps[Random.Range(0, robotsteps.Count)].Play();
			robotstepTimer = +.2f;
		}

	}

}
