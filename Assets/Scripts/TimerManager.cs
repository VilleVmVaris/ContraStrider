using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerManager : MonoBehaviour {

	public float tickInterval;
	float timer;

	readonly List<Timer> timers = new List<Timer>();

	public struct Timer {
		public UnityAction action;
		public int ticks;
		public int tickCount;
		public bool continuous;
	}

	// Use this for initialization
	void Start () {
		tickInterval = Mathf.Max(tickInterval, 0.05f);
		timer = tickInterval;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer < 0) {
			Tick();
			timer += tickInterval;
		}
	}

	void Tick() {
		for (int i = 0; i < timers.Count; ) {
			if (timers[i].ticks == 0 && !timers[i].continuous) {
				timers.RemoveAt(i);
			} else {
				Timer t = timers[i];
				t.ticks--;
				if (t.ticks == 0) {
					if ((t.action.Target as MonoBehaviour) == null) { // Called object is deleted
						t.continuous = false;
						t.ticks = 0;
					} else {
						t.action.Invoke();	
					}
					if (t.continuous) {
						t.ticks = t.tickCount;
					}
				}
				timers[i] = t;
				i++;
			}
		}
	}

	public Timer AddTimer(UnityAction action, int ticks, bool continuous = false) {
		Timer t = new Timer();
		t.action = action;
		t.ticks = ticks;
		t.tickCount = ticks;
		t.continuous = continuous;
		timers.Add(t);
		return t;
	}

	public void RemoveTimer(Timer timer) {
		timers.Remove(timer);
	}
}
