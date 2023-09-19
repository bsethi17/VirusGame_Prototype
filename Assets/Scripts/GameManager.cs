using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void StopGame()
    {
        Time.timeScale = 0f; // Stop the game
    }
}
