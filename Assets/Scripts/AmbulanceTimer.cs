using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmbulanceTimer : MonoBehaviour
{
    // Start is called before the first frame update
    public float timeStart;
    public Text textBox;
    public Text startBtnText;

    bool timerActive = false;

    // Use this for initialization
    void Start()
    {
        textBox.text = timeStart.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            timeStart += Time.deltaTime;
            textBox.text = timeStart.ToString();
        }
    }
}
