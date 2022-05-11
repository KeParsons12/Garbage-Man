using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public float startMinutes = 3f;

    private bool isTimerActive = false;
    private float currentTime = 0f;

    private void Start()
    {
        ResetTimer();
        StartTimer();
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        if(isTimerActive == true)
        {
            currentTime -= Time.deltaTime;
        }

        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        timerText.text = "Time: " + time.ToString(@"mm\:ss\.ff");

    }

    public void StartTimer()
    {
        isTimerActive = true;
    }

    public void PauseTimer()
    {
        isTimerActive = false;
    }

    public void ResetTimer()
    {
        //Multiply times 60 for seconds
        currentTime = startMinutes * 60;
    }
}
