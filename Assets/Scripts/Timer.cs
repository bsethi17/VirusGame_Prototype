﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Timer : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Pause = !Pause;
    }

    [SerializeField] private Image uiFill;
    // [SerializeField] private Text uiText;

    public int Duration;

    private int remainingDuration;

    private bool Pause;

    private void Start()
    {
        Being(Duration);
    }

    private void Being(int Second)
    {
        remainingDuration = Second;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (remainingDuration >= 0)
        {
            if (!Pause)
            {
                // uiText.text = $"{remainingDuration / 60:00}:{remainingDuration % 60:00}";
                uiFill.fillAmount = Mathf.InverseLerp(0, Duration, remainingDuration);
                if (uiFill.fillAmount >= 0.66)
                {
                    uiFill.color = Color.green;
                }
                else if (uiFill.fillAmount >= 0.36)
                {
                    uiFill.color = Color.yellow;
                }
                else
                {
                    uiFill.color = Color.red;
                }
                remainingDuration--;
                yield return new WaitForSeconds(1f);
            }
            yield return null;
        }
        OnEnd();
    }

    private void OnEnd()
    {
        // uiText.text = "Time's Up!";
        // print("End");
    }
}
