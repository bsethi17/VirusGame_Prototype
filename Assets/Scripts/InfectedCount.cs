using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InfectedCount : MonoBehaviour
{
    public TextMeshProUGUI infectedText;
    public PopUpCanvas popUpCanvas;
    //Analytics
    public SuccessRateRequestL1 successRateRequestL1;
    public SuccessRateRequestL2 successRateRequestL2;
    public SuccessRateRequestL3 successRateRequestL3;
    private bool requestSent;
    string currentSceneName;
    int infectedCount;

    private void Awake()
    {
        requestSent = false;
    }

    async void Start()
    {
        popUpCanvas = PopUpCanvas.Instance;
        if (infectedText == null)
        {
            Debug.LogError("Infected Text reference is not set!");
        }
    }

    void Update()
    {
        infectedCount = GetInfectedCount();

        // Get the current scene's name
        currentSceneName = SceneManager.GetActiveScene().name;
        // Set the max infected count based on the scene name
        int maxInfectedForScene = 4;
        if (currentSceneName == "Level1" || currentSceneName == "Level2")
        {
            maxInfectedForScene = 2;
        }

        infectedText.text = "Humans Infected : " + infectedCount + " / " + maxInfectedForScene;

        if (infectedCount == 0 && !getIsInitialVirusPresent())
        {
            EndGame("Virus Lost!");

        }
    }

    private bool getIsInitialVirusPresent()
    {
        GameObject virusObject = GameObject.Find("InitialVirus");
        return virusObject != null;
    }

    private void EndGame(string message)
    {
        if (popUpCanvas != null)
        {
            popUpCanvas.ShowPopUp(message);
            Debug.Log("REQUEST STATUS: " + requestSent);
            //Analytics
            SendAnalytics(currentSceneName);
        }
        else
        {
            Debug.LogWarning("PopUpCanvas reference is not assigned!");
        }
    }

    public int GetInfectedCount()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> nvHumans = new List<GameObject>();
        int infectedCount = 0;

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith("HumanNV"))
            {
                nvHumans.Add(obj);
            }
        }

        foreach (GameObject nvHuman in nvHumans)
        {
            Transform virusChild = FindChildWithTagPrefix(nvHuman.transform, "InitialVirus");
            if (virusChild != null)
            {
                infectedCount++;
            }
        }

        return infectedCount;
    }

    private Transform FindChildWithTagPrefix(Transform parent, string prefix)
    {
        foreach (Transform child in parent)
        {
            if (child.tag.StartsWith(prefix))
            {
                return child;
            }

            Transform result = FindChildWithTagPrefix(child, prefix);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    private void SendAnalytics(string currentlevel)
    {
        //Analytics
        if (!requestSent)
        {
            Debug.Log("SENDING: " + infectedCount);
            if (currentlevel == "Level1")
            {
                if (successRateRequestL1 == null)
                {
                    Debug.LogError("successRateRequest is null");
                }
                else
                {
                    successRateRequestL1.Send(infectedCount);
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
                    successRateRequestL2.Send(infectedCount);
                }
            } else if (currentlevel == "Level3")
            {
                if (successRateRequestL3 == null)
                {
                    Debug.LogError("successRateRequest is null");
                }
                else
                {
                    successRateRequestL3.Send(infectedCount);
                }
            }

            requestSent = true;
        }
    }
}