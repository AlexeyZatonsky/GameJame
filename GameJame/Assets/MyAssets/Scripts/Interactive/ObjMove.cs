using System.Collections;
using UnityEngine;

public class ObjMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform pointA; // Начальная точка
    public Transform pointB; // Конечная точка
    public float moveSpeed = 2f; // Скорость перемещения
    public float smoothTime = 0.4f; // Время сглаживания

    [SerializeField] private float dir;
    [SerializeField] private bool _open = true;

    private Coroutine currentCoroutine; // Текущая корутина
    private Vector3 velocity = Vector3.zero; // Вектор сглаживания

    private void Awake()
    {
        // Если точка A не задана, берем текущую позицию объекта
        if (pointA == null)
        {
            GameObject pointAObject = new GameObject("PointA");
            pointAObject.transform.position = transform.position;
            pointA = pointAObject.transform;
        }

        // Если точка B не задана, создаем её рядом с объектом
        if (pointB == null)
        {
            GameObject pointBObject = new GameObject("PointB");
            pointBObject.transform.position = transform.position + new Vector3(dir, 0f, 0f); // Смещаем вправо
            pointB = pointBObject.transform;
        }
    }

    [ContextMenu("Move To Point B")]
    public void MoveToB()
    {
        StartMovement(pointB.position);
        _open = false;
    }

    [ContextMenu("Move To Point A")]
    public void MoveToA()
    {
        StartMovement(pointA.position);
        _open = true;
    }

    public void Move()
    {
        if (_open)
        {
            MoveToB();
        }
        else {
            MoveToA();
        }

    }

    private void StartMovement(Vector3 target)
    {
        // Если есть запущенная корутина, останавливаем её
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        // Запускаем новую корутину для перемещения
        currentCoroutine = StartCoroutine(MoveToPoint(target));
    }

    private IEnumerator MoveToPoint(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f) // Пока объект не достигнет цели
        {
            // Плавное перемещение с использованием SmoothDamp
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime, moveSpeed);
            yield return null;
        }

        // Устанавливаем объект точно в целевую позицию
        transform.position = target;

        // Завершаем корутину
        currentCoroutine = null;
    }

    private void OnDrawGizmos()
    {
        // Отображаем линии между точками A и B для наглядности
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawSphere(pointA.position, 0.1f);
            Gizmos.DrawSphere(pointB.position, 0.1f);
        }
    }
}