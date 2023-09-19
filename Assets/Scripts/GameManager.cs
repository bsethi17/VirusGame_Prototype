using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void RestartGame()
    {
        // Add any additional restart logic here.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StopGame()
    {
        Time.timeScale = 0f; // Stop the game
    }
}
