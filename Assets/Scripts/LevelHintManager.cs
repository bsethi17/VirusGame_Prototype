using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelHintManager : MonoBehaviour
{
    public TMP_Text tooltipText;

    public void ShowTooltip(string message)
    {
        tooltipText.text = message;
        tooltipText.gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltipText.text = "";
        tooltipText.gameObject.SetActive(false);
    }
}
