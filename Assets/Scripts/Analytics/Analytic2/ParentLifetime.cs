using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ParentLifetime : MonoBehaviour
{
    public static ParentLifetime Instance { get; private set; }
    private List<float> childDurations = new List<float>();

    public AvgLifeTimeL1 avgLifeTimeL1;
    public AvgLifeTimeL2 avgLifeTimeL2;
    public AvgLifeTimeL3 avgLifeTimeL3;
    public AvgLifeTimeL4 avgLifeTimeL4;
    public AvgLifeTimeL5 avgLifeTimeL5;
    public AvgLifeTimeL6 avgLifeTimeL6;

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
        // Debug.Log("HandleChildDestroy");
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
    }

    private void SendAnalytics(string currentlevel, float avg)
    {
        //Analytics
        if (!requestSent)
        {
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
            else if (currentlevel == "Level2")
            {
                if (avgLifeTimeL2 == null)
                {
                    Debug.LogError("avgLifeTimeL2 is null");
                }
                else
                {
                    avgLifeTimeL2.Send(gameObject.tag, avg);
                }
            }
            else if (currentlevel == "Level3")
            {
                if (avgLifeTimeL3 == null)
                {
                    Debug.LogError("avgLifeTimeL3 is null");
                }
                else
                {
                    avgLifeTimeL3.Send(gameObject.tag, avg);
                }
            }
            else if (currentlevel == "Level4")
            {
                if (avgLifeTimeL4 == null)
                {
                    Debug.LogError("avgLifeTimeL4 is null");
                }
                else
                {
                    avgLifeTimeL4.Send(gameObject.tag, avg);
                }
            }
            else if (currentlevel == "Level5")
            {
                if (avgLifeTimeL5 == null)
                {
                    Debug.LogError("avgLifeTimeL5 is null");
                }
                else
                {
                    avgLifeTimeL5.Send(gameObject.tag, avg);
                }
            }
            else if (currentlevel == "Level6")
            {
                if (avgLifeTimeL6 == null)
                {
                    Debug.LogError("avgLifeTimeL6 is null");
                }
                else
                {
                    avgLifeTimeL6.Send(gameObject.tag, avg);
                }
            }
            requestSent = true;
        }
    }
}
