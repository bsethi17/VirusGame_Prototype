using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InfectedCount : MonoBehaviour
{
    public TextMeshProUGUI infectedText;
    public PopUpCanvas popUpCanvas;

    // Analytic1
    public SuccessRateRequestL1 successRateRequestL1;
    public SuccessRateRequestL2 successRateRequestL2;
    public SuccessRateRequestL3 successRateRequestL3;
    public SuccessRateRequestL4 successRateRequestL4;
    public SuccessRateRequestL5 successRateRequestL5;
    public SuccessRateRequestL6 successRateRequestL6;
    private bool requestSent1;

    // Analytic3
    public TerminationL1 terminationL1;
    public TerminationL2 terminationL2;
    public TerminationL3 terminationL3;
    public TerminationL4 terminationL4;
    public TerminationL5 terminationL5;
    public TerminationL6 terminationL6;
    private bool requestSent3;

    string currentSceneName;
    int infectedCount;

    private void Awake()
    {
        requestSent1 = false;
        requestSent3 = false;
    }

    void Start()
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

        infectedText.text = infectedCount + " / " + maxInfectedForScene;

        // player runs out of bullets
        if ((infectedCount == 0 && !getIsInitialVirusPresent()) || UIManager.Instance.GlobalBulletCount == 0 && !IsBulletInScene())
        {
            string endMessage = UIManager.Instance.GlobalBulletCount == 0 ? "Out of Bullets!" : "Virus Lost!";
            EndGame(endMessage);
        }
    }

    private bool IsBulletInScene()
    {
        GameObject bulletObject = GameObject.FindWithTag("bullet");
        return bulletObject != null;
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
            // Analytics
            SendAnalytics1(currentSceneName);
            if (message == "Out of Bullets!")
            {
                SendAnalytics3(currentSceneName, 1);
            }
            else if (message == "Virus Lost!")
            {
                // game lose because ofkilled by vaccine
                SendAnalytics3(currentSceneName, 2);
            }
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
                if(nvHuman.tag != "NVHuman1" && (SceneManager.GetActiveScene().name == "Level5" || SceneManager.GetActiveScene().name == "Level6" || SceneManager.GetActiveScene().name == "Level7"))
                {MonoBehaviour[] parentScripts = nvHuman.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in parentScripts)
            {
                if (script.GetType().Name.StartsWith("Circle"))
                {
                    script.enabled = false;
                }
                else if (script.GetType().Name.StartsWith("Infected"))
                {
                    script.enabled = true;
                }
            }}
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

    // Analytics1
    private void SendAnalytics1(string currentlevel)
    {
        if (!requestSent1)
        {
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
            }
            else if (currentlevel == "Level3")
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
            else if (currentlevel == "Level4")
            {
                if (successRateRequestL4 == null)
                {
                    Debug.LogError("successRateRequest is null");
                }
                else
                {
                    successRateRequestL4.Send(infectedCount);
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
                    successRateRequestL5.Send(infectedCount);
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
                    successRateRequestL6.Send(infectedCount);
                }
            }

            requestSent1 = true;
        }
    }

    // Analytics 3
    private void SendAnalytics3(string currentlevel, int status)
    {
        if (!requestSent3)
        {
            if (currentlevel == "Level1")
            {
                if (terminationL1 == null)
                {
                    Debug.LogError("terminationL1 is null");
                }
                else
                {
                    Debug.Log("sending3 in infected count");
                    terminationL1.Send(status);
                }
            }
            else if (currentlevel == "Level2")
            {
                if (terminationL2 == null)
                {
                    Debug.LogError("terminationL2 is null");
                }
                else
                {
                    terminationL2.Send(status);
                }
            }
            else if (currentlevel == "Level3")
            {
                if (terminationL3 == null)
                {
                    Debug.LogError("terminationL3 is null");
                }
                else
                {
                    terminationL3.Send(status);
                }
            }
            else if (currentlevel == "Level4")
            {
                if (terminationL4 == null)
                {
                    Debug.LogError("terminationL4 is null");
                }
                else
                {
                    terminationL4.Send(status);
                }
            }
            else if (currentlevel == "Level5")
            {
                if (terminationL5 == null)
                {
                    Debug.LogError("terminationL5 is null");
                }
                else
                {
                    terminationL5.Send(status);
                }
            }
            else if (currentlevel == "Level6")
            {
                if (terminationL6 == null)
                {
                    Debug.LogError("terminationL6 is null");
                }
                else
                {
                    terminationL6.Send(status);
                }
            }
            requestSent3 = true;
        }
    }
}

