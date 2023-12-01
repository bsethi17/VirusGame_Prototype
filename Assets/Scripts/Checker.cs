using UnityEngine;
using System.Collections;
public class Checker : MonoBehaviour
{
    public PopUpCanvas popUpCanvas;
    private int levelNumber = 6;

    // Update is called once per frame
    async void Start()
    {
        popUpCanvas.HidePopUp();
    }

    void Update()
    {
        if (IsHealingHousePresent())
        {
            Debug.Log("HealingHouse exists.");
            // You can perform additional actions here if needed.
        }
        else
        {
            // Debug.Log("HealingHouse does not exist.");
            // LevelManager levelManager = FindObjectOfType<LevelManager>();
            // if (levelManager != null)
            // {
            //     levelManager.CompleteLevel(levelNumber + 1);
            // }
            StartCoroutine(ShowDelayedPopup("You Won!", 1.0f)); // Delay for 2 seconds and then show the popup.
            // Actions to take if the HealingHouse does not exist.
        }

        if (UIManager.Instance != null && UIManager.Instance.GlobalBulletCount == 0)
        {
            if (popUpCanvas != null)
            {
                popUpCanvas.ShowPopUp("Virus Lost!");
            }
        }


    }

    bool IsHealingHousePresent()
    {
        GameObject healingHouse = GameObject.Find("healingHouse");
        return healingHouse != null; // Returns true if the HealingHouse is found.
    }

    IEnumerator ShowDelayedPopup(string message, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (popUpCanvas != null)
        {
            popUpCanvas.ShowPopUp(message);
        }
    }
}
