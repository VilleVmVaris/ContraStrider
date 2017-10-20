using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : RaycastController {
    
    public float maxClimbAngle = 80;
    public float maxDescentAngle = 75;

    Vector2 playerInput;

    [HideInInspector]
    public bool canFallThrough;


    public CollisionInfo collisions;

    public override void Start()
    {
        base.Start();

        collisions.faceDir = 1;
    }

    // Overload for the platformmover, so that it doesn't look for the input variable
    public void Move(Vector3 velocity, bool standingOnPlatform)
    {
        Move(velocity, Vector2.zero, standingOnPlatform);
    }
    //Moves the player after checking with raycasts that there are no collisions in the direction where the player is headed
    public void Move(Vector2 velocity, Vector2 input, bool standingOnPlatform = false)
    {
        UpdaterayCastOrigins();
        collisions.Reset();
        collisions.velocityOld = velocity;
        playerInput = input;

        //Change the way that the character is facing
        if(velocity.x != 0)
        {
            collisions.faceDir = (int) Mathf.Sign(velocity.x);
        }

        if (velocity.y < 0)
        {
            DescentSlope(ref velocity);
        }

        HorizontalCollisions(ref velocity);
        
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);

        if(standingOnPlatform)
        {
            collisions.below = true;
        }


    }

    //Raycasts detect colliders with layer "Obstacle" and bring velocity to 0 when next to them
    void VerticalCollisions(ref Vector2 velocity)
    {

        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.green);

            if (hit)
            {
                //Check is platform is passable

                if(hit.collider.tag == "PassablePlatform")
                {
                    if(directionY == 1 || hit.distance == 0)
                    {
                        continue;
                    }

                    if(collisions.fallingThroughPlatform)
                    {
                        continue;
                    }

                    if(playerInput.y == -1)
                    {
                        canFallThrough = true;
                       // collisions.fallingThroughPlatform = true;
                       
                       //continue;

                    } else
                    {
                        canFallThrough = false;
                    }
                    
                    
                } else if ((hit && hit.collider.tag != "PassablePlatform"))
                {
                    canFallThrough = false;
                   
                }

                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if (collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
        //Check for new slopes while already climbing one to stop jitters
        if(collisions.climbingSlope)
        {
            float directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if(hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if(slopeAngle != collisions.slopeAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }

    }

   
    void HorizontalCollisions(ref Vector2 velocity)
    {
        //Check for collisions where character is facing
        float directionX = collisions.faceDir;
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        if(Mathf.Abs(velocity.x) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            // Check which way character is facing before starting raycasts
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;

            //Check spacing of raycasts
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);

            // ... raycast
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.green);

            //Check for collisions
            if (hit)
            {
                if(hit.distance == 0)
                {
                    continue;
                }

                if(playerInput.y == -1)
                {
                    continue;
                }
                //Check for angle of hit surface from bottom ray - is the character on a slope? Compare player's normal and global up to find the angle

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if(i == 0 && slopeAngle <= maxClimbAngle)
                {

                    //Check if player moves from  climbing a slope to descending one, and retain velocity to remove slowdown

                    if(collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        velocity = collisions.velocityOld;
                    }

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

    void ClimbSlope(ref Vector2 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y <= climbmoveAmountY)
        {
            velocity.y = climbmoveAmountY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    void DescentSlope(ref Vector2 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);

        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if(hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

            if(slopeAngle != 0 && slopeAngle <= maxDescentAngle)
            {
                if(Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }



    //Contains information on locations of collisions

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector2 velocityOld;
        public bool fallingThroughPlatform;

        //Used for remembering which way the character is facing
        public int faceDir;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }

    }

}
