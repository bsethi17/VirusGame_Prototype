using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InfectedCount : MonoBehaviour
{
    public TextMeshProUGUI infectedText;

    void Start()
    {
        if (infectedText == null)
        {
            Debug.LogError("Infected Text reference is not set!");
        }
    }

    void Update()
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
                Debug.Log("hello inside human");
                infectedCount++;
            }
        }

        // Get the current scene's name
        string currentSceneName = SceneManager.GetActiveScene().name;
        // Set the max infected count based on the scene name
        int maxInfectedForScene = 4;
        if (currentSceneName == "Level1" || currentSceneName == "Level2")
        {           
            maxInfectedForScene = 2;
        }
       
            infectedText.text = "Humans Infected : " + infectedCount + " / " + maxInfectedForScene;
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
}