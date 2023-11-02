using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectDurationAVG : MonoBehaviour
{
    public static List<float> infectionDurations = new List<float>();

    private float timeOfInfection;

    void Start()
    {
        StartInfection();
    }

    void OnDestroy()
    {
        EndInfection();
    }

    public void StartInfection()
    {
        timeOfInfection = Time.time;
    }

    public void EndInfection()
    {
        float duration = Time.time - timeOfInfection;
        infectionDurations.Add(duration);
        Debug.Log("LIST: " + infectionDurations);
    }

    public float CalculateAverageDuration()
    {
        float sum = 0;
        foreach (float duration in infectionDurations)
        {
            sum += duration;
        }
        float res = sum / infectionDurations.Count;
        Debug.Log("AVG: " + res);
        return res;
    }
}

