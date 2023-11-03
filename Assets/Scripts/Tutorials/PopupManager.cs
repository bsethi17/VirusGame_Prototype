using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public List<GameObject> popups;
    public int currentPopupIndex;
    public GameObject instructionsPanel;

    private void Awake()  // Using Awake instead of Start
    {
        Time.timeScale = 0;
    }

    public void CloseInstructions()
    {
        instructionsPanel.SetActive(false);
        // Resume the game
        Time.timeScale = 1;
    }

    void Start()
    {
        currentPopupIndex = 0;
        ShowPopup(currentPopupIndex);
    }

    void ShowPopup(int index)
    {
        if (index < popups.Count)
        {
            for (int i = 0; i < popups.Count; i++)
            {
                popups[i].SetActive(i == index);
                Debug.Log("set active:" + index);
            }
        }
        else
        {
            // All popups are shown, start the game or do other stuff
            Debug.Log("All popups are shown, start the game!");
        }
    }

    public void NextPopup()
    {
        Debug.Log("NEXTPOPUP");
        currentPopupIndex++;
        Debug.Log("NEXT: " + currentPopupIndex);
        ShowPopup(currentPopupIndex);
    }
}
