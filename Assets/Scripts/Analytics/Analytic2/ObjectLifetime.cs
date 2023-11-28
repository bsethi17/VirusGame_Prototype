using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLifetime : MonoBehaviour
{
    public delegate void OnDestroyHandler(float duration);
    public event OnDestroyHandler OnDestroyEvent;

    // Define a delegate and event for the EndGame handler
    public delegate void OnEndGameHandler(float duration);
    public event OnEndGameHandler OnEndGameEvent;

    private float startTime;
    private bool gameEnded = false;

    void Start()
    {
        startTime = Time.time;
    }

    void OnDestroy()
    {
        if (!gameEnded)
        {
            CalculateDurationOnDestroy();
        }
    }

    public void EndGame()
    {
        gameEnded = true;
        CalculateDurationOnEndGame();
    }

    private void CalculateDurationOnDestroy()
    {
        float duration = Time.time - startTime;
        // Debug.Log(gameObject.name + " duration on destroy: " + duration);
        OnDestroyEvent?.Invoke(duration);
    }

    private void CalculateDurationOnEndGame()
    {
        float duration = Time.time - startTime;
        // Debug.Log(gameObject.name + " duration on end game: " + duration);
        OnEndGameEvent?.Invoke(duration);
    }
}
