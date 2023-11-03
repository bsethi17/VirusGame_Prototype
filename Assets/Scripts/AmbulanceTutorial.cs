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
    private GameObject ambulance;
    public float slideSpeed;

    public PopUpCanvas popUpCanvas;

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
        // Check if human1 is infected; if so, stop the game immediately
        if (IsInfected(human1.transform))
        {
            if (popUpCanvas != null)
            {
                popUpCanvas.ShowPopUp("Virus wins!");
            }
            else
            {
                Debug.LogWarning("PopUpCanvas reference is not assigned!");
            }
            return; // Stop further execution if human1 is infected
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
        if (IsInfected(human1.transform))
        {
            float step = slideSpeed * Time.deltaTime;
            StartCoroutine(MoveToAmbulance(human1, step, callback));
        }
        else
        {
            callback?.Invoke();
        }
    }

    IEnumerator MoveToAmbulance(GameObject human, float step, Action callback)
    {
        while (Vector3.Distance(human.transform.position, ambulance.transform.position) > 0.001f)
        {
            human.transform.position = Vector3.MoveTowards(human.transform.position, ambulance.transform.position, step);
            yield return null;
        }

        callback?.Invoke();
    }

    private int getNumOfInfectedHumans()
    {
        return IsInfected(human1.transform) ? 1 : 0;
    }

    void displayResult()
    {
        if (popUpCanvas != null)
        {
            popUpCanvas.ShowPopUp("Virus Lost!");

            if (!requestSent)
            {
                int numberOfInfectedHumans = getNumOfInfectedHumans();
                successRateRequest.Send(numberOfInfectedHumans);

                requestSent = true;
            }
        }
        else
        {
            Debug.LogWarning("PopUpCanvas reference is not assigned!");
        }
    }
}
