using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimeManager : SingletonManager<TimeManager>
{
    // UI
    [SerializeField] private TMP_Text timerText;

    //Vars
    [SerializeField] private float timerDuration = 10f;
    private float currentTime;

    [SerializeField] private bool halfcomplete = false;
    [SerializeField] private bool colorRed = false;
    [SerializeField] private bool haveChance = true;

    public bool HaveChance => haveChance;

    // Events
    public Action OnTimerStart;
    public Action OnTimerStop;
    public Action OnHalfTime;


    //Timer Status
    [SerializeField] private bool isTimerRunning = false;

    void Start()
    {
        currentTime = timerDuration;
    }

    void Update()
    {
        if (!isTimerRunning) { 
            return; 
        }
        else
        {
            // Update Text
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            timerText.SetText($"{minutes:00}:{seconds:00}");
            currentTime -= Time.deltaTime;  // Time less

            if (!colorRed && currentTime <= timerDuration * 0.25f)
            {
                colorRed = true;
                timerText.color = Color.red;
            }
            else if (!halfcomplete && currentTime <= timerDuration * 0.5f)
            {
                halfcomplete = true;
                OnHalfTime?.Invoke();
                timerText.color = Color.yellow;
            }


            if (currentTime <= 0 )
            {
                StopTimer();
            }
        }
    }

    public void StartTimer()
    {
        if (!isTimerRunning)
        {
            //currentTime = timerDuration;  //reset
            isTimerRunning = true;
            OnTimerStart?.Invoke();
            haveChance = false;
        }
    }

    public void StopTimer()
    {
        if (isTimerRunning)
        {
            isTimerRunning = false;
            
            OnTimerStop?.Invoke();
            
        }
    }

    public void AddTime(float amount)
    {
        currentTime += amount;
        if (!haveChance) { return; }
        //currentTime += amount;
        StartTimer();
        //currentTime = Mathf.Clamp(currentTime, 0, timerDuration);
    }


}
