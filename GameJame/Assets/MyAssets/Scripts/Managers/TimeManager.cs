using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimeManager : SingletonManager<TimeManager>
{
    // UI
    [SerializeField] private TMP_Text timerText;

    // Таймер и время, которое нужно отсчитать
    [SerializeField] private float timerDuration = 10f;
    private float currentTime;

    // События
    public Action OnTimerStart;
    public Action OnTimerStop;

    // Статус таймера
    [SerializeField] private bool isTimerRunning = false;

    void Start()
    {

        OnTimerStop += TestLog;
        // Инициализация
        currentTime = timerDuration;
        timerText.SetText(currentTime.ToString("F2"));  // Начальное значение с округлением до 2 знаков после запятой
    }

    void Update()
    {
        // Если таймер работает
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;  // Уменьшаем оставшееся время на прошедшее с последнего кадра

            // Оновляем текст таймера
            timerText.SetText(currentTime.ToString("F2"));

            // Когда время заканчивается
            if (currentTime <= 0)
            {
                StopTimer();  // Остановить таймер
            }
        }
    }

    // Метод для старта таймера
    public void StartTimer()
    {
        if (!isTimerRunning)
        {
            currentTime = timerDuration;  // Сброс таймера на исходное значение
            isTimerRunning = true;
            OnTimerStart?.Invoke();
        }
    }

    // Метод для остановки таймера
    public void StopTimer()
    {
        if (isTimerRunning)
        {
            isTimerRunning = false;
            currentTime = 0f;  // Обнуляем время
            OnTimerStop?.Invoke();
            timerText.SetText(currentTime.ToString("F2"));
        }
    }

    // Пример метода для проверки остановки таймера (событие)
    public void TestLog()
    {
        Debug.Log("Таймер остановлен.");
    }
}
