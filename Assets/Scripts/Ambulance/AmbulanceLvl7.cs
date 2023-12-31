using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbulanceLvl7 : MonoBehaviour
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
    public GameObject human4;
    private GameObject ambulance;

    public PopUpCanvas popUpCanvasLost;
    public float slideSpeed;

    public PopUpCanvas popUpCanvas;

    // Analytic1
    public SuccessRateRequestL7 successRateRequest;
    private bool requestSent1;

    // Analytic3
    public TerminationL7 terminationL7;
    private bool requestSent3;

    // Analytic 4
    public healingHouse hh;
    public HealedNumberLvl7 healedNumberLvl7;
    private bool requestSent4;

    private int levelNumber = 6;

    private void Awake()
    {
        requestSent1 = false;
        requestSent3 = false;
        requestSent4 = false;
    }

    void Start()
    {
        popUpCanvas.HidePopUp();
        ambulance = this.gameObject;
    }

    void Update()
    {
        // check constantly if all the humans are infected; if so, stop the game immediately
        if (IsInfected(human1.transform) && IsInfected(human2.transform) && IsInfected(human3.transform) && IsInfected(human4.transform))
        {
            if (popUpCanvas != null)
            {
                // // change the level scene button status
                // LevelManager levelManager = FindObjectOfType<LevelManager>();
                // if (levelManager != null)
                // {
                //     levelManager.CompleteLevel(levelNumber + 1);
                // }
                popUpCanvas.ShowPopUp("Virus wins!");
            }
            else
            {
                Debug.LogWarning("PopUpCanvas reference is not assigned!");
            }
        }

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
                MoveInfectedHumansIntoAmbulance(() =>
                {
                    displayResult();
                });
            }
        }
    }

    // check if this human is infected
    bool IsInfected(Transform obj)
    {
        foreach (Transform child in obj)
        {
            if (child.name.StartsWith("InitialVirus"))
            {
                return true;
            }
        }
        return false;
    }

    void MoveInfectedHumansIntoAmbulance(Action callback)
    {
        float step = slideSpeed * Time.deltaTime;
        StartCoroutine(MoveToAmbulance(step, callback));
    }

    // make the human get into the ambulance
    IEnumerator MoveToAmbulance(float step, Action callback)
    {
        if (IsInfected(human2.transform))
        {
            while (Vector3.Distance(human2.transform.position, ambulance.transform.position) > 0.001f)
            {
                human2.transform.position = Vector3.MoveTowards(human2.transform.position, ambulance.transform.position, step);
                yield return null;
            }
        }
        if (IsInfected(human1.transform))
        {
            while (Vector3.Distance(human1.transform.position, ambulance.transform.position) > 0.01f)
            {
                human1.transform.position = Vector3.MoveTowards(human1.transform.position, ambulance.transform.position, step);
                yield return null;
            }
        }
        if (IsInfected(human3.transform))
        {
            while (Vector3.Distance(human3.transform.position, ambulance.transform.position) > 0.001f)
            {
                human3.transform.position = Vector3.MoveTowards(human3.transform.position, ambulance.transform.position, step);
                yield return null;
            }
        }
        if (IsInfected(human4.transform))
        {
            while (Vector3.Distance(human4.transform.position, ambulance.transform.position) > 0.001f)
            {
                human4.transform.position = Vector3.MoveTowards(human4.transform.position, ambulance.transform.position, step);
                yield return null;
            }
        }

        // Movement is finished, invoke the callback.
        if (callback != null)
        {
            callback();
        }
    }

    private int getNumOfInfectedHumans()
    {
        int count = 0;
        if (IsInfected(human1.transform))
        {
            count += 1;
        }
        if (IsInfected(human2.transform))
        {
            count += 1;
        }
        if (IsInfected(human3.transform))
        {
            count += 1;
        }
        if (IsInfected(human4.transform))
        {
            count += 1;
        }
        return count;
    }

    // display result when time's up
    void displayResult()
    {
        if (popUpCanvasLost != null)
        {
            popUpCanvasLost.ShowPopUp("Virus Lost!");

            if (!requestSent1)
            {
                int numberOfInfectedHumans = getNumOfInfectedHumans();
                successRateRequest.Send(numberOfInfectedHumans);

                requestSent1 = true;
                return;
            }

            // Send analytic 3
            if (!requestSent3)
            {
                if (terminationL7)
                {
                    terminationL7.Send(0);
                    requestSent3 = true;
                }
                else
                {
                    Debug.LogError("terminationL7 is null");
                }
            }

            // Send analytic 4
            if (!requestSent4)
            {
                if (healedNumberLvl7)
                {
                    healedNumberLvl7.Send(hh.counter);
                    requestSent4 = true;
                }
                else
                {
                    Debug.LogError("healedNumberLvl7 is null!");
                }
            }
        }
        else
        {
            Debug.LogWarning("PopUpCanvas reference is not assigned!");
        }
    }
}
