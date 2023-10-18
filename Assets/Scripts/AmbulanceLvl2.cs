using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbulanceLvl2 : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed;
    private int currentWaypointIndex = 0;
    public float delay; // Delay in seconds
    private float timer = 0f;
    private bool hasStartedMoving = false;

    public GameObject human1;
    public GameObject human4;
    private bool human1Infected;
    private bool human4Infected;
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
                MoveInfectedHumansIntoAmbulance(() =>
                {
                    displayResult();
                });
            }
        }

        // check constantly if all the humans are infected; if so, stop the game immediately
        if (HasChildren(human1.transform) && HasChildren(human4.transform))
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
        if (HasChildren(human4.transform))
        {
            while (Vector3.Distance(human4.transform.position, ambulance.transform.position) > 0.001f)
            {
                human4.transform.position = Vector3.MoveTowards(human4.transform.position, ambulance.transform.position, step);
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

    void displayResult()
    {
        if (human1Infected && human4Infected)
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
}
