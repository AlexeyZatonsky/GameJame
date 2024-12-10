using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimeManager : SingletonManager<TimeManager>
{
    // UI
    [SerializeField] private TMP_Text timerText;

    // ������ � �����, ������� ����� ���������
    [SerializeField] private float timerDuration = 10f;
    private float currentTime;

    // �������
    public Action OnTimerStart;
    public Action OnTimerStop;

    // ������ �������
    [SerializeField] private bool isTimerRunning = false;

    void Start()
    {

        OnTimerStop += TestLog;
        // �������������
        currentTime = timerDuration;
        timerText.SetText(currentTime.ToString("F2"));  // ��������� �������� � ����������� �� 2 ������ ����� �������
    }

    void Update()
    {
        // ���� ������ ��������
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;  // ��������� ���������� ����� �� ��������� � ���������� �����

            // �������� ����� �������
            timerText.SetText(currentTime.ToString("F2"));

            // ����� ����� �������������
            if (currentTime <= 0)
            {
                StopTimer();  // ���������� ������
            }
        }
    }

    // ����� ��� ������ �������
    public void StartTimer()
    {
        if (!isTimerRunning)
        {
            currentTime = timerDuration;  // ����� ������� �� �������� ��������
            isTimerRunning = true;
            OnTimerStart?.Invoke();
        }
    }

    // ����� ��� ��������� �������
    public void StopTimer()
    {
        if (isTimerRunning)
        {
            isTimerRunning = false;
            currentTime = 0f;  // �������� �����
            OnTimerStop?.Invoke();
            timerText.SetText(currentTime.ToString("F2"));
        }
    }

    // ������ ������ ��� �������� ��������� ������� (�������)
    public void TestLog()
    {
        Debug.Log("������ ����������.");
    }
}
