﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour {

    const float skinWidth = .015f;

    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    public float maxClimbAngle = 80;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    BoxCollider2D collider;
    RaycastOrigins raycastOrigins;

    public LayerMask collisionMask;

    public CollisionInfo collisions;

    // Use this for initialization
    void Start () {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }


    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    //Adds the raycast origin points to the corners of the collider
    void UpdaterayCastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }


    //Distripute raycasts evenly across player's own collider
    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    //Moves the player after checking with raycasts that there are no collisions in the direction where the player is headed
    public void Move(Vector3 velocity)
    {
        collisions.Reset();
        UpdaterayCastOrigins();
        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }

        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }

    //Raycasts detect colliders with layer "Obstacle" and bring velocity to 0 when next to them
    void VerticalCollisions(ref Vector3 velocity)
    {

        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                //Recalculate velocity.x if there are collisions along a slope to stop jittering
                if(collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                //if the player collides with something, this sets the collisions to true based on the direction that the player is moving
                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }

    }

   
    void HorizontalCollisions(ref Vector3 velocity)
    {

        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);


            if (hit)
            {

                //Check for angle of hit surface from bottom ray - is the character on a slope? Compare player's normal and global up to find the angle

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if(i == 0 && slopeAngle <= maxClimbAngle)
                {
                    print(slopeAngle);

                    float distanceToSlopeStart = 0;

                    //Make sure that we are actually on the slope before starting to climb it
                    if(slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;

                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                }
                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {

                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    //Make sure that velocity.y is getting updated if collisions are detected when climbing slope to stop jittering
                    if(collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    //if the player collides with something, this sets the collisions to true based on the direction that the player is moving
                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }

            }
        }

    }

    //Use velocity in X-axis as total distance up the slope - use target distance and slopeAngle to find the velocity in both axis while climbing
    //Set collisions.below to true to enable jumping, since normally vertical movement stops jumping
    //Also - see if velocity in Y is higher than the climb velocity to see if player is jumping

    void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y <= climbVelocityY)

        {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }

    }

    //Contains information on locations of collisions

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public bool climbingSlope;
        public float slopeAngle, slopeAngleOld;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }

    }

}
