using UnityEngine;

public class CirclePathMovementNVLvl4 : MonoBehaviour
{
    public Transform[] waypoints;
    public float timeToNextPoint = 2.0f;
    private int currentWaypointIndex = 0;
    private int direction = 1;

    private float elapsedTime = 0.0f;

    void Update()
    {
        if (waypoints.Length < 2) return;

        Transform currentWaypoint = waypoints[currentWaypointIndex];
        Transform nextWaypoint = waypoints[(currentWaypointIndex + direction + waypoints.Length) % waypoints.Length];

        elapsedTime += Time.deltaTime;
        float fractionOfJourney = elapsedTime / timeToNextPoint;

        transform.position = Vector2.Lerp(currentWaypoint.position, nextWaypoint.position, fractionOfJourney);

        if (fractionOfJourney >= 1.0f)
        {
            currentWaypointIndex = (currentWaypointIndex + direction + waypoints.Length) % waypoints.Length;
            elapsedTime = 0.0f;

            if (currentWaypointIndex == 0 || currentWaypointIndex == waypoints.Length - 1)
            {
                direction = -direction;
            }
        }
    }
}
