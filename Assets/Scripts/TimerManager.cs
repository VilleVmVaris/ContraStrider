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
		public float time;
		public Schedule schedule;
	}

	public enum Schedule {
		Once,
		Continuous,
		During,
		OnceDelta
	}

	// Use this for initialization
	void Start () {
		tickInterval = Mathf.Max(tickInterval, 0.05f);
		timer = tickInterval;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateDeltaTimers();
		timer -= Time.deltaTime;
		if (timer < 0) {
			UpdateTimers();
			Tick();
			timer += tickInterval;
		}
	}

	void UpdateDeltaTimers() {
		for (int i = 0; i < timers.Count; i++) {
			Timer t = timers[i];
			if (!t.action.Target.IsNullOrDestroyed() && t.schedule == Schedule.OnceDelta) {
				t.time -= Time.deltaTime;
				if (t.time < 0) {
					t.action.Invoke();
					timers.RemoveAt(i);
					i--;
				} else {
					timers[i] = t;
				}
			}
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

	//FIXME: This whole thing might be ballooning out of hand a bit
	void Tick() {
		for (int i = 0; i < timers.Count; ) {
			if (timers[i].ticks == 0 && timers[i].schedule != Schedule.Continuous && timers[i].schedule != Schedule.OnceDelta) {
				timers.RemoveAt(i);
			} else {
				Timer t = timers[i];
				t.ticks--;
				if (t.ticks == 0) {
					if ((t.action.Target as MonoBehaviour) == null) { // Called object is deleted
						t.schedule = Schedule.Once;
						t.ticks = 0;
					} else if (t.schedule != Schedule.During && t.schedule != Schedule.OnceDelta) {
						t.action.Invoke();	
					}
					if (t.schedule == Schedule.Continuous) {
						t.ticks = t.tickCount;
					}
				}
				Replace(t);
				i++;
			}
		}
	}

	//FIXME: OK this is staring to get bad, this whole thing needs a rewrite
	void Replace(Timer timer) {
		for (int i = 0; i < timers.Count; i++) {
			if (timer.id == timers[i].id) {
				timers[i] = timer;
			}
		}
	}

	public System.Guid Once(UnityAction action, int ticks) {
		ticks = ticks == 0 ? 1 : ticks;
		Timer t = new Timer();
		t.id = System.Guid.NewGuid();
		t.action = action;
		t.ticks = ticks;
		t.tickCount = ticks;
		t.schedule = Schedule.Once;
		timers.Add(t);
		return t.id;
	}

	public System.Guid Once(UnityAction action, float time) {
		time = time <= 0 ? 0.1f : time;
		var t = new Timer();
		t.id = System.Guid.NewGuid();
		t.action = action;
		t.time = time;
		t.schedule = Schedule.OnceDelta;
		timers.Add(t);
		return t.id;
	}

	public System.Guid Continuously(UnityAction action, int ticksInterval) {
		ticksInterval = ticksInterval == 0 ? 1 : ticksInterval;
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
		untilTicks = untilTicks == 0 ? 1 : untilTicks;
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
