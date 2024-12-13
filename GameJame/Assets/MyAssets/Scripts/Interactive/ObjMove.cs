using UnityEngine;
using System.Collections;

public class ObjMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public Vector3 pointA; // ��������� �����
    public Vector3 pointB; // �������� �����
    public float moveSpeed = 2f; // ������������ �������� �����������
    public float smoothTime = 0.4f; // ����� ����������� ��������

    private Coroutine currentCoroutine;
    private Vector3 velocity = Vector3.zero; // ��� ����������� ��������
    private float currentSpeed = 0f; // ������� ��������

    void Start()
    {
        // ��������� ������� ������� ��� ����, �� �������������
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
            Debug.LogWarning("�������� ��� �����������. ��������� ���������� ������� ��������.");
            return;
        }
        currentCoroutine = StartCoroutine(MoveToPoint(target));
    }

    private IEnumerator MoveToPoint(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            // ����������� ���������� �������� ��� �������� ������
            currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, Time.deltaTime / smoothTime);
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime, currentSpeed);
            yield return null;
        }

        // ������������� ������ ������� ��� ���������� ��������
        transform.position = target;
        currentSpeed = 0f; // ���������� �������� ��� ���������� ��������
        currentCoroutine = null; // ���������� ������� ��������
    }
}
