using UnityEngine;

public class AmbulanceArrivePathMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed;
    private int currentWaypointIndex = 0;
    private float delay = 5f; // Delay in seconds
    private float timer = 0f;
    private bool hasStartedMoving = false;

    void Update()
    {
        if (!hasStartedMoving)
        {
            timer += Time.deltaTime;
            if (timer >= delay)
            {
                hasStartedMoving = true;
            }
            return;
        }

        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.001f)
        {
            if (currentWaypointIndex < waypoints.Length - 1)
            {
                currentWaypointIndex++;
            }
        }
    }
}
