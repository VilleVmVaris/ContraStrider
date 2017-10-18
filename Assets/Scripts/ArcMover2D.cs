using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArcMover2D : MonoBehaviour {

	public float firingAngle; // default was 45.0f;
	public float gravity;     // default was  9.8f;

	Vector2 target;
	float elapseTime;
	float flightDuration;
	float Vx;
	float Vy;

	public UnityAction TargetReached;

	// Use this for initialization
	void Awake () {
		target = Vector2.zero;
		flightDuration = int.MaxValue;
		elapseTime = 0;
		Vx = 0;
		Vy = 0;
	}

	// Update is called once per frame
	void Update () {
		if (target != Vector2.zero) {
			if (elapseTime < flightDuration) {
				// Rotate projectile
				// if necessary, or looks cool.
				// Move towards target in arc
				transform.Translate(
					Vx * Time.deltaTime,
					(Vy - (gravity * elapseTime)) * Time.deltaTime,
					0,
					Space.Self);
				elapseTime += Time.deltaTime;
			} else {
				elapseTime = 0;
				target = Vector2.zero;
				TargetReached();
			}
		}
	}

	public void SetTarget(Vector2 target) {
		this.target = target;
		var distance = Vector2.Distance(transform.position, target);
		// Velocity to throw to target in angle
		var velocity = distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);
		// Velocity X and Y components
		Vx = Mathf.Sqrt(velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
		Vy = Mathf.Sqrt(velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

		flightDuration = distance / Vx;
	}
}
