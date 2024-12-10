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

    public void EmptyHands()
    {
        leftHandConstraint.weight = 0;
        rightHandConstraint.weight = 0;

        leftHandRotationConstraint.weight = 0;
        rightHandRotationConstraint.weight = 0;
    }

    public void HoldItem()
    {
        leftHandConstraint.weight = 0;
        rightHandConstraint.weight = 1;

        leftHandRotationConstraint.weight = 0;
        rightHandRotationConstraint.weight = 1;
    }
}


