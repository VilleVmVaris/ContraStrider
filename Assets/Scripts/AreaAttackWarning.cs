using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttackWarning : MonoBehaviour {

	ParticleSystem[] effects;

	// Use this for initialization
	void Start() {
		effects = GetComponentsInChildren<ParticleSystem>();
		Stop();
	}

	public void Play() {
		foreach (var effect in effects) {
			effect.Play();
		}
	}

	public void Stop() {
		foreach (var effect in effects) {
			effect.Stop();
		}
	}

}
