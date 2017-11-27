using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour {

	public GameObject boltPrefab;

	// Pooling
	List<GameObject> activeBolts;
	List<GameObject> inactiveBolts;

	int maxBolts = 5;

	// Use this for initialization
	void Start () {
		activeBolts = new List<GameObject>(); 
		inactiveBolts = new List<GameObject>();
	
		for (int i = 0; i < maxBolts; i++) {
			var bolt = Instantiate(boltPrefab, transform);
			bolt.GetComponent<LightningBolt>().Init(5);
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

		// TODO: create bolts
		if (Input.GetKeyDown(KeyCode.L)) {
			var pos1 = new Vector2(100, 100);
			var pos2 = new Vector2(-100, -100);

			pos1 = transform.InverseTransformPoint(pos1);
			pos2 = transform.InverseTransformPoint(pos2);

			CreatePooledBolt(pos1, pos2, Color.white, 1f);
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
