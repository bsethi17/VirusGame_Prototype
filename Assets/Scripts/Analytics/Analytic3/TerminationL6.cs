using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TerminationL6 : MonoBehaviour
{
    public static TerminationL6 Instance { get; private set; }
    [SerializeField] private string googleFormURL;
    private long _sessionID;

    // 0 represents ends with timer up
    // 1 represents ends with out of bullets
    // 2 represents ends with virus killed by vaccine
    private int _isTimerUp;

    private bool isSubmitted;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _sessionID = DateTime.Now.Ticks;
        isSubmitted = false;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void Send(int isTimerUp)
    {
        if (!isSubmitted)
        {
            _isTimerUp = isTimerUp;
            StartCoroutine(Post(_sessionID.ToString(), _isTimerUp.ToString()));
            isSubmitted = true;
        }
    }

    private IEnumerator Post(string sessionID, string isTimerUp)
    {
        WWWForm form = new WWWForm();

        // session ID entry
        form.AddField("entry.1577356890", sessionID);

        // number of infected humans entry
        form.AddField("entry.1184599555", isTimerUp);

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
                Debug.Log("Form 6 upload complete!");
            }
        }
    }
}
