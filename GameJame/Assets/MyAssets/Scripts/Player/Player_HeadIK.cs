using UnityEngine;

namespace HorrorGame.PlayerHead_IK
{
    public class Player_HeadIK : MonoBehaviour
    {
        [SerializeField] private Transform lookTarget;
        [Range(0.0f, 1.0f)]
        [SerializeField] private float lookWeight = 1.0f;
        [SerializeField] private float lookDistance = 10.0f;

        [SerializeField] private HumanBodyBones chestBone = HumanBodyBones.Chest;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float chestWeight = 0.5f;

        [SerializeField] private Transform leftHandTarget;
        [SerializeField] private Transform rightHandTarget;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float leftHandWeight = 1f;
        [Range(0.0f, 1.0f)]
        [SerializeField] private float rightHandWeight = 1f;

        private Animator animator;
        private Transform chestTransform;

        private void Start()
        {
            animator = GetComponent<Animator>();
            chestTransform = animator.GetBoneTransform(chestBone);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (lookTarget != null)
            {
                Vector3 lookDirection = lookTarget.forward * lookDistance;
                Vector3 lookAtPosition = lookTarget.position + lookDirection;

                animator.SetLookAtWeight(lookWeight);
                animator.SetLookAtPosition(lookAtPosition);

                Quaternion chestRotation = Quaternion.Lerp(chestTransform.rotation, Quaternion.LookRotation(lookDirection, Vector3.up), chestWeight);

                animator.SetBoneLocalRotation(chestBone, Quaternion.Inverse(chestTransform.rotation) * chestRotation);

                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);

                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
            }
        }
    }
}