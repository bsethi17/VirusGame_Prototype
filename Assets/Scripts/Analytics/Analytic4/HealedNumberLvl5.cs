
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealedNumberLvl5 : MonoBehaviour
{
    public static HealedNumberLvl5 Instance { get; private set; }
    [SerializeField] private string googleFormURL;
    private long _sessionID;
    // number of healed humans
    private int _count;

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

    public void Send(int count)
    {
        _count = count;
        StartCoroutine(Post(_sessionID.ToString(), _count.ToString()));
    }

    private IEnumerator Post(string sessionID, string count)
    {
        Debug.Log("Sending analytic 4");
        WWWForm form = new WWWForm();

        // session ID entry, human number, avg time
        form.AddField("entry.92007636", sessionID);
        form.AddField("entry.449946207", count);

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