using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpCanvas : MonoBehaviour
{
    public static PopUpCanvas Instance { get; private set; }
    public TMP_Text popUpText;
    private GameManager gameManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        HidePopUp();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {

    }

    public void ShowPopUp(string message)
    {
         this.gameObject.SetActive(true);
        popUpText.text = message;   
        gameManager.StopGame();       
    }

    public void HidePopUp()
    {
        this.gameObject.SetActive(false);
    }
}
