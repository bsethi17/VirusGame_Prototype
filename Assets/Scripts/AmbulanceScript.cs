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
    public GameObject human3;
    private GameObject ambulance;
    public float slideSpeed;

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

    void TakeInfectedPersonAway(Transform transform)
    {
        float step = slideSpeed * Time.deltaTime;
        if (HasChildren(transform))
        {
            // make the human get into ambulance
            transform.position = Vector3.MoveTowards(transform.position, ambulance.transform.position, step);

        }

    }

    void HandleLastWaypointReached()
    {
        // This function will be called when the object stops at the last waypoint.
        TakeInfectedPersonAway(human2.transform);
        TakeInfectedPersonAway(human1.transform);
        TakeInfectedPersonAway(human3.transform);
    }
}
