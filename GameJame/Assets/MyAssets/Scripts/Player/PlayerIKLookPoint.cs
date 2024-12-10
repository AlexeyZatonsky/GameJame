using UnityEngine;

public class PlayerIKLookPoint : MonoBehaviour
    {
        [SerializeField] private Transform targetPoint;
        [SerializeField] private float rayDistance = 10f;

        private Camera playerCamera;

        private void Awake()
        {
            InitializeCamera();
        }

        private void Update()
        {
            Vector3 cameraForward = playerCamera.transform.forward;
            Vector3 targetPosition = playerCamera.transform.position + cameraForward * rayDistance;

            targetPoint.position = targetPosition;
        }

        private void InitializeCamera()
        {
            var playerCameraComponent = GetComponentInChildren<PlayerCamera>();
            if (playerCameraComponent != null)
            {
                playerCamera = playerCameraComponent.GetComponentInChildren<Camera>();
            }
        }
    }
