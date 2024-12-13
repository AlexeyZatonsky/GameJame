using System.Collections;
using UnityEngine;

public class ObjMove : InteractiveObject
{
    [Header("Movement Settings")]
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;
    public float smoothTime = 0.4f;
    [SerializeField] private float toMoveCoordinate = 0.01f;

    private Coroutine currentCoroutine;
    private Vector3 velocity = Vector3.zero;
    private float currentSpeed = 0f;

    private void Awake()
    {
        pointA = this.transform; // Устанавливаем текущий объект как точку A

        if (pointB == null) // Проверяем, задана ли точка B
        {
            GameObject pointBObject = new GameObject("PointB");
            pointB = pointBObject.transform;
        }

        
        Vector3 newPosition = new Vector3(
            pointB.localPosition.x + toMoveCoordinate, 
            pointB.localPosition.y, 
            pointB.localPosition.z); // Получаем локальную позицию объекта pointB
        
        pointB.localPosition = newPosition;
        
    }

    [ContextMenu("Move To Point B")]
    public void MoveToB()
    {
        StartMovement(pointB.position); // Передаем позицию точки B
    }

    [ContextMenu("Move To Point A")]
    public void MoveToA()
    {
        StartMovement(pointA.position); // Передаем позицию точки A
    }

    private void StartMovement(Vector3 target)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine); // Останавливаем текущую корутину, если она уже запущена
        }
        
        currentCoroutine = StartCoroutine(MoveToPoint(target)); // Запускаем новую корутину
    }

    private IEnumerator MoveToPoint(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f) // Пока объект не достигнет цели
        {
            currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, Time.deltaTime / smoothTime); // Плавное увеличение скорости
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime, currentSpeed); // Плавное перемещение
            yield return null;
        }

        transform.position = target; // Гарантируем точную позицию
        currentSpeed = 0f; // Сбрасываем скорость
        currentCoroutine = null; // Сбрасываем текущую корутину
    }
}
