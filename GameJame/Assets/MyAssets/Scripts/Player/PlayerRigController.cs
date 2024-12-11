using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerRigController : MonoBehaviour
{
    [Header("Rig")]
    [SerializeField] private RigBuilder rigBuilder;
    [SerializeField] private Rig rig;

    [Header("Hands")]
    [SerializeField] private TwoBoneIKConstraint leftHandConstraint;
    [SerializeField] private TwoBoneIKConstraint rightHandConstraint;
    [SerializeField] private MultiRotationConstraint leftHandRotationConstraint;
    [SerializeField] private MultiRotationConstraint rightHandRotationConstraint;

    [Header("Settings")]
    [SerializeField] private float weightHandsChangeSpeed = 5f;

    private PlayerInventory playerInventory;

    private Coroutine weightChangeCoroutine;

    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();
    }

    private void OnEnable()
    {
        playerInventory.OnItemChanged += HandleItemChange;
    }

    private void OnDisable()
    {
        playerInventory.OnItemChanged -= HandleItemChange;
    }

    private void HandleItemChange(Loot loot)
    {
        if (loot == null)
        {
            EmptyHands();
        }
        else
        {
            HoldItem();
        }
    }

    private void EmptyHands()
    {
        if (weightChangeCoroutine != null)
        {
            StopCoroutine(weightChangeCoroutine);
        }
        
        weightChangeCoroutine = StartCoroutine(ChangeWeights(0, 0, 0, 0));
    }

    private void HoldItem()
    {
        if (weightChangeCoroutine != null)
        {
            StopCoroutine(weightChangeCoroutine);
        }
        
        weightChangeCoroutine = StartCoroutine(ChangeWeights(0, 1, 0, 1));
    }

    private IEnumerator ChangeWeights(float leftTarget, float rightTarget, float leftRotTarget, float rightRotTarget)
    {
        while (!Mathf.Approximately(leftHandConstraint.weight, leftTarget) || 
               !Mathf.Approximately(rightHandConstraint.weight, rightTarget) ||
               !Mathf.Approximately(leftHandRotationConstraint.weight, leftRotTarget) ||
               !Mathf.Approximately(rightHandRotationConstraint.weight, rightRotTarget))
        {
            leftHandConstraint.weight = Mathf.MoveTowards(leftHandConstraint.weight, leftTarget, weightHandsChangeSpeed * Time.deltaTime);
            rightHandConstraint.weight = Mathf.MoveTowards(rightHandConstraint.weight, rightTarget, weightHandsChangeSpeed * Time.deltaTime);
            
            leftHandRotationConstraint.weight = Mathf.MoveTowards(leftHandRotationConstraint.weight, leftRotTarget, weightHandsChangeSpeed * Time.deltaTime);
            rightHandRotationConstraint.weight = Mathf.MoveTowards(rightHandRotationConstraint.weight, rightRotTarget, weightHandsChangeSpeed * Time.deltaTime);
            
            yield return null;
        }
    }
}


