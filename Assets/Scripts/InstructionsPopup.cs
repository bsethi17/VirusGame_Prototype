using UnityEngine;

public class InstructionsPopup : MonoBehaviour
{
    public GameObject instructionsPanel;

    private void Awake()  // Using Awake instead of Start
    {
        // Automatically show the popup when the scene initializes
        ShowInstructions();
    }

    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true);
        // Pause the game
        Time.timeScale = 0;
    }

    public void CloseInstructions()
    {
        // Resume the game
        Time.timeScale = 1;
        instructionsPanel.SetActive(false);
    }
}
