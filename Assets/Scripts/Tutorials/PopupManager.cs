using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public List<GameObject> popups;
    public int currentPopupIndex;
    public GameObject instructionsPanel;

    public static bool PopupsShownKey { get; private set; } // popup show flag

    private void Awake()  // Using Awake instead of Start
    {
        if (PopupsShownKey)
        {
            Debug.Log("Popups have already been shown in a previous session.");
            CloseInstructions();
            return;
        }
        Time.timeScale = 0;
        
    }

    public void CloseInstructions()
    {
        Debug.Log("CloseInstructions:");
        instructionsPanel.SetActive(false);
        PopupsShownKey = true;
        Debug.Log("PopupsShownKey:" + PopupsShownKey);
        // Resume the game
        Time.timeScale = 1;
    }

    void Start()
    {
        if (PopupsShownKey)
        {
            Debug.Log("Popups have already been shown in a previous session.");
            for (int i = 0; i < popups.Count; i++)
            {
                popups[i].SetActive(false);
            }
            Time.timeScale = 1;
            return;
        }
        else
        {
            Debug.Log("Showing popups for the first time.");
        }
        currentPopupIndex = 0;
        ShowPopup(currentPopupIndex);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

    void ShowPopup(int index)
    {
        if (index < popups.Count)
        {
            for (int i = 0; i < popups.Count; i++)
            {
                popups[i].SetActive(i == index);
            }
        }
    }

    public void NextPopup()
    {
        currentPopupIndex++;
        ShowPopup(currentPopupIndex);
    }

    public static void SetPopupsShown(bool shown)
    {
        PopupsShownKey = shown;
    }
}
