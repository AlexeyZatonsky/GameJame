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

    [Header("Body")]
    [SerializeField] private MultiAimConstraint upperChestConstraint;
    [SerializeField] private MultiAimConstraint headConstraint;

    [Header("Settings")]
    [SerializeField] private float weightHandsChangeSpeed = 5f;

    private PlayerInventory playerInventory;
    private Animator animator;

    private Coroutine weightChangeCoroutine;

    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();
        animator = GetComponent<Animator>();
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
        animator.SetLayerWeight(animator.GetLayerIndex("Interact_Layer"), 0f);
    }

    private void HoldItem()
    {
        if (weightChangeCoroutine != null)
        {
            StopCoroutine(weightChangeCoroutine);
        }
        
        weightChangeCoroutine = StartCoroutine(ChangeWeights(0, 1, 0, 1));
        animator.SetLayerWeight(animator.GetLayerIndex("Interact_Layer"), 1f);
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

    public void SetHeadUpperChestWeight(float weight)
    {
        upperChestConstraint.weight = weight;
        headConstraint.weight = weight;
    }
}


