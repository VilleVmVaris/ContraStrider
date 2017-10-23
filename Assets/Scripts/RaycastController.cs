using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour {

    [HideInInspector]
    public float horizontalRaySpacing;

    [HideInInspector]
    public float verticalRaySpacing;

    [HideInInspector]
    public BoxCollider2D collider;

    [HideInInspector]
    public RaycastOrigins raycastOrigins;

    public const float skinWidth = .015f;

    const float dstBetweenRays = .25f;

    [HideInInspector]
    public int horizontalRayCount;

    [HideInInspector]
    public int verticalRayCount;


    public LayerMask collisionMask;

    public virtual void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

	public virtual void Start() {
		CalculateRaySpacing();
	}

    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    //Adds the raycast origin points to the corners of the collider
    public void UpdaterayCastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }


    //Distripute raycasts evenly across player's own collider
    public void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        horizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);



        //horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        //verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);
    }
}
