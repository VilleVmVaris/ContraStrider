using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public float verticalOffset;
	public float lookAheadX;
	public float lookSmoothTimeX;
	public float verticalSmoothTime;
	public Vector2 focusAreaSize;

	Controller2D followTarget;
	FocusArea focusArea;

	float currentLookAheadX;
	float targetLookAheadX;
	float lookAheadDirectionX;
	float smoothLookVelocityX;
	float smoothVelocityY;

	struct FocusArea {
		public Vector2 center;
		public Vector2 velocity;
		float left, right, top, bottom;

		public FocusArea(Bounds targetBounds, Vector2 size) {
			left = targetBounds.center.x - size.x/2;
			right = targetBounds.center.x + size.x/2;
			bottom = targetBounds.min.y;
			top = targetBounds.min.y + size.y;
			velocity = Vector2.zero;
			center = new Vector2((left+right)/2,(top+bottom)/2);
		}

		public void Update(Bounds targetBounds){
			float shiftX = 0;
			if (targetBounds.min.x < left) {
				shiftX = targetBounds.min.x - left;
			} else if (targetBounds.max.x > right) {
				shiftX = targetBounds.max.x - right;
			}
			left += shiftX;
			right += shiftX;
			float shiftY = 0;
			if (targetBounds.min.y < bottom) {
				shiftY = targetBounds.min.y - bottom;
			} else if (targetBounds.max.y > top) {
				shiftY = targetBounds.max.y - top;
			}
			top += shiftY;
			bottom += shiftY;
			center = new Vector2((left+right)/2,(top+bottom)/2);
			velocity = new Vector2(shiftX,shiftY);
		}
	}


	// Use this for initialization
	void Start () {
		followTarget = GameObject.Find("Player").GetComponent<Controller2D>();
		focusArea = new FocusArea(followTarget.GetComponent<Collider2D>().bounds, focusAreaSize);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//TODO: Public collider in controller!
		focusArea.Update(followTarget.GetComponent<Collider2D>().bounds);


		Vector2 focusPosition = focusArea.center + Vector2.up * verticalOffset;
		if (!Mathf.Approximately(focusArea.velocity.x, 0)) {
			lookAheadDirectionX = Mathf.Sign(focusArea.velocity.x);
		}

		targetLookAheadX = lookAheadDirectionX * lookAheadX;
		currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);
		focusPosition += Vector2.right * currentLookAheadX;

		transform.position = (Vector3)focusPosition + Vector3.forward * -10;
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color(1, 0, 0, .5f);
		Gizmos.DrawCube(focusArea.center, focusAreaSize);
	}
}
