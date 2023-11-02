using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PopUpCanvas : MonoBehaviour
{
    public static PopUpCanvas Instance { get; private set; }
    public TMP_Text popUpText;
    private GameManager gameManager;
    string currentlevel;

    //Analytics
    public SuccessRateRequestL1 successRateRequestL1;
    public SuccessRateRequestL2 successRateRequestL2;
    public SuccessRateRequestL3 successRateRequestL3;
    private bool requestSent;

    void Awake()
    {
        requestSent = false;
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
        if (message == "Virus wins!")
        {
            EndLife();

            StartCoroutine(gameManager.DelayedStopGame());
            currentlevel = SceneManager.GetActiveScene().name;
            SendAnalytics(currentlevel);
        }
        else
        {
            StartCoroutine(gameManager.DelayedStopGame());
        }

    }

    public void HidePopUp()
    {
        this.gameObject.SetActive(false);
    }

    private void SendAnalytics(string currentlevel)
    {
        //Analytics
        if (!requestSent)
        {
            Debug.Log("SENDING: ");
            if (currentlevel == "Level1")
            {
                if (successRateRequestL1 == null)
                {
                    Debug.LogError("successRateRequest is null");
                }
                else
                {
                    successRateRequestL1.Send(2);
                }
            }
            else if (currentlevel == "Level2")
            {
                if (successRateRequestL2 == null)
                {
                    Debug.LogError("successRateRequest is null");
                }
                else
                {
                    successRateRequestL2.Send(2);
                }
            } else if (currentlevel == "Level3")
            {
                if (successRateRequestL3 == null)
                {
                    Debug.LogError("successRateRequest is null");
                }
                else
                {
                    successRateRequestL3.Send(4);
                }
            }

            requestSent = true;
        }
    }

    public void EndLife()
    {
        ObjectLifetime[] objects = FindObjectsOfType<ObjectLifetime>();
        foreach (ObjectLifetime obj in objects)
        {
            obj.EndGame();
        }
    }
}
