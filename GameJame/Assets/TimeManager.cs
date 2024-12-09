using System;
using UnityEngine;

public class TimeManager : SingletonManager<TimeManager>
{   
    //UI
    [SerializeField] private TextMesh timerText;

    //Player

    //EVents
    public Action OnTimerStart;
    public Action OnTimerStop;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
