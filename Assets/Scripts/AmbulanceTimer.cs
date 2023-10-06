using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmbulanceTimer : MonoBehaviour
{
    // Start is called before the first frame update
    Image timerBar;

    public GameObject timeupText;
    public float maxTime = 10f;
    float timeLeft;
    void Start()
    {
        timerBar = GetComponent<Image>();
        timeupText.SetActive(false);
        timeLeft = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            timerBar.fillAmount = timeLeft / maxTime;
        }
        else {
            timeupText.SetActive(true);
        }
    }
}
