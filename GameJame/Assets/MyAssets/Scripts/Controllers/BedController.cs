using UnityEngine;

public class BedController : MonoBehaviour
{
    [SerializeField] private Transform sleepPosition;
    [SerializeField] private Transform wakeUpPosition;

    public Transform GetSleepPosition => sleepPosition;
    public Transform GetWakeUpPosition => wakeUpPosition;
}
