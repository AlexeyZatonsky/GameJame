using UnityEngine;
using System.Collections;

public class ObjMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public Vector3 pointA; // Начальная точка
    public Vector3 pointB; // Конечная точка
    public float moveSpeed = 2f; // Максимальная скорость перемещения
    public float smoothTime = 0.4f; // Время сглаживания движения

    private Coroutine currentCoroutine;
    private Vector3 velocity = Vector3.zero; // Для сглаживания движения
    private float currentSpeed = 0f; // Текущая скорость

    void Start()
    {
        // Начальная позиция остаётся как есть, не телепортируем
    }

    [ContextMenu("Move To Point B")]
    public void MoveToB()
    {
        StartMovement(pointB);
    }

    [ContextMenu("Move To Point A")]
    public void MoveToA()
    {
        StartMovement(pointA);
    }

    private void StartMovement(Vector3 target)
    {
        if (currentCoroutine != null)
        {
            Debug.LogWarning("Движение уже выполняется. Подождите завершения текущей операции.");
            return;
        }
        currentCoroutine = StartCoroutine(MoveToPoint(target));
    }

    private IEnumerator MoveToPoint(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            // Постепенное увеличение скорости для плавного старта
            currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, Time.deltaTime / smoothTime);
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime, currentSpeed);
            yield return null;
        }

        // Устанавливаем точную позицию для устранения дрожания
        transform.position = target;
        currentSpeed = 0f; // Сбрасываем скорость для следующего движения
        currentCoroutine = null; // Сбрасываем текущую корутину
    }
}
