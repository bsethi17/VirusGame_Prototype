using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SuccessRateRequestL6 : MonoBehaviour
{
    public static SuccessRateRequestL6 Instance { get; private set; }
    [SerializeField] private string googleFormURL;
    private long _sessionID;
    private int _numOfInfectedHumans;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("SuccessRateRequest instance created");
        }
        _sessionID = DateTime.Now.Ticks;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void Send(int numOfInfectedHumans)
    {
        // test
        _numOfInfectedHumans = numOfInfectedHumans;
        StartCoroutine(Post(_sessionID.ToString(), _numOfInfectedHumans.ToString()));
    }

    private IEnumerator Post(string sessionID, string numOfInfectedHumans)
    {
        WWWForm form = new WWWForm();

        // session ID entry
        form.AddField("entry.329282840", sessionID);

        // number of infected humans entry
        form.AddField("entry.2131817782", numOfInfectedHumans);

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
                Debug.Log("Form upload complete!");
            }
        }
    }
}
