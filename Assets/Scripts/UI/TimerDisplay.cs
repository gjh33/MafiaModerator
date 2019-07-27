using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TimerText;

    public void SetSeconds(float seconds)
    {
        TimeSpan span = TimeSpan.FromSeconds(seconds);
        SetDisplay(span.Minutes, span.Seconds);
    }

    public void SetMinutes(float minutes)
    {
        TimeSpan span = TimeSpan.FromMinutes(minutes);
        SetDisplay(span.Minutes, span.Seconds);
    }

    public void SetDisplay(int minutes, int seconds)
    {
        TimerText.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
    }
}
