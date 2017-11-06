using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerManager : MonoBehaviour {

	public float tickInterval;
	float timer;

	readonly List<Timer> timers = new List<Timer>();

	public struct Timer {
		public System.Guid id;
		public UnityAction action;
		public int ticks;
		public int tickCount;
		public Schedule schedule;
	}

	public enum Schedule {
		Once,
		Continuous,
		During
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
			UpdateTimers();
			Tick();
			timer += tickInterval;
		}
	}

	void UpdateTimers() {
		for (int i = 0; i < timers.Count; i++) {
			Timer t = timers[i];
			if ((t.action.Target as MonoBehaviour) != null // Called object is deleted 
				&& t.schedule == Schedule.During 
				&& t.ticks != 0) { 
				t.action.Invoke();	
			}
		}
	}

	void Tick() {
		for (int i = 0; i < timers.Count; ) {
			if (timers[i].ticks == 0 && timers[i].schedule != Schedule.Continuous) {
				timers.RemoveAt(i);
			} else {
				Timer t = timers[i];
				t.ticks--;
				if (t.ticks == 0) {
					if ((t.action.Target as MonoBehaviour) == null) { // Called object is deleted
						t.schedule = Schedule.Once;
						t.ticks = 0;
					} else if (t.schedule != Schedule.During) {
						t.action.Invoke();	
					}
					if (t.schedule == Schedule.Continuous) {
						t.ticks = t.tickCount;
					}
				}
				timers[i] = t;
				i++;
			}
		}
	}

	public System.Guid Once(UnityAction action, int ticks) {
		Timer t = new Timer();
		t.id = System.Guid.NewGuid();
		t.action = action;
		t.ticks = ticks;
		t.tickCount = ticks;
		t.schedule = Schedule.Once;
		timers.Add(t);
		return t.id;
	}


	public System.Guid Continuously(UnityAction action, int ticksInterval) {
		Timer t = new Timer();
		t.id = System.Guid.NewGuid();
		t.action = action;
		t.ticks = ticksInterval;
		t.tickCount = ticksInterval;
		t.schedule = Schedule.Continuous;
		timers.Add(t);
		return t.id;
	}

	public System.Guid During(UnityAction action, int untilTicks) {
		Timer t = new Timer();
		t.id = System.Guid.NewGuid();
		t.action = action;
		t.ticks = untilTicks;
		t.tickCount = untilTicks;
		t.schedule = Schedule.During;
		timers.Add(t);
		return t.id;
	}

	public void RemoveTimer(System.Guid id) {
		for (int i = 0; i < timers.Count; i++) {
			if (timers[i].id == id) {
				timers.RemoveAt(i);
				i--;
			}
		}
	}
}
