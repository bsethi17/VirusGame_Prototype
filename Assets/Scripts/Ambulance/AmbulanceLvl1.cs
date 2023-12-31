using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbulanceLvl1 : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed;
    private int currentWaypointIndex = 0;
    // Delay in seconds, time when the ambulance starts to move
    public float delay;
    private float timer = 0f;
    private bool hasStartedMoving = false;

    public GameObject human1;
    public GameObject human2;
    private bool human1Infected;
    private bool human2Infected;
    private GameObject ambulance;
    public float slideSpeed;
    public PopUpCanvas popUpCanvasLost;
    public PopUpCanvas popUpCanvas;

    // Analytic1
    public SuccessRateRequestL1 successRateRequestL1;
    private bool requestSent1;

    // Analytic3
    public TerminationL1 terminationL1;
    private bool requestSent3;

    private int levelNumber = 1;

    private void Awake()
    {
        requestSent1 = false;
        requestSent3 = false;
    }

    void Start()
    {
        popUpCanvas.HidePopUp();
        ambulance = this.gameObject;
    }

    void Update()
    {
        // check constantly if all the humans are infected; if so, stop the game immediately
        if (HasChildren(human1.transform) && HasChildren(human2.transform))
        {
            // virus wins
            if (popUpCanvas != null)
            {
                // // change the level scene button status
                // LevelManager levelManager = FindObjectOfType<LevelManager>();
                // if (levelManager != null)
                // {
                //     levelManager.CompleteLevel(levelNumber + 1);
                // }
                popUpCanvas.ShowPopUp("Virus wins!");
                return;
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
    bool HasChildren(Transform obj)
    {
        return obj.childCount > 0;
    }

    void MoveInfectedHumansIntoAmbulance(Action callback)
    {
        float step = slideSpeed * Time.deltaTime;
        StartCoroutine(MoveToAmbulance(step, callback));
    }

    // make the human get into the ambulance
    IEnumerator MoveToAmbulance(float step, Action callback)
    {
        if (HasChildren(human2.transform))
        {
            while (Vector3.Distance(human2.transform.position, ambulance.transform.position) > 0.001f)
            {
                human2.transform.position = Vector3.MoveTowards(human2.transform.position, ambulance.transform.position, step);
                yield return null;
            }
        }
        if (HasChildren(human1.transform))
        {
            while (Vector3.Distance(human1.transform.position, ambulance.transform.position) > 0.01f)
            {
                human1.transform.position = Vector3.MoveTowards(human1.transform.position, ambulance.transform.position, step);
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
        if (HasChildren(human1.transform))
        {
            count += 1;
        }
        if (HasChildren(human2.transform))
        {
            count += 1;
        }
        return count;
    }

    void displayResult()
    {
        if (human1Infected && human2Infected)
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
        // player loses because of timer's up
        else
        {
            if (popUpCanvasLost != null)
            {
                popUpCanvasLost.ShowPopUp("Virus Lost!");
                // Send analytic 1
                if (!requestSent1)
                {
                    int numberOfInfectedHumans = getNumOfInfectedHumans();
                    successRateRequestL1.Send(numberOfInfectedHumans);
                    // Debug.Log("SENT:" + numberOfInfectedHumans);
                    requestSent1 = true;
                }

                // Send analytic 3
                if (!requestSent3)
                {
                    if (terminationL1)
                    {
                        terminationL1.Send(0);
                        requestSent3 = true;
                    }
                    else
                    {
                        Debug.LogError("terminationL1 is null");
                    }
                }
            }
            else
            {
                Debug.LogWarning("PopUpCanvas reference is not assigned!");
            }
        }
    }
}
