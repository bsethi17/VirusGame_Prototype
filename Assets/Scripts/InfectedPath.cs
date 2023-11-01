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


 private bool hasVisitedFirstWaypoint = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hii triggered");
        if (collision.CompareTag("FirstWaypoint")) // Change "FirstWaypoint" to the tag of your first waypoint
        {
            if (!hasVisitedFirstWaypoint)
            {
                // Mark the first waypoint as visited
                hasVisitedFirstWaypoint = true;
            }
            else
            {
                // Find and disable the InfectedPath script
                InfectedPath infectedPathScript = GetComponent<InfectedPath>();
                if (infectedPathScript != null)
                {
                    infectedPathScript.enabled = false;
                }

                // Find and enable the CirclePathMovementNV1 script
                MonoBehaviour circlePathScript = (MonoBehaviour)GetComponent(typeof(MonoBehaviour)); // Replace MonoBehaviour with the actual type of your circle path script
                if (circlePathScript != null)
                {
                    circlePathScript.enabled = true;
                }
            }
        }
    }
}
