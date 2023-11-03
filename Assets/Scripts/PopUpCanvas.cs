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

    // Analytic1
    public SuccessRateRequestL1 successRateRequestL1;
    public SuccessRateRequestL2 successRateRequestL2;
    public SuccessRateRequestL3 successRateRequestL3;
    public SuccessRateRequestL4 successRateRequestL4;
    public SuccessRateRequestL5 successRateRequestL5;
    public SuccessRateRequestL6 successRateRequestL6;
    private bool requestSent1;

    void Awake()
    {
        requestSent1 = false;
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
        currentlevel = SceneManager.GetActiveScene().name;
        if (message == "Virus wins!")
        {
            EndLife();

            StartCoroutine(gameManager.DelayedStopGame());
            SendAnalytics1(currentlevel);
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

    private void SendAnalytics1(string currentlevel)
    {
        //Analytics
        if (!requestSent1)
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
            }
            else if (currentlevel == "Level3")
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
            else if (currentlevel == "Level4")
            {
                if (successRateRequestL4 == null)
                {
                    Debug.LogError("successRateRequest is null");
                }
                else
                {
                    successRateRequestL4.Send(4);
                }
            }
            else if (currentlevel == "Level5")
            {
                if (successRateRequestL5 == null)
                {
                    Debug.LogError("successRateRequest is null");
                }
                else
                {
                    successRateRequestL5.Send(4);
                }
            }
            else if (currentlevel == "Level6")
            {
                if (successRateRequestL6 == null)
                {
                    Debug.LogError("successRateRequest is null");
                }
                else
                {
                    successRateRequestL6.Send(4);
                }
            }
            requestSent1 = true;
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
