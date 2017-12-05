using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	[Header("Size and Offset")]
	public float cameraMinSize = 3f;
	public float minVerticalOffset;
	public float cameraSize = 5f;
	public float verticalOffset;
	public float cameraMaxSize = 7f;
	public float maxVerticalOffset;

	[Header("Movement")]
	public float lookAheadX;
	public float lookSmoothTimeX;
	public float verticalSmoothTime;
	public float zoomSmoothSpeed;
	public Vector2 focusAreaSize;
	public Transform bossLockPosition;

	Controller2D followTarget;
	FocusArea focusArea;

	Camera currentCamera;
	bool indoors = false;
	bool wideOutdoors = false;
	float cameraVerticalOffset;

	float currentLookAheadX;
	float targetLookAheadX;
	float lookAheadDirectionX;
	float smoothLookVelocityX;
	float smoothVelocityY;

	bool lookAheadStopped;
	bool bossMode = false;

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
		currentCamera = GetComponent<Camera>();
		followTarget = GameObject.Find("Player").GetComponent<Controller2D>();
		focusArea = new FocusArea(followTarget.collider.bounds, focusAreaSize);
		cameraVerticalOffset = verticalOffset;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Vector2 focusPosition;
		if (bossMode) {
			focusPosition = bossLockPosition.transform.position;
		} else {
			focusArea.Update(followTarget.collider.bounds);
			focusPosition = focusArea.center + Vector2.up * cameraVerticalOffset;
		}
		if (!Mathf.Approximately(focusArea.velocity.x, 0)) {
			lookAheadDirectionX = Mathf.Sign(focusArea.velocity.x);
			if (Mathf.Approximately(Mathf.Sign(followTarget.playerInput.x), Mathf.Sign(focusArea.velocity.x) ) 
				&& !Mathf.Approximately(followTarget.playerInput.x, 0)) {
				lookAheadStopped = false;
				targetLookAheadX = lookAheadDirectionX * lookAheadX;
			} else {
				if (!lookAheadStopped) {
					lookAheadStopped = true;
					targetLookAheadX = currentLookAheadX + (lookAheadDirectionX * lookAheadX - currentLookAheadX) / 4f;	
				}
			}
		}

		currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);
		focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
		focusPosition += Vector2.right * currentLookAheadX;

		// Move camera
		transform.position = (Vector3)focusPosition + Vector3.forward * -10;

		// Zoom camera during state transition
		if (indoors && currentCamera.orthographicSize > cameraMinSize) {
			currentCamera.orthographicSize = Mathf.Lerp(currentCamera.orthographicSize, cameraMinSize, Time.deltaTime * zoomSmoothSpeed);
			cameraVerticalOffset = minVerticalOffset;
		} else if (!indoors && !wideOutdoors && !Mathf.Approximately(currentCamera.orthographicSize, cameraSize)) {
			currentCamera.orthographicSize = Mathf.Lerp(currentCamera.orthographicSize, cameraSize, Time.deltaTime * zoomSmoothSpeed);
			cameraVerticalOffset = verticalOffset;
		} else if (wideOutdoors && currentCamera.orthographicSize < cameraMaxSize) {
			currentCamera.orthographicSize = Mathf.Lerp(currentCamera.orthographicSize, cameraMaxSize, Time.deltaTime * zoomSmoothSpeed);
			cameraVerticalOffset = maxVerticalOffset;
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color(1, 0, 0, .3f);
		Gizmos.DrawCube(focusArea.center, focusAreaSize);
	}

	public void ToggleIndoorsMode() {
		indoors = !indoors;
		wideOutdoors = false;
	}

	public void ToggleWideOutdoorsMode() {
		wideOutdoors = !wideOutdoors;
		indoors = false;
	}

	public void ActivateBossMode() {
		bossMode = true;
	}
}
