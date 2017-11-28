using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour {
	
	public Transform startPoint;
	public Transform endPoint;
	public GameObject boltPrefab;

	// Pooling
	List<GameObject> activeBolts;
	List<GameObject> inactiveBolts;

	static readonly int MAX_BOLTS = 5;

	// Use this for initialization
	void Start () {
		activeBolts = new List<GameObject>(); 
		inactiveBolts = new List<GameObject>();
	
		for (int i = 0; i < MAX_BOLTS; i++) {
			var bolt = Instantiate(boltPrefab, transform);
			bolt.GetComponent<LightningBolt>().Init(25);
			bolt.SetActive(false);
			inactiveBolts.Add(bolt);
		}
	}
	
	// Update is called once per frame
	void Update () {
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

		if (Input.GetKeyDown(KeyCode.L)) {
			CreatePooledBolt(startPoint.position, endPoint.position, Color.white, .3f);
			CreatePooledBolt(startPoint.position, endPoint.position, Color.white, .5f);
			CreatePooledBolt(startPoint.position, endPoint.position, Color.white, .8f);
		}

		// Update and draw active bolts
		for (int i = 0; i < activeBolts.Count; i++) {
			activeBolts[i].GetComponent<LightningBolt>().UpdateBolt();
			activeBolts[i].GetComponent<LightningBolt>().Draw();
		}
	}

	void CreatePooledBolt(Vector2 source, Vector2 destination, Color color, float thickness) {
		if (inactiveBolts.Count > 0) {
			var bolt = inactiveBolts[inactiveBolts.Count - 1];
			bolt.SetActive(true);
			activeBolts.Add(bolt);
			inactiveBolts.RemoveAt(inactiveBolts.Count - 1);
			var component = bolt.GetComponent<LightningBolt>();
			component.FireBolt(source, destination, color, thickness);
		}
	}
}
