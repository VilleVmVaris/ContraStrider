using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSegment : MonoBehaviour {

	public Vector2 start;
	public Vector2 end;
	public float thickness;

	public GameObject startCap;
	public GameObject body;
	public GameObject endCap;


	public LightningSegment(Vector2 start, Vector2 end, float thickness) {
		this.start = start;
		this.end = end;
		this.thickness = thickness;
	}

	public void SetColor(Color color) {
		startCap.GetComponent<SpriteRenderer>().color = color;
		body.GetComponent<SpriteRenderer>().color = color;
		endCap.GetComponent<SpriteRenderer>().color = color;
	}

	public void Draw() {
		var direction = end - start;
		var rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		// Scale to lenght and distance
		body.transform.localScale = new Vector3(
			100 * direction.magnitude / body.GetComponent<SpriteRenderer>().sprite.rect.width,
			thickness, 
			body.transform.localScale.z);

		startCap.transform.localScale = new Vector3(
			startCap.transform.localScale.x,
			thickness, 
			body.transform.localScale.z);

		endCap.transform.localScale = new Vector3(
			startCap.transform.localScale.x,
			thickness, 
			body.transform.localScale.z);
	
		// Rotate
		body.transform.rotation = Quaternion.Euler(0, 0, rotation);
		startCap.transform.rotation = Quaternion.Euler(0, 0, rotation);
		endCap.transform.rotation = Quaternion.Euler(0, 0, rotation);

		// Convert rotation to radians for Cos/Sin later
		rotation *= Mathf.Deg2Rad;

		// Store adjustment value
		var bodyWorldAdjustment = body.transform.localScale.x * body.GetComponent<SpriteRenderer>().sprite.rect.width / 2f;
		var startWorldAdjustment = startCap.transform.localScale.x * startCap.GetComponent<SpriteRenderer>().sprite.rect.width / 2f;
		var endWorldAdjustment = endCap.transform.localScale.x * endCap.GetComponent<SpriteRenderer>().sprite.rect.width / 2f;
	
		// Adjust child positions
		body.transform.position += new Vector3(
			.01f * Mathf.Cos(rotation) * bodyWorldAdjustment,
			.01f * Mathf.Sin(rotation) * bodyWorldAdjustment,
			0);

		startCap.transform.position += new Vector3(
			.01f * Mathf.Cos(rotation) * startWorldAdjustment,
			.01f * Mathf.Sin(rotation) * startWorldAdjustment,
			0);

		endCap.transform.position += new Vector3(
			.01f * Mathf.Cos(rotation) * bodyWorldAdjustment * 2f,
			.01f * Mathf.Sin(rotation) * bodyWorldAdjustment * 2f,
			0);
		endCap.transform.position += new Vector3(
			.01f * Mathf.Cos(rotation) * endWorldAdjustment,
			.01f * Mathf.Sin(rotation) * endWorldAdjustment,
			0);
	}
}
