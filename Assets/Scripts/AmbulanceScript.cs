using UnityEngine;

public class AmbulanceScript : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed;
    private int currentWaypointIndex = 0;
    public float delay; // Delay in seconds
    private float timer = 0f;
    private bool hasStartedMoving = false;

    public GameObject human1;
    public GameObject human2;
    private GameObject ambulance;
    public float slideSpeed = 1.0f;

    void Start()
    {
        ambulance = this.gameObject;
    }

    void Update()
    {
        // Add some delay before it starts to move
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
            // make the ambulance stop at the last waypoint
            if (currentWaypointIndex < waypoints.Length - 1)
            {
                currentWaypointIndex++;
            }
            else
            {
                // The object has reached the last waypoint.
                HandleLastWaypointReached();
            }

        }
    }

    // check if this human is infected
    bool HasChildren(Transform obj)
    {
        return obj.childCount > 0;
    }

    void HandleLastWaypointReached()
    {
        // This function will be called when the object stops at the last waypoint.
        if (HasChildren(human2.transform))
        {
            // make the human slide into ambulance
            float step = slideSpeed * Time.deltaTime;

            human2.transform.position = Vector3.MoveTowards(human2.transform.position, ambulance.transform.position, step);
        }
    }
}
