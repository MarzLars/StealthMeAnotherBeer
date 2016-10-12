using UnityEngine;
using System.Collections;

public class PatrollingAICharacter : AICharacterMovement
{
    public enum PatrolType
    {
        Loop,
        PingPong,
        OnceOff
    }

    [SerializeField]
    protected Transform[] waypoints;
    [SerializeField]
    protected PatrolType patrolType;

    protected int currentWayPointIndex = -1;

    private void Start()
    {
        SetTarget(FindClosestWaypoint(), TargetType.Waypoint);
    }

    protected override void EngageTarget()
    {
        base.EngageTarget();
        if (targetType == TargetType.Waypoint)
        {
            if (++currentWayPointIndex < waypoints.Length)
            {
                target = waypoints[currentWayPointIndex];
            }
            else
            {
                currentWayPointIndex = 0;
                switch (patrolType)
                {
                    case PatrolType.OnceOff:
                        {
                            target = null;
                            break;
                        }
                    case PatrolType.Loop:
                        {
                            SetTarget(waypoints[currentWayPointIndex], TargetType.Waypoint);
                            break;
                        }
                    case PatrolType.PingPong:
                        {
                            waypoints = ReversedWayPoints();
                            SetTarget(waypoints[currentWayPointIndex], TargetType.Waypoint);
                            break;
                        }
                }
            }
        }
    }

    private Transform FindClosestWaypoint()
    {
        Vector3 thisPosition = chestBody.transform.position;

        int length = waypoints.Length;
        float closest = float.MaxValue;
        int closestIndex = -1;
        for (int i = 0; i < length; i++)
        {
            float distance = Vector3.Distance(thisPosition, waypoints[i].position);
            if (distance < closest)
            {
                closest =  distance;
                closestIndex = i;
            }
        }

        if (closestIndex != -1)
        {
            currentWayPointIndex = closestIndex;
            return waypoints[closestIndex];
        }
        else
        {
            Debug.LogError("Could not find waypoiny");
            return null;
        }
    }

    private Transform[] ReversedWayPoints()
    {
        Transform[] reversed = new Transform[waypoints.Length];

        int length = waypoints.Length;
        for (int i = length - 1; i >= 0; i--)
        {
            reversed[length - i - 1] = waypoints[i];
        }

        return reversed;
    }
}
