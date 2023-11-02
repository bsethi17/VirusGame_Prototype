using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ParentLifetime : MonoBehaviour
{
     public static ParentLifetime Instance { get; private set; }
    private List<float> childDurations = new List<float>();

    public AvgLifeTimeL1 avgLifeTimeL1;

    private bool requestSent;


    void Awake()
    {
        requestSent = false;
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Update()
    {

        foreach (Transform child in transform)
        {
            if (child.CompareTag("InitialVirus"))
            {
                ObjectLifetime objectLifetime = child.GetComponent<ObjectLifetime>();
                if (objectLifetime == null)
                {
                    objectLifetime = child.gameObject.AddComponent<ObjectLifetime>();
                }
                objectLifetime.OnDestroyEvent += HandleChildDestroy;
                objectLifetime.OnEndGameEvent += HandleChildEndGame;

            }
        }

    }

    private void HandleChildDestroy(float duration)
    {
        // Add the child's duration to the list
        childDurations.Add(duration);

        // Print the average duration of all child objects
        PrintAverageChildDuration();
    }
    private void HandleChildEndGame(float duration)
    {
        Debug.Log("HandleChildEndGame");
        // Add the child's duration to the list
        childDurations.Add(duration);

        // Print the average duration of all child objects
        PrintAverageChildDuration();
    }

    private void PrintAverageChildDuration()
    {
        float sum = 0f;
        foreach (float duration in childDurations)
        {
            sum += duration;
        }
        float avg = childDurations.Count > 0 ? sum / childDurations.Count : 0f;
        // Debug.Log("Average child duration: " + avg);
        string currentlevel = SceneManager.GetActiveScene().name;
        SendAnalytics(currentlevel, avg);
        Debug.Log("Current object tag: " + gameObject.tag);
    }

    private void SendAnalytics(string currentlevel, float avg)
    {
        //Analytics
        if (!requestSent)
        {
            Debug.Log("SENDING ANL2: ");
            Debug.Log("Current object tag: " + gameObject.tag);
            Debug.Log("requestSent: " + requestSent);
            if (currentlevel == "Level1")
            {
                if (avgLifeTimeL1 == null)
                {
                    Debug.LogError("avgLifeTimeL1 is null");
                }
                else
                {
                    avgLifeTimeL1.Send(gameObject.tag, avg);
                }
            }
            

            requestSent = true;
        }
    }
}