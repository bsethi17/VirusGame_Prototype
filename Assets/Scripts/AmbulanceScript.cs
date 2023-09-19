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
    private bool human1Infected;
    private bool human2Infected;
    private bool human3Infected;
    private GameObject ambulance;
    public float slideSpeed;

    public PopUpCanvas popUpCanvas;

    void Start()
    {
        popUpCanvas.HidePopUp();
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

    bool isInfected(Transform transform)
    {
        float step = slideSpeed * Time.deltaTime;
        if (HasChildren(transform))
        {
            // make the human get into ambulance
            transform.position = Vector3.MoveTowards(transform.position, ambulance.transform.position, step);
            return true;
        }
        else
        {
            return false;
        }
    }

    void displayResult()
    {
        if (human1Infected && human2Infected && human3Infected)
        {
            if (popUpCanvas != null)
            {
                popUpCanvas.ShowPopUp("Virus wins!");
            }
            else
            {
                Debug.LogWarning("PopUpCanvas reference is not assigned!");
            }
        }
        else
        {
            if (popUpCanvas != null)
            {
                popUpCanvas.ShowPopUp("Human wins!");
            }
            else
            {
                Debug.LogWarning("PopUpCanvas reference is not assigned!");
            }
        }
    }

    void HandleLastWaypointReached()
    {
        // This function will be called when the object stops at the last waypoint.
        human2Infected = isInfected(human2.transform);
        human1Infected = isInfected(human1.transform);
        human3Infected = isInfected(human3.transform);

        // give the game results: if there's still non-vaccinated human who is not infected, then the humans win; otherwise us(virus) win
        Invoke("displayResult", 3f);
    }
}
