using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentLifetime : MonoBehaviour
{
    private List<float> childDurations = new List<float>();

    void Update()
    {

        foreach (Transform child in transform)
        {
            Debug.Log("Child tag: " + child.gameObject.tag);
            if (child.CompareTag("InitialVirus"))
            {
                Debug.Log("New initialVirus child detected: " + child.gameObject.name);
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
        Debug.Log("Average child duration: " + avg);
    }
}
