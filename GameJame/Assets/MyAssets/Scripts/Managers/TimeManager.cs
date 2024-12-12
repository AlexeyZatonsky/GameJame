using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject timerUI;
    [SerializeField] private TMP_Text timerText;

    private float currentTime;

    private GameManager gameManager;

    public event Action OnTimerEnd;

    private Coroutine timerCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);

        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void OnEnable()
    {
        gameManager.OnGameStateChanged += ChangeTimer;
    }

    private void OnDisable()
    {
        gameManager.OnGameStateChanged -= ChangeTimer;
    }

    private void ChangeTimer(GameState state)
    {
        if (state == GameState.FirstTimer)
        {
            StartTimer();
        }
        else if (state == GameState.LastChanceTimer)
        {
            StartLastChanceTimer();
        }
    }

    private IEnumerator TimerCoroutine()
    {
        while (currentTime > 0)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            timerText.SetText($"{minutes:00}:{seconds:00}");
            
            if (currentTime <= gameManager.GetTimeToBed() * 0.25f)
            {
                timerText.color = Color.red;
            }
            else if (currentTime <= gameManager.GetTimeToBed() * 0.5f)
            {
                timerText.color = Color.yellow;
            }

            currentTime -= Time.deltaTime;
            yield return null;
        }
        
        currentTime = 0;
        timerText.SetText("00:00");
        timerText.color = Color.red;
        timerCoroutine = null;

        OnTimerEnd?.Invoke();
    }

    private void StartTimer()
    {
        currentTime = gameManager.GetTimeToBed();
        timerText.color = Color.white;
        timerCoroutine = StartCoroutine(TimerCoroutine());
        SetTimerUI(true);
    }

    private void StartLastChanceTimer()
    {
        currentTime = gameManager.GetTimeForLastChance();
        timerText.color = Color.white;
        timerCoroutine = StartCoroutine(TimerCoroutine());
        SetTimerUI(true);
    }

    public void SetTimerUI(bool isActive)
    {
        timerUI.SetActive(isActive);
    }
}
