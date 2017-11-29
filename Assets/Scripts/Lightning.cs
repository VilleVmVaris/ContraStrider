using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour {
	
	public Transform origin;
	public Transform testTarget;
	public GameObject boltPrefab;

	// Pooling
	List<GameObject> activeBolts;
	List<GameObject> inactiveBolts;

	List<StrikeTarget> targets;

	TimerManager timer;

	static readonly int MAX_BOLTS = 50;

	struct StrikeTarget {
		public Vector3 target;
		public List<GameObject> bolts;
	}

	// Use this for initialization
	void Start() {
		timer = GameObject.Find("GameManager").GetComponent<TimerManager>();
		activeBolts = new List<GameObject>(); 
		inactiveBolts = new List<GameObject>();
		targets = new List<StrikeTarget>();

		for (int i = 0; i < MAX_BOLTS; i++) {
			var bolt = Instantiate(boltPrefab, transform);
			bolt.GetComponent<LightningBolt>().Init(25);
			bolt.SetActive(false);
			inactiveBolts.Add(bolt);
		}
	}
	
	// Update is called once per frame
	void Update() {
		GameObject bolt;
		LightningBolt component;

		int activeBoltsCount = activeBolts.Count;

		// Deactivate faded bolts
		for (int i = activeBoltsCount - 1; i >= 0; i--) {
			bolt = activeBolts[i];
			component = bolt.GetComponent<LightningBolt>();
			if (component.IsFadeOutComplete) {
				component.DeactívateSegments();
				bolt.SetActive(false);
				activeBolts.RemoveAt(i);
				inactiveBolts.Add(bolt);
			}
		}

		// *** Testing ***
		if (Input.GetKeyDown(KeyCode.L)) {
			CreatePooledBolt(origin.position, testTarget.position, Color.white, .3f);
			CreatePooledBolt(origin.position, testTarget.position, Color.white, .5f);
			CreatePooledBolt(origin.position, testTarget.position, Color.white, .8f);
		}

		// Some hacks to continue lightning until disabled
		if (targets.Count > 0) {
			foreach (var strikeTarget in targets) {
				for (int i = strikeTarget.bolts.Count - 1; i >= 0 ; i--) {
					if (!strikeTarget.bolts[i].activeSelf) {
						strikeTarget.bolts.RemoveAt(i);
					}
				}
				if (strikeTarget.bolts.Count < 3) {
					strikeTarget.bolts.Add(CreatePooledBolt(origin.position, strikeTarget.target, Color.white, .3f));
					strikeTarget.bolts.Add(CreatePooledBolt(origin.position, strikeTarget.target, Color.white, .5f));
					strikeTarget.bolts.Add(CreatePooledBolt(origin.position, strikeTarget.target, Color.white, .8f));
				}
			}	
		}

		// Update and draw active bolts
		for (int i = 0; i < activeBolts.Count; i++) {
			activeBolts[i].GetComponent<LightningBolt>().UpdateBolt();
			activeBolts[i].GetComponent<LightningBolt>().Draw();
		}
	}

	public void Strike(Vector3 target) {
		var strike = new StrikeTarget();
		strike.target = target;
		strike.bolts = new List<GameObject>();
		targets.Add(strike);
	}

	public void Stop() {
		targets.Clear();
	}

	GameObject CreatePooledBolt(Vector2 source, Vector2 destination, Color color, float thickness) {
		if (inactiveBolts.Count > 0) {
			var bolt = inactiveBolts[inactiveBolts.Count - 1];
			bolt.SetActive(true);
			activeBolts.Add(bolt);
			inactiveBolts.RemoveAt(inactiveBolts.Count - 1);
			var component = bolt.GetComponent<LightningBolt>();
			component.FireBolt(source, destination, color, thickness);
			return bolt;
		}
		return null;
	}
}
