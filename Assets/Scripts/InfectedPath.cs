using UnityEngine;

public class InfectedPath : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 5.0f;
    private int currentWaypointIndex = 0;
    private int direction = 1;  // 1 for forward, -1 for backward

    void Update()
    {
        if (waypoints.Length < 2) return;  // Need at least 2 waypoints to move

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.001f)
        {
            currentWaypointIndex += direction;

            if (currentWaypointIndex >= waypoints.Length - 1 || currentWaypointIndex <= 0)
            {
                direction = -direction;  // Change direction
            }
        }
    }
}
