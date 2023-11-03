
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealedNumberLvl6 : MonoBehaviour
{
    public static HealedNumberLvl6 Instance { get; private set; }
    [SerializeField] private string googleFormURL;
    private long _sessionID;
    // number of healed humans
    private string _count;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _sessionID = DateTime.Now.Ticks;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void Send(string count)
    {
        _count = count;
        StartCoroutine(Post(_sessionID.ToString(), _count.ToString()));
    }

    private IEnumerator Post(string sessionID, string count)
    {
        WWWForm form = new WWWForm();

        // session ID entry, human number, avg time
        form.AddField("entry.219798129", sessionID);
        form.AddField("entry.1569260419", count);

        // Send responses and verify result    
        using (UnityWebRequest www = UnityWebRequest.Post(googleFormURL, form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Analytic 4 Form upload complete!");
            }
        }
    }
}