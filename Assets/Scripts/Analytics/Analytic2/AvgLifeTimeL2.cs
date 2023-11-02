using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AvgLifeTimeL2 : MonoBehaviour
{
    public static AvgLifeTimeL2 Instance { get; private set; }
    [SerializeField] private string googleFormURL;
    private long _sessionID;
    private string _humanNumber;
    private float _averageTime;

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

    public void Send(string humanNumber, float averageTime)
    {
        _humanNumber = humanNumber;
        _averageTime = averageTime;
        StartCoroutine(Post(_sessionID.ToString(), _humanNumber.ToString(), _averageTime.ToString()));
    }

    private IEnumerator Post(string sessionID, string humanNumber, string averageTime)
    {
        WWWForm form = new WWWForm();

        // session ID entry, human number, avg time
        form.AddField("entry.1014364818", sessionID);
        form.AddField("entry.185431139", humanNumber);
        form.AddField("entry.1678725862", averageTime);

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
                Debug.Log("Analytic 2 Form upload complete!");
            }
        }
    }
}
