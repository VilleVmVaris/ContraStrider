using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastController
{

    public LayerMask passengerMask;

    public Vector3[] localWaypoints;
    Vector3[] globalWaypoints;

    public float speed;

    int fromWayPointIndex;

    float percentBetweenPoints;

    List<PassengerMovement> passengerMovement;
    Dictionary<Transform, Controller2D> passengerDictionary = new Dictionary<Transform, Controller2D>();

    public bool Cyclic;

    [Range(0, 2)]
    public float easeAmount;

    public override void Start()
    {
        base.Start();

        globalWaypoints = new Vector3[localWaypoints.Length];

        for(int i = 0; i < localWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }

    }

    // Update is called once per frame
    void Update()
    {
        UpdaterayCastOrigins();

        Vector3 velocity = CalculatePlatformMovement();

        CalculatePassangerMovement(velocity);
        MovePassengers(true);
        transform.Translate(velocity);
        MovePassengers(false);
    }

    //Ease / slow down when approaching a waypoint

    float Ease (float x)
    {
        float a = easeAmount +1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    Vector3 CalculatePlatformMovement()
    {

        //Add a wait time for platform movement using Antti's timer?

        fromWayPointIndex %= globalWaypoints.Length;
        int toWayPointIndex = (fromWayPointIndex + 1) % globalWaypoints.Length;
        float distanceBtwWayPoints = Vector3.Distance(globalWaypoints[fromWayPointIndex], globalWaypoints[toWayPointIndex]);
        percentBetweenPoints += Time.deltaTime * speed / distanceBtwWayPoints;
        percentBetweenPoints = Mathf.Clamp01(percentBetweenPoints);
        float easedPercentBetweenWayPoints = Ease(percentBetweenPoints);

        Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWayPointIndex], globalWaypoints[toWayPointIndex], easedPercentBetweenWayPoints);

        if (percentBetweenPoints >= 1)
        {
            percentBetweenPoints = 0;
            fromWayPointIndex++;

            if (!Cyclic)
            {
                if (fromWayPointIndex >= globalWaypoints.Length - 1)
                {
                    fromWayPointIndex = 0;
                    System.Array.Reverse(globalWaypoints);
                }
            }
        }
        return newPos - transform.position; 
    }

    void MovePassengers(bool beforeMovePlatform)
    {
        foreach(PassengerMovement passenger in passengerMovement)
        {
            if(!passengerDictionary.ContainsKey(passenger.transform))
            {
                passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<Controller2D>());
            }
            if(passenger.moveBeforePlatform == beforeMovePlatform)
            {
                passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
                
            }
        }
    }

    //"Passengers" - any Controller2D being moved by this platform
    void CalculatePassangerMovement(Vector3 velocity)
    {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();
        passengerMovement = new List<PassengerMovement>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        // Vertically moving platform
        if (velocity.y != 0)
        {
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = (directionY == 1) ? velocity.x : 0;
                        float pushY = velocity.y - (hit.distance - skinWidth) * directionY;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
                    }

                }
            }

        }

        //Horizontally moving platforms

        if (velocity.x != 0)
        {
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

                if (hit)
                {
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x - (hit.distance - skinWidth) * directionX;

                        // add small downward force to passenger to make it check below itself to know that is colliding with ground, this enables jumping while pushed

                        float pushY = -skinWidth;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));
                    }

                }
            }
        }

        //If on top horizontally or downward moving platform

        if (directionY == -1 || velocity.y == 0 && velocity.x != 0)
        {
            
            float rayLength = skinWidth * 2;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

                if (hit)
                {
                   
                    if (!movedPassengers.Contains(hit.transform))
                    {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x;
                        float pushY = velocity.y;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
                    }

                }
            }
        }
    }

    struct PassengerMovement
    {
        public Transform transform;
        public Vector3 velocity;
        public bool standingOnPlatform;
        public bool moveBeforePlatform;

        public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform)
        {
            transform = _transform;
            velocity = _velocity;
            standingOnPlatform = _standingOnPlatform;
            moveBeforePlatform = _moveBeforePlatform;
        }
    }

    private void OnDrawGizmos()
    {
        if(localWaypoints != null)
        {
            Gizmos.color = Color.red;
            float size = .3f;

            for(int i = 0; i < localWaypoints.Length; i++)
            {
                Vector3 globalWayPointPosition = (Application.isPlaying) ? globalWaypoints[i] :  localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWayPointPosition - Vector3.up * size, globalWayPointPosition + Vector3.up * size);
                Gizmos.DrawLine(globalWayPointPosition - Vector3.left * size, globalWayPointPosition + Vector3.left * size);
            }
        }
    }
}