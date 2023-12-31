using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbulanceTutorial : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed;
    private int currentWaypointIndex = 0;
    public float delay; // Delay in seconds
    private float timer = 0f;
    private bool hasStartedMoving = false;

    public GameObject human1;
    //public GameObject human2;
    //public GameObject human3;
    //public GameObject human4;
    private GameObject ambulance;
    public float slideSpeed;

    public PopUpCanvas popUpCanvas;

    public PopUpCanvas popUpCanvasLost;
    // Analytics
    public SuccessRateRequestL6 successRateRequest;
    private bool requestSent;

    private void Awake()
    {
        requestSent = false;
    }

    async void Start()
    {
        popUpCanvas.HidePopUp();
        ambulance = this.gameObject;
    }

    void Update()
    {
        // check constantly if all the humans are infected; if so, stop the game immediately
        // if (IsInfected(human1.transform))
        // {
        //     if (popUpCanvas != null)
        //     {
        //         popUpCanvas.ShowPopUp("Virus wins!");
        //     }
        //     else
        //     {
        //         Debug.LogWarning("PopUpCanvas reference is not assigned!");
        //     }
        // }

        // Add some delay before it starts to move
        if (!hasStartedMoving)
        {
            timer += Time.deltaTime;
            if (timer >= delay)
            {
                displayResult2();
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
        if (IsInfected(human1.transform))
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
        if (IsInfected(human1.transform))
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

            if (!requestSent)
            {
                int numberOfInfectedHumans = getNumOfInfectedHumans();
                successRateRequest.Send(numberOfInfectedHumans);

                requestSent = true;
                return;
            }
            else
            {
                Debug.LogWarning("PopUpCanvas reference is not assigned!");
            }
        }
    }

    void displayResult2()
    {
        if (popUpCanvasLost != null)
        {
            popUpCanvasLost.ShowPopUp("Virus Lost!");
        }
    }
}
