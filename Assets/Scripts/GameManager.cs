using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private string originalSceneName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        originalSceneName = SceneManager.GetActiveScene().name;
    }


    public void RestartGame()
    {
        // Add any additional restart logic here.
        ResetBulletCount();
        SceneManager.LoadScene(originalSceneName);
        ResumeGame();
    }

    public void NextLevel() {
        ResetBulletCount();
        SceneManager.LoadScene("LevelScene");
        ResumeGame();
    }

    public void StopGame()
    {
        Time.timeScale = 0f; // Stop the game
    }

    // deley the stop operation to make sure the pop up shows up correctly
    public IEnumerator DelayedStopGame()
    {
        yield return new WaitForSeconds(0.05f);
        StopGame();
    }

    void ResumeGame()
    {
        Time.timeScale = 1f; // Unpause the game

        // reset the virus to the initial virus 
        BulletScript.isInitialVirus = false;
        BulletScript.maxRange = 2;
    }

    private void ResetBulletCount()
    {
        UIManager.Instance.ResetBulletsToInitialCount(); // Calling the reset method from UIManager.
    }
}

