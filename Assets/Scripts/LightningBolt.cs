using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : MonoBehaviour {

	public List<GameObject> activeSegments;
	public List<GameObject> inactiveSegments;

	public GameObject segmentPrefab;

	public float alpha;
	public float fadeOutRate;
	public Color tint;

	public Vector2 Start {
		get { 
			return activeSegments[0].GetComponent<LightningSegment>().start;
		}
	}

	public Vector2 End {
		get { 
			return activeSegments[activeSegments.Count - 1].GetComponent<LightningSegment>().end;
		}
	}

	public bool IsFadeOutComplete {
		get { 
			return alpha <= 0;
		}
	}

	public void Init(int maxSegments) {
		// Pool segments into lists
		activeSegments = new List<GameObject>(maxSegments);
		inactiveSegments = new List<GameObject>(maxSegments);
	
		for (int i = 0; i < maxSegments; i++) {
			GameObject segment = Instantiate(segmentPrefab, transform);
			segment.SetActive(false);
			inactiveSegments.Add(segment);
		}
	}

	public void FireBolt(Vector2 source, Vector2 destination, Color color, float thickness) {
		tint = color;
		alpha = 1.5f;
		fadeOutRate = 0.03f;

		// Set minimum size
		if (Vector2.Distance(destination, source) <= 0) {
			Vector2 adjust = Random.insideUnitCircle;
			if (adjust.magnitude <= 0) {
				adjust.x += .1f;
			}
			destination += adjust;
		}

		var slope = destination - source;
		var normal = (new Vector2(slope.y, -slope.x)).normalized;
		var distance = slope.magnitude;

		List<float> positions = new List<float>();
		positions.Add(0);

		// Generate random positions
		for (int i = 0; i < distance / 4; i++) {
			positions.Add(Random.Range(.25f, .75f));
		}

		positions.Sort();

		var sway = 80;
		var jaggies = 1 / sway;
		var spread = 1f;

		// Start at source
		var previous = source;
		var previousDisplacement = 0f;

		for (int i = 0; i < positions.Count; i++) {
			// Stop at pool size
			if (inactiveSegments.Count <= 0) {
				break;
			}

			var pos = positions[i];

			// Perpendicular variation for close positions, prevents sharp angles
			var scale = (distance * jaggies) * (pos - positions[i - 1]);
			// Points to near middle of the bolt
			var envelope = pos > .95f ? 20 * (1 - pos) : spread;

			float displacement = Random.Range(-sway, sway);
			displacement -= (displacement - previousDisplacement) * (1f - scale);
			displacement *= envelope;

			// End point
			var point = source + (pos * slope) + (displacement * normal);

			activateLineSegment(previous, point, thickness);
			previous = point;
			previousDisplacement = displacement;
		}
	}

	void activateLineSegment(Vector2 start, Vector2 end, float thickness) {
		var inactiveCount = inactiveSegments.Count;
		if (inactiveCount <= 0) {
			return;
		}
		var segment = inactiveSegments[inactiveCount - 1];
		segment.SetActive(true);
		var component = segment.GetComponent<LightningSegment>();
		component.SetColor(Color.white);
		component.start = start;
		component.end = end;
		component.thickness = thickness;
		inactiveSegments.RemoveAt(inactiveCount - 1);
		activeSegments.Add(segment);
	}

	public void DeactívateSegments() {
		for (int i = activeSegments.Count - 1; i >= 0 ; i--) {
			var segment = activeSegments[i];
			segment.SetActive(false);
			activeSegments.RemoveAt(i);
			inactiveSegments.Add(segment);
		}
	}

	public void Draw() {
		if (alpha <= 0) {
			return;
		}

		foreach (var segment in activeSegments) {
			var component = segment.GetComponent<LightningSegment>();
			component.SetColor(tint * (alpha * 0.6f));
			component.Draw();
		}
	}

	public void UpdateComponent() {
		alpha -= fadeOutRate;
	}
}
