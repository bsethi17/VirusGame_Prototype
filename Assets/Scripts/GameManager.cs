using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private string originalSceneName;

    public bool isInitialVirus;
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
        Debug.Log("RELOAD");
        // Add any additional restart logic here.
        ResetBulletCount();
        SceneManager.LoadScene(originalSceneName);
        ResumeGame();
    }

    public void Menu()
    {
        ResetBulletCount();
        PopupManager.SetPopupsShown(false);
        SceneManager.LoadScene("LevelScene");
        ResumeGame();
    }

    public void GoToLevel2()
    {
        ResetBulletCount();
        PopupManager.SetPopupsShown(false);
        SceneManager.LoadScene("Level2");
    }

    public void GoToLevel3()
    {
        ResetBulletCount();
        PopupManager.SetPopupsShown(false);
        SceneManager.LoadScene("Level3");
    }

    public void GoToLevel4()
    {
        ResetBulletCount();
        PopupManager.SetPopupsShown(false);
        SceneManager.LoadScene("Level4");
    }

    public void GoToLevel5()
    {
        ResetBulletCount();
        PopupManager.SetPopupsShown(false);
        SceneManager.LoadScene("Level5");
    }

    public void GoToLevel6()
    {
        ResetBulletCount();
        PopupManager.SetPopupsShown(false);
        SceneManager.LoadScene("Level6");
    }

    public void GoToLevel7()
    {
        ResetBulletCount();
        PopupManager.SetPopupsShown(false);
        SceneManager.LoadScene("Level7");
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isInitialVirus = false;
    }
}

