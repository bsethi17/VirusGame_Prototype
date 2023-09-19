using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpCanvas : MonoBehaviour
{
    public GameObject canvas;
    public TMP_Text popUpText;

    void Start()
    {
        HidePopUp();
    }

    void Update()
    {

    }

    public void ShowPopUp(string message)
    {
        popUpText.text = message;
        canvas.SetActive(true);
    }

    public void HidePopUp()
    {
        canvas.SetActive(false);
    }
}
