using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Button[] levelButtons;

    private static LevelManager instance;

    public static int maxFinishedLevel = 1;

    void Awake()
    {

    }

    void Start()
    {
        // // Disable all level buttons initially
        // DisableAllButtons();

        // // Enable the first level button
        // EnableButtonFromLevel(maxFinishedLevel);
    }

    public void CompleteLevel(int level)
    {
        Debug.Log("try to enable from level" + level);
        DisableAllButtons();

        // Enable the next level button
        EnableButtonFromLevel(level + 1);

        // Change color of completed level button (optional)
        ChangeButtonColor(level, Color.green);

        maxFinishedLevel = level;
    }

    private void EnableButton(int level)
    {
        if (level <= levelButtons.Length)
        {
            levelButtons[level - 1].interactable = true;
        }
    }

    private void EnableButtonFromLevel(int level)
    {
        for (int i = 1; i <= level; i++)
        {
            EnableButton(i);
        }
    }

    private void DisableAllButtons()
    {
        foreach (Button button in levelButtons)
        {
            button.interactable = false;
        }
    }

    private void ChangeButtonColor(int level, Color color)
    {
        if (level <= levelButtons.Length)
        {
            ColorBlock colors = levelButtons[level - 1].colors;
            colors.normalColor = color;
            levelButtons[level - 1].colors = colors;
        }
    }
}
