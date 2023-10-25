using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class rangepopup : MonoBehaviour
{
    public TextMeshProUGUI popupText;
    public float displayTime = 2f;
    private Coroutine popupCoroutine;

    private void Awake()
    {
        if (popupText != null)
        {
            popupText.gameObject.SetActive(false);
        }
    }

    public void ShowPopup(string message)
    {
        if (popupCoroutine != null)
        {
            StopCoroutine(popupCoroutine);
        }
        popupCoroutine = StartCoroutine(ShowPopupCoroutine(message));
    }

    private IEnumerator ShowPopupCoroutine(string message)
    {
        if (popupText != null)
        {
            popupText.text = message;
            popupText.gameObject.SetActive(true);
            yield return new WaitForSeconds(displayTime);
            popupText.gameObject.SetActive(false);
        }
    }
}
