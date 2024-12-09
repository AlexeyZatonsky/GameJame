using System;
using UnityEngine;

public class TimeManager : SingletonManager<TimeManager>
{   
    //UI
    [SerializeField] private TextMesh timerText;

    //
    [SerializeField] private float timerAmount = 10;

    //Player

    //EVents
    public Action OnTimerStart;
    public Action OnTimerStop;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnTimerStart?.Invoke();
        OnTimerStop += TestLog;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time <= timerAmount)
        {
            Debug.Log(Time.time);
            return;
        }
        else { 
            OnTimerStop?.Invoke();
        }  
        
        
    }

    public void TestLog()
    {
        Debug.Log("TimerStopped");
    }
}
